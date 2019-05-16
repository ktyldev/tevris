using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Pickupable
{
    [SerializeField]
    private Pickupable handle_;
    private Collider collider_;

	// Use this for initialization
	void Start () {
        onPutDown.AddListener(DeactivateHandle);
        onPickUp.AddListener(ActivateHandle);
        collider_ = this.GetComponent<Collider>();
	}

    void ActivateHandle()
    {
        handle_.gameObject.SetActive(true);
        collider_.isTrigger = true;
        canExplode_ = false;
    }

    void DeactivateHandle()
    {
        if (handle_.IsPickedUp) handle_.PutDown();
        collider_.isTrigger = false;
        handle_.gameObject.SetActive(false);
        canExplode_ = true;
    }

    private bool canExplode_ = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (!canExplode_)
            return;

        if (collision.gameObject.GetComponentInParent<Tetromino>() != null)
        {
            var explodable = GetComponent<Explodable>();
            var velocity = collision.relativeVelocity.magnitude;

            explodable.Explode();
        }

        canExplode_ = false;
    }
}
