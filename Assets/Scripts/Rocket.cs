using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public ParticleSystem Explosion;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        GameObject exp = Instantiate(Explosion.gameObject, pos, rot) as GameObject;
        ParticleSystem parts = exp.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration;
        Destroy(gameObject);
        Destroy(exp, totalDuration);

    }
}
