using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickupable : MonoBehaviour
{
    private bool isPickedUp_ = false;
    private bool isActivated_ = false;

    protected UnityEvent onPickUp;
    protected UnityEvent onPutDown;

    protected UnityEvent onActivate;
    protected UnityEvent onDeactivate;

    public bool IsPickedUp {
        get { return isPickedUp_; }
    }

    public bool IsActivated
    {
        get { return isActivated_; }
    }

    protected void Awake()
    {
        onPickUp        = new UnityEvent();
        onPutDown       = new UnityEvent();
        onActivate      = new UnityEvent();
        onDeactivate    = new UnityEvent();
    }

    public bool PickUp()
    {
        if (isPickedUp_)
            return false;

        isPickedUp_ = true;

        onPickUp.Invoke();
        return true;
    }

    public void PutDown()
    {
        isPickedUp_ = false;

        if (isActivated_)
            Deactivate();

        onPutDown.Invoke();
    }

    public void Activate()
    {
        if (!IsPickedUp || isActivated_) return;

        isActivated_ = true;
        onActivate.Invoke();
    }

    public void Deactivate()
    {
        if (!isActivated_) return;

        isActivated_ = false;
        onDeactivate.Invoke();
    }
}
