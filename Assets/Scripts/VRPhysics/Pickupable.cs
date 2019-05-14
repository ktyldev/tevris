using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickupable : MonoBehaviour
{
    private bool isPickedUp_ = false;
    protected UnityEvent onPickUp;
    protected UnityEvent onPutDown;

    public bool IsPickedUp {
        get { return isPickedUp_; }
    }

    protected void Awake()
    {
        onPickUp    = new UnityEvent();
        onPutDown   = new UnityEvent();
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

        onPutDown.Invoke();
    }
}
