using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
public class CollideAndDamage : ExtensibleMonobehavior
{
    [SerializeField] private float colliderDamage = 0f;
    [SerializeField] private float baseColliderDamage = 0f;
    [SerializeField] private float collisionDelayTime = 0.1f;
    public enum ColliderType {Single, Multiple};
    [SerializeField] private ColliderType colliderType = ColliderType.Single;
    private BinarySearchTree<CollisionData> binarySearchTree = new BinarySearchTree<CollisionData>();
    private List<BoxCollider> boxColliders = new List<BoxCollider>();
    [SerializeField] private List<DynamicCollisionData> dynamicCollisionDatas = new List<DynamicCollisionData>();
    [SerializeField] private bool isDynamic = false;
    public enum InflictEffect {None, Freeze};
    [SerializeField] private InflictEffect inflictEffect = InflictEffect.None;
    [SerializeField] private float inflictEffectDuration = 0f;
    [SerializeField] private float inflictEffectChance = 0f;
    public delegate void InflictEffectDelegate(CustomMonoBehavior customMonoBehavior);
    public InflictEffectDelegate inflictEffectDelegate;
    public delegate void OnTriggerEnterDelegate(Collider other);
    public delegate void OnTriggerStayDelegate(Collider other);
    public delegate void OntriggerStayOnceDelegate();
    public OnTriggerEnterDelegate onTriggerEnterDelegate;
    public OnTriggerStayDelegate onTriggerStayDelegate;
    public OntriggerStayOnceDelegate onTriggerStayOnceDelegate;
    private ParticleSystemEvent particleSystemEvent;
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public float InflictEffectDuration { get => inflictEffectDuration; set => inflictEffectDuration = value; }
    public float InflictEffectChance { get => inflictEffectChance; set => inflictEffectChance = value; }
    public bool IsDynamic { get => isDynamic; set => isDynamic = value; }

    public void Awake()
    {
        ExcludeTags = new List<string>();
        boxColliders.AddRange(GetComponents<BoxCollider>());

        if (boxColliders.Count != 0) for (int i=0;i<boxColliders.Count;i++)
        {
            dynamicCollisionDatas[i].ColliderDefaultCenter = boxColliders[i].center;
            dynamicCollisionDatas[i].ColliderDefaultSize = boxColliders[i].size;
        }

        ChooseInflictEffect();
        if (colliderType == ColliderType.Single)
        {
            onTriggerEnterDelegate += (Collider other) => 
            {
                if (!ExcludeTags.Contains(other.gameObject.tag))
                {
                    CustomMonoBehavior collideWithCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(other.gameObject);
                    if (collideWithCustomMonoBehavior != null)
                    {
                        collideWithCustomMonoBehavior.UpdateHealth(colliderDamage);
                        inflictEffectDelegate?.Invoke(collideWithCustomMonoBehavior);
                    }
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
                    && !ExcludeTags.Contains(other.gameObject.tag)
                )
                {
                    // need more work
                    CustomMonoBehavior collideWithCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(other.gameObject);
                    if (collideWithCustomMonoBehavior != null)
                    {
                        collideWithCustomMonoBehavior.UpdateHealth(colliderDamage);
                        inflictEffectDelegate?.Invoke(collideWithCustomMonoBehavior);
                    }
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

        if (isDynamic) boxColliders.ForEach((boxCollider) => {boxCollider.enabled = false;});
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
            boxColliders[i].enabled = true;
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
            if (j == dynamicCollisionDatas[i].Cycle - 1) break;
            yield return new WaitForSeconds(dynamicCollisionDatas[i].CycleLifeTime + dynamicCollisionDatas[i].StartDelayTime);
        }

        yield return new WaitForSeconds(dynamicCollisionDatas[i].CycleLifeTime);
        ResetDynamicCollider(i);
        boxColliders[i].enabled = false;
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

    public void ChooseInflictEffect()
    {
        switch (inflictEffect)
        {
            case InflictEffect.Freeze:
                inflictEffectDelegate = (CustomMonoBehavior customMonoBehavior) => 
                {
                    if (UnityEngine.Random.Range(0f, 1f) <= inflictEffectChance)
                    {
                        customMonoBehavior.BuffAndNegativeEffect.ApplyEffect
                        (
                            BuffAndNegativeEffect.Effect.Freeze, BuffAndNegativeEffect.DurationType.Once, inflictEffectDuration
                        );
                    }
                };
                break;
            default: break;
        }
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