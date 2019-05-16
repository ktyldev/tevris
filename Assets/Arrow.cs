using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class Arrow : MonoBehaviour
{
    public bool IsActive { get; set; }

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsActive)
            return;

        if (collision.gameObject.GetComponentInParent<Tetromino>() == null)
            return;

        var explodable = GetComponent<Explodable>();

        var velocity = collision.relativeVelocity.magnitude;

        explodable.Explode();
    }
}
