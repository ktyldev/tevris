using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public ParticleSystem particle;
    public ParticleSystem particle2;
    bool playParticle = false;

    public bool worldSpace = true;

    LineRenderer Line;
    Renderer rend;

    public WeaponScript wP;

    public float ammo = 10f;
    float timeFired = 0.0f;
    // need a way to reset for new gun or somehing

    void Start () {
        rend = GetComponent<Renderer>();
        Line = gameObject.GetComponent<LineRenderer>();
        Line.enabled = false;
	}
	

	void Update () {

		if(wP.laserActive == true)
        {
            if (wP.Firing == true)
            {
                StopCoroutine("FireLaser");
                StartCoroutine("FireLaser");
            }
        }
        if (playParticle == true)
        {
            particle.Play();
            particle2.Play();
        }
        if(wP.Firing == true)
        {
            timeFired += Time.deltaTime;
        }
	}

    IEnumerator FireLaser()
    {
        if(wP.laserActive == true)
        {
            Line.enabled = true;
            while (wP.Firing == true)
            {
                if (timeFired <= ammo)
                {
                    if (particle.isPlaying)
                    {
                        playParticle = false;
                    }
                    else
                    {
                        playParticle = true;
                    }
                    rend.material.mainTextureOffset = new Vector2(0, Time.time);
                    Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;

                    Line.SetPosition(0, ray.origin);
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        Line.SetPosition(1, hit.point);
                        if (hit.rigidbody)
                        {
                            //do something here//
                        }
                    }

                    else
                        Line.SetPosition(1, ray.GetPoint(100));
                }
                yield return null;
            }
            Line.enabled = false;
            if(particle.isPlaying == true)
            {
                particle.Stop();
                particle2.Stop();
            }
        }
    }
}
