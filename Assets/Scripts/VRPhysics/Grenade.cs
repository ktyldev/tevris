using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class Grenade : Pickupable {

    bool isActive = false;

    void Start()
    {
        // when you let go, the grenade activates.
        onPutDown.AddListener(() => isActive = true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        // we're exploding!
        Debug.Log("boom!");
        this.GetComponent<Explodable>().Explode();
    }
}
