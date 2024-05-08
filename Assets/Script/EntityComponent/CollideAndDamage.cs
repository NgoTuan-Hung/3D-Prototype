using System.Collections.Generic;
using UnityEngine;

public class CollideAndDamage : MonoBehaviour 
{
    private float colliderDamage = 0f;
    private float baseColliderDamage = 0f;
    private List<string> collideExcludeTags = new List<string>();

    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public List<string> CollideExcludeTags { get => collideExcludeTags; set => collideExcludeTags = value; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!collideExcludeTags.Contains(other.gameObject.tag))
        {
            GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, other.gameObject);
        }
    }
}