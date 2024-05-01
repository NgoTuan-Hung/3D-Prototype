using System.Collections;
using UnityEngine;

public class EntityAction : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;

    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public virtual void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    public virtual void Start()
    {
        
    }
}