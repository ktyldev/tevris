using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    public WeaponScript wP;

    public Rigidbody Bullet;

    public float FireRate = 10f;
    float Timer = 0f;

    public int ammo = 10;
    int timesFired = 0;
    // need a way to reset for new gun or somehing

    void Update()
    {
        if (wP.gunActive == true)
        {
            if (wP.Firing == true)
            {
                if (timesFired != ammo)
                {
                    if (Timer <= 0)
                    {
                        Rigidbody Clone;
                        Clone = Instantiate(Bullet, transform.position, transform.rotation);

                        Clone.velocity = transform.TransformDirection(Vector3.forward * 30);
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
}
