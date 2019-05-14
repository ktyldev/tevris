using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Extensions;

public struct VRPickerData
{
    public GameObject VisualPrefab;
    public XRNode Node;
    public string PickupAxis;

    public VRPickerData(GameObject visualPrefab, XRNode node, string pickupAxis)
    {
        VisualPrefab = visualPrefab;
        Node = node;
        PickupAxis = pickupAxis;
    }
}

public class VRPickerNode {
    private string name_;
    private GameObject baseObject_;
    private Vector3 basePosition_;
    private SphereCollider pickupTrigger_;
    private Transform visuals_;

    private Vector3 handPosition_;
    private Vector3 handVelocity_;

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
    }

    void SetBasePosition(Vector3 basePosition)
        => basePosition_ = basePosition;

    public void Update()
    {
        bool nowHeld = Input.GetAxis(data_.PickupAxis) != 0.0f;

        // has hold state changed?
        if (nowHeld != isHeld_)
        {
            if (nowHeld) PickUp();
            else LetGo();

            isHeld_ = nowHeld;
        }

        if (visuals_ == null) return;
        if (heldRigidbody_ != null) heldRigidbody_.velocity = Vector3.zero;

        (heldItem_ != null ? heldItem_.transform : visuals_).SetPositionAndRotation(
            basePosition_ + InputTracking.GetLocalPosition(data_.Node),
            InputTracking.GetLocalRotation(data_.Node)
        );

        visuals_.gameObject.SetActive(
            InputTracking.GetLocalPosition(data_.Node) != Vector3.zero &&
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
                float distance = (WorldPosition - collider.transform.position).magnitude;
                if (distance < smallestDistance)
                {
                    currentClosest = collider;
                    smallestDistance = distance;
                }
            }

            pickedObject = currentClosest.GetComponent<Pickupable>();
        }

        heldItem_ = pickedObject;
        heldRigidbody_ = pickedObject.gameObject.GetComponent<Rigidbody>();
    }

    private void LetGo()
    {
        Debug.Log("[" + name_ + "]: letting go");
        if (heldItem_ == null) return;
        heldRigidbody_.velocity = handVelocity_;

        heldItem_.PutDown();
        heldItem_ = null;
        heldRigidbody_ = null;
    }
}
