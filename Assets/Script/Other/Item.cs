using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {SkillBook}
    public ItemType itemType;
    private bool interactable = true;
    private string skillName;

    void Awake()
    {
        OnInteract = itemType switch
        {
            ItemType.SkillBook => SkillBookInteract,
            _ => null
        };
    }

    public delegate void InteractableDelegate(Collision collision);
    public event InteractableDelegate OnInteract;

    private CustomMonoBehavior interactedCustomMonoBehavior;
    public void SkillBookInteract(Collision collision)
    {
        interactedCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(collision.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
