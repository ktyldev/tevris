using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRSettings : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 2.0f)]
    private float renderScale_ = 1.0f;

    void Update () {
        if (XRSettings.eyeTextureResolutionScale != renderScale_)
        {
            XRSettings.eyeTextureResolutionScale = renderScale_;
        }
	}
}
