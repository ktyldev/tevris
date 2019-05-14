using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Destroyable))]
public class Explodable : MonoBehaviour {
    [SerializeField]
    private float explosionRadius = 5.0f;

    [SerializeField]
    private float explosionStrength = 100.0f;

    public void Explode(float time = 0.0f)
    {
        if (time > 0.0f)
        {
            StartCoroutine(ExplodeIn(time));
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            ~0 // all objects
        );

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;

            var diff = transform.position - collider.transform.position;
            var amt = 1.0f / Mathf.Pow((diff.magnitude + 1f), 2);

            var rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 explosionForce = explosionStrength * amt * (diff.normalized);

                rb.AddForce(explosionForce);
            }

            var explodable = collider.GetComponent<Explodable>();
            if (explodable != null)
            {
                explodable.Explode(diff.magnitude / GameConstants.VRSoundSpeed);
            }

            var destroyable = collider.GetComponent<Destroyable>();
            if (destroyable != null)
            {
                destroyable.Destroy(DestructionMethod.Explosion);
            }
        }

        // now, destroy ourselves!
        gameObject
            .GetComponent<Destroyable>()
            .Destroy(DestructionMethod.Explosion);
    }

    public IEnumerator ExplodeIn(float time)
    {
        yield return new WaitForSeconds(time);
        this.Explode();
    }
}
