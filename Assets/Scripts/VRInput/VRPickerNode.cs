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

    private bool isHeld_;
    private Pickupable heldItem_;

    private VRPickerData data_;

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

        (heldItem_ != null ? heldItem_.transform : visuals_).SetPositionAndRotation(
            basePosition_ + InputTracking.GetLocalPosition(data_.Node),
            InputTracking.GetLocalRotation(data_.Node)
        );

        visuals_.gameObject.SetActive(
            InputTracking.GetLocalPosition(data_.Node) != Vector3.zero &&
            heldItem_ == null
        );
    }

    public void PickUp()
    {
        Debug.Log("[" + name_ + "]: picking up");
        var pickup = GameObject.FindGameObjectWithTag("Player").GetComponent<Pickupable>();
        if (pickup.PickUp())
            heldItem_ = pickup;
    }

    private void LetGo()
    {
        Debug.Log("[" + name_ + "]: letting go");
        heldItem_.PutDown();
        heldItem_ = null;
    }
}
