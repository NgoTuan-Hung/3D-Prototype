using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollideAndDamage : MonoBehaviour 
{
    [SerializeField] private float colliderDamage = 0f;
    [SerializeField] private float baseColliderDamage = 0f;
    [SerializeField] private List<string> collideExcludeTags = new List<string>();

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

    public void OnParticleCollision(GameObject gameObject)
    {
        Debug.Log("Particle Collision Detected!");
        if (!collideExcludeTags.Contains(gameObject.tag))
        {
            GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, gameObject);
        }
    }
}