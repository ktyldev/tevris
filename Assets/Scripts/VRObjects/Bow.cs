using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Pickupable
{
    [SerializeField]
    private Pickupable handle_;

	// Use this for initialization
	void Start () {
        onPutDown.AddListener(DeactivateHandle);
        onPickUp.AddListener(ActivateHandle);
	}

    void ActivateHandle()
    {
        handle_.gameObject.SetActive(true);
    }

    void DeactivateHandle()
    {
        if (handle_.IsPickedUp) handle_.PutDown();
        handle_.gameObject.SetActive(false);
    }
}
