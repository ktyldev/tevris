using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestructionMethod
{
    Deletion,
    Explosion
}

public enum DestructionType
{
    Self,
    Parent,
    Children
}

public class Destroyable : MonoBehaviour {
    public DestructionType DestructionType = DestructionType.Self;

    public void Destroy(DestructionMethod method)
    {
        SpawnFX(method);

        GameObject toDestroy = null;
        switch(DestructionType)
        {
        case DestructionType.Self:
            toDestroy = this.gameObject;
            break;
        case DestructionType.Parent:
            toDestroy = this.transform.parent.gameObject;
            break;
        default:
            toDestroy = this.gameObject;
            break;
        }

        GameObject.Destroy(toDestroy);
    }

    private void SpawnFX(DestructionMethod method) {
        switch(method)
        {
            case DestructionMethod.Explosion:
                break;

            case DestructionMethod.Deletion:
            default:
                break;
        }
    }
}
