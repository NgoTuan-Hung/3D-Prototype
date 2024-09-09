using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* most monobehavior class should derive from this */
public class ExtensibleMonobehavior : MonoBehaviour
{
    private bool freeze = false;

    public bool Freeze1 { get => freeze; set => freeze = value; }

    public virtual void Freeze()
    {
        freeze = true;
    }

    public virtual void UnFreeze()
    {
        freeze = false;
    }
}
