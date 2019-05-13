using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRInput : MonoBehaviour {
    public Transform rightHandTracker;
    public Transform leftHandTracker;
    public Vector3 basePosition;

	// Use this for initialization
	void Start () {
        basePosition = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 leftHandPos = InputTracking.GetLocalPosition(XRNode.LeftHand);
        Vector3 rightHandPos = InputTracking.GetLocalPosition(XRNode.RightHand);

        leftHandTracker.SetPositionAndRotation(
            basePosition + leftHandPos,
            InputTracking.GetLocalRotation(XRNode.LeftHand)
        );

        rightHandTracker.SetPositionAndRotation(
            basePosition + rightHandPos,
            InputTracking.GetLocalRotation(XRNode.RightHand)
        );

        rightHandTracker.gameObject.SetActive(rightHandPos != Vector3.zero);
        leftHandTracker.gameObject.SetActive(leftHandPos != Vector3.zero);

        Debug.Log(rightHandPos);
	}
}
