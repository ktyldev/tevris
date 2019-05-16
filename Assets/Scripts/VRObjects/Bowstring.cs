using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class Bowstring : Pickupable
{
    public Transform arrowSpawn;
    // minimum distance string needs to be pulled back in order to fire
    public float minDrawDistance;
    public Transform top;
    public Transform bot;

    [SerializeField]
    private GameObject projectile_;

    private GameObject projectileInstance_;
    private Transform projectileRoot_;
    private Vector3 restingPosition_;
    private LineRenderer line_;

    void Start()
    {
        line_ = GetComponent<LineRenderer>();

        onPickUp.AddListener(Activate);
        onPutDown.AddListener(Release);

        restingPosition_ = transform.localPosition;
        //restingPosition_ = Vector3.zero;

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

    void LateUpdate()
    {
        var positions = new[]
        {
            top.position,
            transform.position,
            bot.position
        };

        line_.positionCount = 3;
        line_.SetPositions(positions);
    }

    void Activate()
    {
        Debug.Log("picked up handle!");
        transform.localPosition = restingPosition_;

        projectileInstance_ = Instantiate(projectile_);
        projectileInstance_.transform.parent = projectileRoot_;
        projectileInstance_.transform.position = transform.position;
    }

    void Release()
    {
        Destroy(projectileInstance_);
        projectileInstance_ = null;

        var draw = Vector3.Distance(transform.localPosition, restingPosition_);
        if (draw > minDrawDistance)
        {
            Shoot();
        }

        transform.localPosition = restingPosition_;
    }

    private void Shoot()
    {
        var holdRotation = Quaternion.FromToRotation(
            -projectileRoot_.forward,
            (transform.localPosition - restingPosition_).normalized);

        var projectile = Instantiate(
            projectile_,
            arrowSpawn.position,
            arrowSpawn.rotation,
            projectileRoot_);

        projectile.transform.Rotate(holdRotation.eulerAngles);

        var rb = projectile.GetComponent<Rigidbody>();
        var arrow = projectile.GetComponent<Arrow>();

        rb.AddForce(rb.transform.forward * 10000.0f);
        arrow.IsActive = true;
    }
}
