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

    private Vector3 handPosition_;
    private Vector3 handVelocity_;

    private bool laserMode_;
    private bool isHeld_;
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

        var position = basePosition_ + InputTracking.GetLocalPosition(data_.Node);
        var rotation = InputTracking.GetLocalRotation(data_.Node);
        visuals_.SetPositionAndRotation(position, rotation);

        bool showLine = laserMode_ && heldItem_ == null && !noTracking;
        pickupRenderer_.enabled = showLine;
        pickupRenderer_.SetPositions(new []
        {
            position,
            position + (rotation * Vector3.forward) * 1000.0f
        });

        if (heldRigidbody_ != null)
        {
            heldRigidbody_.MovePosition(position);
            heldRigidbody_.MoveRotation(rotation);
        }
        else if (heldItem_ != null)
        {
            heldItem_.transform.SetPositionAndRotation(position, rotation);
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
        Debug.Log("[" + name_ + "]: picking up");
        PickUpProximity();
    }

    private void PickUpLaser()
    {
    }

    private void PickUpProximity()
    {
        int layerMask = 1 << GameConstants.PickupLayer;
        Collider[] hitColliders = Physics.OverlapSphere(
            WorldPosition,
            GameConstants.VRPickupRadius,
            layerMask
        );

        Pickupable pickedObject;
        if (hitColliders.Length == 0) return;
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

        if (!pickedObject.PickUp()) return;

        heldItem_ = pickedObject;
        heldRigidbody_ = pickedObject.gameObject.GetComponent<Rigidbody>();
    }

    public void LetGo()
    {
        Debug.Log("[" + name_ + "]: letting go");
        if (heldItem_ == null) return;

        // transfer motion to our object
        heldRigidbody_.velocity = handVelocity_ * GameConstants.VRPickupVelocityTransfer;

        heldItem_.PutDown();
        heldItem_ = null;
        heldRigidbody_ = null;
    }
}
