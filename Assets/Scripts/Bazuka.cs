using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazuka : MonoBehaviour {

    public WeaponScript wP;

    public Rigidbody Missile;

    public float FireRate = 5f;
    float Timer = 0f;

    public int ammo = 5;
    int timesFired = 0;
    // need a way to reset for new gun or somehing
    // can call resetAmmo() to reload

    void Update()
    {
        if (wP.BazukaActive == true)
        {
            if (wP.Firing == true)
            {
                if (timesFired != ammo)
                {
                    if (Timer <= 0)
                    {
                        Rigidbody Clone;
                        Clone = Instantiate(Missile, transform.position, transform.rotation);

                        Clone.velocity = transform.TransformDirection(Vector3.forward * 50);
                        Timer = FireRate;
                        wP.Firing = false;
                        timesFired = timesFired + 1;
                    }
                }


            }
        }
        Timer -= Time.deltaTime;
        wP.Firing = false;
    }

    void resetAmmo()
    {

        timesFired = 0;

    }
}
