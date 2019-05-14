using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestructionMethod
{
    Deletion,
    Explosion
}

public class Destroyable : MonoBehaviour {
    public void Destroy(DestructionMethod method)
        => Destroy(this.gameObject);
}
