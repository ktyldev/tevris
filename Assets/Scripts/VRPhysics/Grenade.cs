using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class Grenade : Pickupable {

    bool isActive_ = false;

    void Start()
    {
        // when you let go, the grenade activates.
        onPutDown.AddListener(() => isActive_ = true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActive_) return;

        var explodable = this.GetComponent<Explodable>();

        float velocity = collision.relativeVelocity.magnitude;

        if (velocity < GameConstants.GrenadeExplosionVelocity)
        {
            // deactivate if we're not dropped with enough force
            isActive_ = false;
            return;
        }

        explodable.Explode();
    }
}
