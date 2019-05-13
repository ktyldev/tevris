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
            new VRPickerData( null, XRNode.RightHand ),
            new VRPickerData( null, XRNode.LeftHand )
        };

        foreach (var data in nodeData_)
        {
            nodes_.Add(new VRPickerNode(data, basePosition));
        }

        Debug.Log(System.String.Join(", ", Input.GetJoystickNames()));
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.JoystickButton0))
            Debug.Log("pressed!");

        float x = (
            Input.GetAxis(GameConstants.OTTriggerLeftHand) * 1.0f +
            Input.GetAxis(GameConstants.OTTriggerLeftIndex) * 2.0f +
            Input.GetAxis(GameConstants.OTTriggerRightHand) * 4.0f +
            Input.GetAxis(GameConstants.OTTriggerRightIndex) * 8.0f
        );

        Debug.Log(x);

        foreach(var pickerNode in nodes_)
        {
            pickerNode.Update();
        }
	}
}
