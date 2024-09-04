using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[ExecuteInEditMode]
public class CollideAndDamage : MonoBehaviour 
{
    [SerializeField] private float colliderDamage = 0f;
    [SerializeField] private float baseColliderDamage = 0f;
    [SerializeField] private float collisionDelayTime = 0.1f;
    [SerializeField] private List<string> collideExcludeTags = new List<string>();
    public enum ColliderType {Single, Multiple};
    [SerializeField] private ColliderType colliderType = ColliderType.Single;
    private BinarySearchTree<CollisionData> binarySearchTree = new BinarySearchTree<CollisionData>();
    private List<BoxCollider> boxColliders = new List<BoxCollider>();
    [SerializeField] private List<DynamicCollisionData> dynamicCollisionDatas = new List<DynamicCollisionData>();
    [SerializeField] private bool isDynamic = false;
    public delegate void OnTriggerEnterDelegate(Collider other);
    public delegate void OnTriggerStayDelegate(Collider other);
    public delegate void OntriggerStayOnceDelegate();
    public OnTriggerEnterDelegate onTriggerEnterDelegate;
    public OnTriggerStayDelegate onTriggerStayDelegate;
    public OntriggerStayOnceDelegate onTriggerStayOnceDelegate;
    private ParticleSystemEvent particleSystemEvent;
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public List<string> CollideExcludeTags { get => collideExcludeTags; set => collideExcludeTags = value; }

    public void Awake()
    {
        boxColliders.AddRange(GetComponents<BoxCollider>());

        if (boxColliders.Count != 0) for (int i=0;i<boxColliders.Count;i++)
        {
            dynamicCollisionDatas[i].ColliderDefaultCenter = boxColliders[i].center;
            dynamicCollisionDatas[i].ColliderDefaultSize = boxColliders[i].size;
        }

        if (colliderType == ColliderType.Single)
        {
            onTriggerEnterDelegate += (Collider other) => 
            {
                if (!collideExcludeTags.Contains(other.gameObject.tag))
                {
                    Debug.Log("CollideAndDamage: " + other.gameObject.name);
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

        if (isDynamic) StartDynamicCollider();
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

    public void StartDynamicCollider()
    {
        for (int i=0;i<boxColliders.Count;i++)
        {
            StartCoroutine(StartDynamicCollider(i));
        }
    }

    public static bool test = false;
    public bool valid = false;
    public void Update()
    {
        if (test && valid)
        {
            test = false;
            valid = false;
            StartCoroutine(Valid());
            Test();
        }
    }

    public IEnumerator Valid()
    {
        yield return new WaitForSeconds(0.5f);
        valid = true;
    }

    public void Test()
    {
        StartDynamicCollider();
    }

    public IEnumerator StartDynamicCollider(int i)
    {
        yield return new WaitForSeconds(dynamicCollisionDatas[i].StartDelayTime);

        for (int j=0;j<dynamicCollisionDatas[i].Cycle;j++)
        {
            ResetDynamicCollider(i);
            StartCoroutine(DynamicColliderCycle(i));
            yield return new WaitForSeconds(dynamicCollisionDatas[i].CycleLifeTime + dynamicCollisionDatas[i].StartDelayTime);
        }

        yield return new WaitForSeconds(dynamicCollisionDatas[i].CycleLifeTime);
        ResetDynamicCollider(i);
    }

    public IEnumerator DynamicColliderCycle(int i)
    {
        float timePassed = 0f;
        float actualTime = 0f;
        while (true)
        {
            boxColliders[i].center = new Vector3
            (
                dynamicCollisionDatas[i].CenterXOverLifeTime.Evaluate(actualTime),
                dynamicCollisionDatas[i].CenterYOverLifeTime.Evaluate(actualTime),
                dynamicCollisionDatas[i].CenterZOverLifeTime.Evaluate(actualTime)
            );

            boxColliders[i].size = new Vector3
            (
                dynamicCollisionDatas[i].SizeXOverLifeTime.Evaluate(actualTime),
                dynamicCollisionDatas[i].SizeYOverLifeTime.Evaluate(actualTime),
                dynamicCollisionDatas[i].SizeZOverLifeTime.Evaluate(actualTime)
            );

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            timePassed += Time.fixedDeltaTime;
            actualTime = timePassed / dynamicCollisionDatas[i].CycleLifeTime;

            if (timePassed > dynamicCollisionDatas[i].CycleLifeTime) break;
        }
    }

    public void ResetDynamicCollider(int i)
    {
        boxColliders[i].center = dynamicCollisionDatas[i].ColliderDefaultCenter;
        boxColliders[i].size = dynamicCollisionDatas[i].ColliderDefaultSize;
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

[Serializable]
class DynamicCollisionData
{
    [SerializeField] private int cycle = 0;
    [SerializeField] private float cycleLifeTime = 0f;
    [SerializeField] private float startDelayTime = 0f;
    [SerializeField] private Vector3 colliderDefaultCenter = new Vector3();
    [SerializeField] private Vector3 colliderDefaultSize = new Vector3();
    [SerializeField] private AnimationCurve centerXOverLifeTime = new();
    [SerializeField] private AnimationCurve centerYOverLifeTime = new();
    [SerializeField] private AnimationCurve centerZOverLifeTime = new();
    [SerializeField] private AnimationCurve sizeXOverLifeTime = new ();
    [SerializeField] private AnimationCurve sizeYOverLifeTime = new ();
    [SerializeField] private AnimationCurve sizeZOverLifeTime = new ();

    public Vector3 ColliderDefaultCenter { get => colliderDefaultCenter; set => colliderDefaultCenter = value; }
    public Vector3 ColliderDefaultSize { get => colliderDefaultSize; set => colliderDefaultSize = value; }
    public AnimationCurve CenterXOverLifeTime { get => centerXOverLifeTime; set => centerXOverLifeTime = value; }
    public AnimationCurve CenterYOverLifeTime { get => centerYOverLifeTime; set => centerYOverLifeTime = value; }
    public AnimationCurve CenterZOverLifeTime { get => centerZOverLifeTime; set => centerZOverLifeTime = value; }
    public AnimationCurve SizeXOverLifeTime { get => sizeXOverLifeTime; set => sizeXOverLifeTime = value; }
    public AnimationCurve SizeYOverLifeTime { get => sizeYOverLifeTime; set => sizeYOverLifeTime = value; }
    public AnimationCurve SizeZOverLifeTime { get => sizeZOverLifeTime; set => sizeZOverLifeTime = value; }
    public int Cycle { get => cycle; set => cycle = value; }
    public float CycleLifeTime { get => cycleLifeTime; set => cycleLifeTime = value; }
    public float StartDelayTime { get => startDelayTime; set => startDelayTime = value; }
}