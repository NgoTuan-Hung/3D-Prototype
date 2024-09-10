using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* most monobehavior class should derive from this */
public class ExtensibleMonobehavior : MonoBehaviour
{
    private bool freeze = false;
    private List<string> excludeTags;
    public bool Freeze1 { get => freeze; set => freeze = value; }
    public List<string> ExcludeTags { get => excludeTags; set => excludeTags = value; }

    public virtual void Freeze()
    {
        freeze = true;
    }

    public virtual void UnFreeze()
    {
        freeze = false;
    }
}
