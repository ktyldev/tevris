using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private bool isPickedUp_ = false;

    public bool PickUp()
    {
        if (isPickedUp_) return false;
        isPickedUp_ = true;
        return true;
    }

    public bool PutDown()
        => isPickedUp_ = false;
}
