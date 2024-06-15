using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollideAndDamage : MonoBehaviour 
{
    [SerializeField] private float colliderDamage = 0f;
    [SerializeField] private float baseColliderDamage = 0f;
    [SerializeField] private float collisionDelayTime = 0.1f;
    [SerializeField] private List<string> collideExcludeTags = new List<string>();
    public enum ColliderType {Single, Multiple};
    [SerializeField] private ColliderType colliderType = ColliderType.Single;
    private BinarySearchTree<CollisionData> binarySearchTree = new BinarySearchTree<CollisionData>();
    public delegate void OnTriggerEnterDelegate(Collider other);
    public delegate void OnTriggerStayDelegate(Collider other);
    public delegate void OntriggerStayOnceDelegate();
    public OnTriggerEnterDelegate onTriggerEnterDelegate;
    public OnTriggerStayDelegate onTriggerStayDelegate;
    public OntriggerStayOnceDelegate onTriggerStayOnceDelegate;
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public List<string> CollideExcludeTags { get => collideExcludeTags; set => collideExcludeTags = value; }

    public void Awake()
    {
        if (colliderType == ColliderType.Single)
        {
            onTriggerEnterDelegate += (Collider other) => 
            {
                if (!collideExcludeTags.Contains(other.gameObject.tag))
                {
                    GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, other.gameObject);
                }
            };
        }
        else if (colliderType == ColliderType.Multiple)
        {
            onTriggerStayDelegate += (Collider other) => 
            {
                onTriggerStayOnceDelegate?.Invoke(); onTriggerStayOnceDelegate = null;
                if
                (
                    CheckCollidable(other.gameObject)
                    && !collideExcludeTags.Contains(other.gameObject.tag)
                )
                {
                    // need more work
                    GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, other.gameObject);
                }
            };
        }
    }

    public void OnEnable()
    {
        onTriggerStayOnceDelegate += () => 
        {
            StartCoroutine(CollisionTimer());
        };
    }


    public IEnumerator CollisionTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            binarySearchTree.Travel((CollisionData collisionData) => {collisionData.TimePassed -= Time.fixedDeltaTime;});
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {   
        onTriggerEnterDelegate?.Invoke(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        onTriggerStayDelegate?.Invoke(other);
    }

    public bool CheckCollidable(GameObject gameObject)
    {
        CollisionData collisionData = binarySearchTree.Search((CollisionData collisionData) => {return collisionData.CustomMonoBehavior.gameObject.GetInstanceID();}, gameObject.GetInstanceID());
        if (collisionData == null)
        {
            binarySearchTree.Insert
            (
                new CollisionData() {CustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(gameObject), TimePassed = collisionDelayTime}
            );

            return true;
        }
        else if (collisionData.TimePassed <= 0)
        {
            collisionData.TimePassed = collisionDelayTime;
            return true;
        }

        return false;
    }
}

class CollisionData : IComparable<CollisionData>
{
    private CustomMonoBehavior customMonoBehavior;
    private float timePassed;

    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public float TimePassed { get => timePassed; set => timePassed = value; }

    public int CompareTo(CollisionData other)
    {
        return customMonoBehavior.gameObject.GetInstanceID().CompareTo(other.customMonoBehavior.gameObject.GetInstanceID());
    }
}