using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public bool gunActive = false;
    public bool laserActive = false;
    public bool FireWorkActive = false;

    public bool Firing;

    public int currentWeapon = 1;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Firing = true;
        }
        else
        {
            Firing = false;
        }

        if (Input.GetKeyDown("1"))
        {
            currentWeapon = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            currentWeapon = 0;
        }

        switch (currentWeapon)
        {
            case 0:
                gunActive = true;
                laserActive = false;
                break;
            case 1:
                gunActive = false;
                laserActive = true;
                break;
        }

	}
}
