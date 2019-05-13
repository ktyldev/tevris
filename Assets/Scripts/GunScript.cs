using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    public WeaponScript wP;

    public Rigidbody Bullet;

    public float FireRate = 1f;
    float Timer = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (wP.Firing == true)
        {
            Timer -= Time.deltaTime;
            if (Timer >= 0)
            {
                Debug.Log("shooting");
                Rigidbody Clone;
                Clone = Instantiate(Bullet, transform.position, transform.rotation);

                Clone.velocity = transform.TransformDirection(Vector3.forward * 10);
                Timer = FireRate;

            }
            else
            {
                Debug.Log("Reloading");
            }
        }
    }
}
