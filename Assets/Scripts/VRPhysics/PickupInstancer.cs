using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInstancer : MonoBehaviour {
	void Awake () {
        foreach (var gameObject in GameObject.FindGameObjectsWithTag(GameConstants.PickupTag))
        {
            gameObject.layer = GameConstants.PickupLayer;

            // if they already have a script, don't give them another
            if (gameObject.GetComponent<Pickupable>() != null)
                continue;

            gameObject.AddComponent<Pickupable>();
        }
	}
}
