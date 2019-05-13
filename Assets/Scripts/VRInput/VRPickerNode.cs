using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public struct VRPickerData
{
    public GameObject VisualPrefab;
    public XRNode Node;

    public VRPickerData(GameObject visualPrefab, XRNode node)
    {
        VisualPrefab = visualPrefab;
        Node = node;
    }
}

public class VRPickerNode {
    private GameObject baseObject_;
    private Vector3 basePosition_;
    private SphereCollider pickupTrigger_;
    private Transform visuals_;

    private VRPickerData data_;

    public VRPickerNode(VRPickerData data, Vector3 basePosition)
    {
        data_ = data;
        basePosition_ = basePosition;

        baseObject_ = new GameObject(InputTracking.GetNodeName((ulong)data.Node));

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
        if (visuals_ == null) return;
        visuals_.SetPositionAndRotation(
            basePosition_ + InputTracking.GetLocalPosition(data_.Node),
            InputTracking.GetLocalRotation(data_.Node)
        );

        visuals_.gameObject.SetActive(
            InputTracking.GetLocalPosition(data_.Node) != Vector3.zero
        );
    }
}
