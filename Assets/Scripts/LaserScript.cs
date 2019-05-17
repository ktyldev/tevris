using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : Pickupable {

    public ParticleSystem particle;
    public ParticleSystem particle2;
    public LineRenderer laser;

    bool playParticle = false;

    public bool worldSpace = true;

    Renderer rend;
    IEnumerator fireLaser;

    public float ammo = 10f;
    float timeFired = 0.0f;
    // need a way to reset for new gun or somehing
    // can call resetAmmo() to reload

    void Start () {
        rend = laser.gameObject.GetComponent<Renderer>();

        laser.enabled = false;

        onActivate.AddListener(ActivateLaser);
        onDeactivate.AddListener(DeactivateLaser);
	}

	void Update () {
        timeFired += Time.deltaTime;
        rend.material.mainTextureOffset = new Vector2(0, Time.time);
        laser.SetPosition(0, laser.transform.position);
        laser.SetPosition(1, laser.transform.position + laser.transform.rotation * Vector3.forward * 1000.0f);
	}

    void ActivateLaser()
    {
        fireLaser = FireLaser();
        StartCoroutine(fireLaser);

        laser.enabled = true;
        particle.Play();
        particle2.Play();
    }

    void DeactivateLaser()
    {
        if (fireLaser != null)
            StopCoroutine(fireLaser);

        laser.enabled = false;
        particle.Stop();
        particle2.Stop();
    }

    IEnumerator FireLaser()
    {
        while (true)
        {
            if (timeFired <= ammo)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log(hit.collider.gameObject.ToString());
                    if (hit.rigidbody)
                    {
                        //do  damage or something here//
                    }
                }
                else
                {
                    laser.SetPosition(1, ray.GetPoint(100));
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void resetAmmo() {

        timeFired = 0.0f;
    }
}
