using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkScript : MonoBehaviour {

    public WeaponScript wP;

    public Rigidbody rocket;

    public float FireRate = 30f;
    float Timer = 0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (wP.gunActive == true)
        {
            if (wP.Firing == true)
            {

                if (Timer <= 0)
                {
                    Rigidbody Clone;
                    Clone = Instantiate(rocket, transform.position, transform.rotation);

                    Clone.velocity = transform.TransformDirection(Vector3.forward * 300);
                    Timer = FireRate;
                    GameObject.Destroy(Clone, 6f);
                    wP.Firing = false;
                }

            }
        }
        Timer -= Time.deltaTime;
        wP.Firing = false;
    }
}
