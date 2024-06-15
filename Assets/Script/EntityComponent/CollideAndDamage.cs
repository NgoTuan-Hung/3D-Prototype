using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollideAndDamage : MonoBehaviour 
{
    [SerializeField] private float colliderDamage = 0f;
    [SerializeField] private float baseColliderDamage = 0f;
    [SerializeField] private List<string> collideExcludeTags = new List<string>();
    public enum ColliderType {Single, Multiple};
    [SerializeField] private ColliderType colliderType;
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public List<string> CollideExcludeTags { get => collideExcludeTags; set => collideExcludeTags = value; }

    public void Awake()
    {

    }


    public void StartMultipleCollision()
    {

    }

    public IEnumerator CheckCollision()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
    }

    public virtual void OnTriggerEnter(Collider other)
    {   
        if (!collideExcludeTags.Contains(other.gameObject.tag))
        {
            GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, other.gameObject);
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        
    }
}