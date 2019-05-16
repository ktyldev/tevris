using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Extensions;

public struct VRPickerData
{
    public GameObject VisualPrefab;
    public XRNode Node;
    public Material LineMaterial;
    public string PickupAxis;
    public string LineAxis;
    public string LineButton;

    public VRPickerData(
            GameObject visualPrefab, 
            XRNode node, 
            Material lineMaterial,
            string pickupAxis, 
            string lineAxis, 
            string lineButton
    ) {
        VisualPrefab = visualPrefab;
        Node = node;
        LineMaterial = lineMaterial;
        PickupAxis = pickupAxis;
        LineAxis = lineAxis;
        LineButton = lineButton;
    }
}

public class VRPickerNode {
    private string name_;
    private GameObject baseObject_;
    private Vector3 basePosition_;
    private Transform visuals_;
    private LineRenderer pickupRenderer_;
    private const int layerMask_ = 1 << GameConstants.PickupLayer;

    private Vector3 handPosition_;
    private Quaternion handRotation_;
    private Vector3 handVelocity_;

    private Vector3 worldPosition_;

    private bool laserMode_ = false;
    private bool isHeld_ = false;
    private bool isAtHand_ = false;

    private Pickupable heldItem_;
    private Rigidbody heldRigidbody_;

    private VRPickerData data_;

    public Vector3 WorldPosition
        => handPosition_ + basePosition_;

    public VRPickerNode(VRPickerData data, Vector3 basePosition)
    {
        data_ = data;
        basePosition_ = basePosition;
        name_ = data_.Node.ToString();

        baseObject_ = new GameObject(name_);

        if (data.VisualPrefab != null)
        {
            var inst = GameObject.Instantiate(data.VisualPrefab);
            visuals_ = inst.transform;
            visuals_.parent = baseObject_.transform;
        }

        pickupRenderer_ = baseObject_.AddComponent<LineRenderer>();
        pickupRenderer_.material = data_.LineMaterial;
        pickupRenderer_.startWidth = 0.02f;
        pickupRenderer_.endWidth = 0.02f;
        pickupRenderer_.startColor = Color.cyan;
        pickupRenderer_.endColor = Color.cyan;
    }

    void SetBasePosition(Vector3 basePosition)
        => basePosition_ = basePosition;

    public void Update()
    {
        bool nowHeld = Input.GetAxis(data_.PickupAxis) != 0.0f;
        bool noTracking = InputTracking.GetLocalPosition(data_.Node) == Vector3.zero;

        laserMode_ =
            Input.GetAxis(data_.LineAxis) == 0.0f  &&
            !Input.GetButton(data_.LineButton);


        // has hold state changed?
        if (nowHeld != isHeld_)
        {
            if (nowHeld) PickUp();
            else LetGo();

            isHeld_ = nowHeld;
        }

        if (heldRigidbody_ != null) heldRigidbody_.velocity = Vector3.zero;

        worldPosition_ = basePosition_ + InputTracking.GetLocalPosition(data_.Node);
        handRotation_ = InputTracking.GetLocalRotation(data_.Node);
        visuals_.SetPositionAndRotation(worldPosition_, handRotation_);

        bool showLine = laserMode_ && heldItem_ == null && !noTracking;
        pickupRenderer_.enabled = showLine;
        pickupRenderer_.SetPositions(new []
        {
            worldPosition_,
            worldPosition_ + (handRotation_ * Vector3.forward) * 1000.0f
        });

        if (heldRigidbody_ != null)
        {
            var smoothPosition = Vector3.Lerp(
                heldRigidbody_.transform.position,
                worldPosition_,
                isAtHand_
                    ? GameConstants.VRHandHeldLerp
                    : GameConstants.VRHandFloatLerp
            );

            float dist
                = Vector3.Distance(
                    heldRigidbody_.transform.position,
                    worldPosition_
                );

            if (!isAtHand_ && dist <= GameConstants.VRHandSnapThreshold)
            {
                isAtHand_ = true;
            }

            heldRigidbody_.MovePosition(smoothPosition);
            heldRigidbody_.MoveRotation(handRotation_);
        }
        else if (heldItem_ != null)
        {
            heldItem_.transform.SetPositionAndRotation(worldPosition_, handRotation_);
        }

        visuals_.gameObject.SetActive(
            !noTracking &&
            heldItem_ == null
        );

        var newPosition = InputTracking.GetLocalPosition(data_.Node);
        var newVelocity = (newPosition - handPosition_) / Time.deltaTime;
        handVelocity_ = Vector3.Lerp(handVelocity_, newVelocity, 0.2f);
        handPosition_ = newPosition;
    }

    public void PickUp()
    {
        // try a proxy pickup regardless
        if (PickUpProximity()) return;

        // if laser is shown, try a laser pickup
        if (laserMode_) PickUpLaser();
    }

    private bool PickUpLaser()
    {
        float distance = GameConstants.VRLaserPickupMaxDistance;
        float alpha = Mathf.Deg2Rad * GameConstants.VRLaserPickupMaxAngle;
        float radius = distance * Mathf.Tan(alpha);

        Vector3 direction = handRotation_ * Vector3.forward;

        RaycastHit hit;

        bool hasHit
            = PhysicsExtensions.ConeCastAngle(
                worldPosition_,
                GameConstants.VRLaserPickupMaxAngle,
                direction,
                out hit,
                GameConstants.VRLaserPickupMaxDistance,
                layerMask_,
                QueryTriggerInteraction.Collide
            );

        if (!hasHit) return false;

        var hitPickup = hit.collider.GetComponent<Pickupable>();
        if (hitPickup == null || !hitPickup.PickUp()) return false;

        heldItem_ = hitPickup;
        heldRigidbody_ = hitPickup.GetComponent<Rigidbody>();
        isAtHand_ = false;

        return true;
    }

    private bool PickUpProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            WorldPosition,
            GameConstants.VRPickupRadius,
            layerMask_,
            QueryTriggerInteraction.Collide
        );

        Pickupable pickedObject;
        if (hitColliders.Length == 0) return false;
        else if (hitColliders.Length == 1)
        {
            pickedObject = hitColliders[0].gameObject.GetComponent<Pickupable>();
        }
        else
        {
            float smallestDistance = Mathf.Infinity;
            Collider currentClosest = null;
            foreach(var collider in hitColliders)
            {
                float distance = Vector3.Distance(WorldPosition, collider.transform.position);
                if (distance < smallestDistance)
                {
                    currentClosest = collider;
                    smallestDistance = distance;
                }
            }

            pickedObject = currentClosest.GetComponent<Pickupable>();
        }

        if (!pickedObject.PickUp()) return false;

        heldItem_ = pickedObject;
        heldRigidbody_ = pickedObject.gameObject.GetComponent<Rigidbody>();
        isAtHand_ = true;

        return true;
    }

    public void LetGo()
    {
        if (heldItem_ == null) return;

        if (heldRigidbody_ != null)
        {
            float minBound = GameConstants.VRVelocityScaleBounds.x;
            float maxBound = GameConstants.VRVelocityScaleBounds.y;

            float velocityScaleLerpValue
                = Mathf.Clamp01((handVelocity_.magnitude - minBound) / (maxBound - minBound));

            float velocityScale
                = Mathf.Lerp(
                    GameConstants.VRVelocityScaleValues.x,
                    GameConstants.VRVelocityScaleValues.y,
                    velocityScaleLerpValue
                );

            Vector3 scaledVelocity = handVelocity_ * velocityScale;

            Debug.Log(handVelocity_ + " => " + scaledVelocity.magnitude);

            // transfer motion to our object
            heldRigidbody_.velocity = scaledVelocity;
        }

        heldItem_.PutDown();
        isAtHand_ = false;
        heldItem_ = null;
        heldRigidbody_ = null;
    }
}
