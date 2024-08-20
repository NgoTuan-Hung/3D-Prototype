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
    private BoxCollider boxCollider;
    [SerializeField] private bool isDynamic = false;
    [SerializeField] private bool isDynamicColliderTriggered = false;
    [SerializeField] private int cycle = 0;
    [SerializeField] private float cycleLifeTime = 0f;
    [SerializeField] private float startDelayTime = 0f;
    private Vector3 colliderDefaultCenter = new Vector3();
    private Vector3 colliderDefaultSize = new Vector3();
    [SerializeField] private AnimationCurve centerXOverLifeTime = new();
    [SerializeField] private AnimationCurve centerYOverLifeTime = new();
    [SerializeField] private AnimationCurve centerZOverLifeTime = new();
    [SerializeField] private AnimationCurve sizeXOverLifeTime = new ();
    [SerializeField] private AnimationCurve sizeYOverLifeTime = new ();
    [SerializeField] private AnimationCurve sizeZOverLifeTime = new ();
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
        boxCollider = GetComponent<BoxCollider>();
        colliderDefaultCenter = boxCollider.center;
        colliderDefaultSize = boxCollider.size;

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

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void ScriptsHasBeenReloaded()
    {
        SceneView.duringSceneGui += DuringSceneGui;
    }

    static void DuringSceneGui(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.isKey && e.keyCode == KeyCode.F)
        {
            print("You pressed F key!");
            triggerEvent = true;
        }
    }

    public static bool triggerEvent = false;
    void Update()
    {
        if (isDynamic && isDynamicColliderTriggered && triggerEvent)
        {
            triggerEvent = false;
            isDynamicColliderTriggered = false;
            StartCoroutine(StartDynamicCollider());
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

    public IEnumerator StartDynamicCollider()
    {
        yield return new WaitForSeconds(startDelayTime);
        ResetDynamicCollider();
        StartCoroutine(DynamicColliderCycle());

        for (int i=1;i<cycle;i++)
        {
            yield return new WaitForSeconds(cycleLifeTime + startDelayTime);
            ResetDynamicCollider();
            StartCoroutine(DynamicColliderCycle());
        }
    }

    public IEnumerator DynamicColliderCycle()
    {
        float timePassed = 0f;
        float actualTime = 0f;
        while (true)
        {
            boxCollider.center = new Vector3
            (
                centerXOverLifeTime.Evaluate(actualTime),
                centerYOverLifeTime.Evaluate(actualTime),
                centerZOverLifeTime.Evaluate(actualTime)
            );

            boxCollider.size = new Vector3
            (
                sizeXOverLifeTime.Evaluate(actualTime),
                sizeYOverLifeTime.Evaluate(actualTime),
                sizeZOverLifeTime.Evaluate(actualTime)
            );

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            timePassed += Time.fixedDeltaTime;
            actualTime = timePassed / cycleLifeTime;

            if (timePassed > cycleLifeTime) break;
        }
    }

    public void ResetDynamicCollider()
    {
        boxCollider.center = colliderDefaultCenter;
        boxCollider.size = colliderDefaultSize;
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