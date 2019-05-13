using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRInput : MonoBehaviour {
    public GameObject rightHandTracker_;
    public GameObject leftHandTracker_;

    public List<VRPickerNode> nodes_
        = new List<VRPickerNode>();
    private List<VRPickerData> nodeData_
        = new List<VRPickerData>();

	// Use this for initialization
	void Start () {
        Vector3 basePosition = Camera.main.transform.position;
        nodeData_ = new List<VRPickerData>()
        {
            new VRPickerData( rightHandTracker_, XRNode.RightHand ),
            new VRPickerData( leftHandTracker_, XRNode.LeftHand )
        };

        foreach (var data in nodeData_)
        {
            nodes_.Add(new VRPickerNode(data, basePosition));
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach(var pickerNode in nodes_)
        {
            pickerNode.Update();
        }
	}
}
