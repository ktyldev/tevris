using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowHandle : Pickupable
{
    private Vector3 restingPosition_;

    void Start()
    {
        Debug.Log("handle start");
        onPickUp.AddListener(Activate);
        onPutDown.AddListener(Shoot);

        restingPosition_ = transform.localPosition;
        this.gameObject.SetActive(false);
    }

    void Activate()
    {
        Debug.Log("picked up handle!");
        transform.localPosition = restingPosition_;
    }

    void Shoot()
    {
        Debug.Log("shot projectile");
        transform.localPosition = restingPosition_;
    }
}
