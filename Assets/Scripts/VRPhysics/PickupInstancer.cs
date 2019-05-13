using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInstancer : MonoBehaviour {
	void Start () {
        foreach (var gameObject in GameObject.FindGameObjectsWithTag(GameConstants.PickupTag))
        {
            gameObject.AddComponent<Pickupable>();
            gameObject.layer = GameConstants.PickupLayer;
        }
	}
}
