using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherScript : MonoBehaviour {

    public WeaponScript wP;

    public Rigidbody Rocket;

    public float FireRate = 10f;
    float Timer = 0f;

    public int ammo = 10;
    int timesFired = 0;
    // need a way to reset for new gun or somehing

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (wP.FireWorkActive == true)
        {
            if (wP.Firing == true)
            {
                if (timesFired != ammo)
                {
                    if (Timer <= 0)
                    {
                        Rigidbody Clone;
                        Clone = Instantiate(Rocket, transform.position, transform.rotation);

                        Clone.velocity = transform.TransformDirection(Vector3.forward * 25);
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
