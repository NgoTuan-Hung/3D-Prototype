using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameEffect : MonoBehaviour
{
    [SerializeField] new private ParticleSystem particleSystem;
    private bool particleSystemBool = false;
    private ParticleSystemEvent particleSystemEvent;
    private bool particleSystemEventBool = false;
    private VisualEffect visualEffect;
    private bool visualEffectBool = false;
    private CollideAndDamage collideAndDamage;
    private bool collideAndDamageBool = false;
    [SerializeField] private bool disableAfterTime = false;
    [SerializeField] private float disableTime = 2f;
    [SerializeField] private List<Animator> animators = new List<Animator>();
    private Animator animator;
    private bool animatorBool = false;
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }
    internal ParticleSystemEvent ParticleSystemEvent { get => particleSystemEvent; set => particleSystemEvent = value; }
    public CollideAndDamage CollideAndDamage { get => collideAndDamage; set => collideAndDamage = value; }
    public bool CollideAndDamageBool { get => collideAndDamageBool; set => collideAndDamageBool = value; }
    public List<Animator> Animators { get => animators; set => animators = value; }
    public VisualEffect VisualEffect { get => visualEffect; set => visualEffect = value; }
    public bool VisualEffectBool { get => visualEffectBool; set => visualEffectBool = value; }
    public bool DisableAfterTime { get => disableAfterTime; set => disableAfterTime = value; }
    public float DisableTime { get => disableTime; set => disableTime = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public bool TravelToDirectionBool { get => travelToDirectionBool; set => travelToDirectionBool = value; }

    void Awake()
    {
        if ((particleSystem = GetComponentInChildren<ParticleSystem>()) != null)
        {
            // particleSystemBool = true;
            // if (!alreadySetOriginal)
            // {
            //     alreadySetOriginal = true;
            //     var main = particleSystem.main;
            //     particleSystemStartSizeXMultiplierOriginal = main.startSizeXMultiplier;
            //     particleSystemStartSizeYMultiplierOriginal = main.startSizeYMultiplier;
            //     particleSystemStartSizeZMultiplierOriginal = main.startSizeZMultiplier;
            //     particleSystemStartLifetimeOriginal = main.startLifetime.constantMax;
            // }
        }

        if ((particleSystemEvent = GetComponentInChildren<ParticleSystemEvent>()) != null) particleSystemEventBool = true;
        if ((visualEffect = GetComponentInChildren<VisualEffect>()) != null) visualEffectBool = true;
        if ((collideAndDamage = GetComponentInChildren<CollideAndDamage>()) != null) collideAndDamageBool = true;
        if ((animator = GetComponent<Animator>()) != null) animatorBool = true;
    }

    void OnEnable()
    {
        animators.ForEach(animators => 
        {
            animators.Play("Effect");
        });

        if (disableAfterTime)
        {
            StartCoroutine(DisableAfterTimeCoroutine());
        }
    }

    public IEnumerator DisableAfterTimeCoroutine()
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }

    public Coroutine TriggerActionWithCondition(bool distanceToTarget, GameObject targetGameObject, float distance, bool timer, float time, Action action)
    {
        return StartCoroutine(TriggerActionWithConditionCoroutine(distanceToTarget, targetGameObject, distance, timer, time, action));
    }

    public IEnumerator TriggerActionWithConditionCoroutine(bool distanceToTarget, GameObject targetGameObject, float distance, bool timer, float time, Action action)
    {
        if (distanceToTarget)
        {
            while (true)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                if (Vector3.Distance(targetGameObject.transform.position, transform.position) <= distance)
                {
                    action?.Invoke();
                    break;
                }
            }
        }
        else if (timer)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
    }

    private bool travelToDirectionBool = false;
    public void TravelToDirection(Vector3 direction, float speed)
    {
        StartCoroutine(TravelToDirectionCoroutine(direction, speed));
    }

    public IEnumerator TravelToDirectionCoroutine(Vector3 direction, float speed)
    {
        travelToDirectionBool = true;
        Vector3 finalDirection = direction.normalized * speed;
        while (travelToDirectionBool)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            transform.position += finalDirection * Time.fixedDeltaTime;
        }
    }

    /* Use these in case we want to use the effect with animation */
    public delegate void AnimationEvent1Delegate();
    public AnimationEvent1Delegate animationEvent1Delegate;
    public void AnimationEvent1()
    {
        animationEvent1Delegate?.Invoke();
        animationEvent1Delegate = null;
    }

    public delegate void AnimationEvent2Delegate();
    public AnimationEvent2Delegate animationEvent2Delegate;
    public void AnimationEvent2()
    {
        animationEvent2Delegate?.Invoke();
        animationEvent2Delegate = null;
    }

    public delegate void AnimationEvent3Delegate();
    public AnimationEvent3Delegate animationEvent3Delegate;
    public void AnimationEvent3()
    {
        animationEvent3Delegate?.Invoke();
        animationEvent3Delegate = null;
    }

    public delegate void AnimationEvent4Delegate();
    public AnimationEvent4Delegate animationEvent4Delegate;
    public void AnimationEvent4()
    {
        animationEvent4Delegate?.Invoke();
        animationEvent4Delegate = null;
    }

    public delegate void OnDisableDelegate();
    public OnDisableDelegate onDisableDelegate;
    void OnDisable()
    {
        onDisableDelegate?.Invoke();
        onDisableDelegate = null;
    }

    private bool alreadySetOriginal = false;
    private float particleSystemStartSizeXMultiplierOriginal;
    private float particleSystemStartSizeYMultiplierOriginal;
    private float particleSystemStartSizeZMultiplierOriginal;
    private float particleSystemStartLifetimeOriginal;
    public void SetParticleSystemOriginalValue()
    {
        var main = particleSystem.main;
        main.startSizeXMultiplier = particleSystemStartSizeXMultiplierOriginal;
        main.startSizeYMultiplier = particleSystemStartSizeYMultiplierOriginal;
        main.startSizeZMultiplier = particleSystemStartSizeZMultiplierOriginal;
        main.startLifetime = particleSystemStartLifetimeOriginal;

        Debug.Log("SetParticleSystemOriginalValue");
    }

    public void Follow(Transform target, Vector3 offset, bool transformPoint, bool keepTargetRotation)
    {
        StartCoroutine(FollowCoroutine(target, offset, transformPoint, keepTargetRotation));
    }

    private delegate void FollowDelegate();
    private FollowDelegate followDelegate;
    public IEnumerator FollowCoroutine(Transform target, Vector3 offset, bool transformPoint, bool keepTargetRotation)
    {
        followDelegate = null;
        if (transformPoint) followDelegate += () => transform.position = target.TransformPoint(offset);
        else followDelegate += () => transform.position = target.position + offset;

        if (keepTargetRotation) followDelegate += () => transform.rotation = target.rotation;

        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            followDelegate?.Invoke();
        }
    }
}