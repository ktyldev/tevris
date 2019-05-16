using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class Bowstring : Pickupable
{
    public Transform arrowSpawn;

    [SerializeField]
    private GameObject projectile_;

    private GameObject projectileInstance_;
    private Transform projectileRoot_;

    private Vector3 restingPosition_;

    void Start()
    {
        Debug.Log("handle start");
        onPickUp.AddListener(Activate);
        onPutDown.AddListener(Shoot);

        //restingPosition_ = transform.localPosition;
        restingPosition_ = Vector3.zero;

        if (projectile_ == null)
            throw new System.Exception("projectile not set in bow!");

        var root = new GameObject("Bow Projectiles");

        projectileRoot_ = root.transform;

        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (projectileInstance_ != null)
        {
            projectileInstance_.transform.position = transform.position;
            projectileInstance_.transform.LookAt(transform.parent.position);
        }
    }

    void Activate()
    {
        Debug.Log("picked up handle!");
        transform.localPosition = restingPosition_;

        projectileInstance_ = Instantiate(projectile_);
        projectileInstance_.transform.parent = projectileRoot_;
        projectileInstance_.transform.position = transform.position;
    }

    void Shoot()
    {
        Destroy(projectileInstance_);
        projectileInstance_ = null;
        
        this.Instantiate<Rigidbody>(
            projectile_, 
            arrowSpawn.position, 
            arrowSpawn.rotation, 
            null,
            rb => rb.AddForce(rb.transform.forward * 10000.0f));

        transform.localPosition = restingPosition_;
    }
}
