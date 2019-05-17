using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : Pickupable {

    public ParticleSystem particle;
    public ParticleSystem particle2;
    public LineRenderer laser;

    private Collider collider_;
    public TetrisGridCollision gridCollision_;

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
        collider_ = gridCollision_.GetCollider();

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
        yield return new WaitForSeconds(GameConstants.LaserStartDelay);
        while (true)
        {
            if (timeFired <= ammo)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit? hit = null;

                var hits = Physics.RaycastAll(ray, 50, -1, QueryTriggerInteraction.Collide);
                foreach (var h in hits)
                {
                    if (h.collider == collider_)
                    {
                        hit = h;
                        break;
                    }
                }

                if (hit.HasValue)
                {
                    gridCollision_.SpaceCast(hit.Value);
                }
            }
            yield return new WaitForSeconds(GameConstants.LaserPlaceDelay);
        }
    }

    void resetAmmo() {

        timeFired = 0.0f;
    }
}
