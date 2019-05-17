using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : Pickupable {

    public bool gunActive = false;
    public bool laserActive = false;
    public bool FireWorkActive = false;
    public bool BazukaActive = false;

    public bool Firing = true;

    public int currentWeapon = 1;

    void Start()
    {
        onActivate.AddListener(() => Firing = true);
        onDeactivate.AddListener(() => Firing = false);
    }

    void Update () {
        if (Input.GetKeyDown("1"))
        {
            currentWeapon = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            currentWeapon = 0;
        }
        else if (Input.GetKeyDown("3"))
        {
            currentWeapon = 2;
        }
        else if (Input.GetKeyDown("4"))
        {
            currentWeapon = 3;
        }

        switch (currentWeapon)
        {
            case 0:
                gunActive = true;
                laserActive = false;
                FireWorkActive = false;
                BazukaActive = false;
                break;
            case 1:
                gunActive = false;
                laserActive = true;
                FireWorkActive = false;
                BazukaActive = false;
                break;
            case 2:
                gunActive = false;
                laserActive = false;
                FireWorkActive = true;
                BazukaActive = false;
                break;
            case 3:
                gunActive = false;
                laserActive = false;
                FireWorkActive = false;
                BazukaActive = true;
                break;
        }

	}
}
