using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameEffect : MonoBehaviour
{
    [SerializeField] new private ParticleSystem particleSystem;
    private ParticleSystemEvent particleSystemEvent;
    private bool particleSystemEventBool = false;
    private VisualEffect visualEffect;
    private bool visualEffectBool = false;
    private CollideAndDamage collideAndDamage;
    private bool collideAndDamageBool = false;
    [SerializeField] private List<Animator> animators = new List<Animator>();
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }
    internal ParticleSystemEvent ParticleSystemEvent { get => particleSystemEvent; set => particleSystemEvent = value; }
    public CollideAndDamage CollideAndDamage { get => collideAndDamage; set => collideAndDamage = value; }
    public bool CollideAndDamageBool { get => collideAndDamageBool; set => collideAndDamageBool = value; }
    public List<Animator> Animators { get => animators; set => animators = value; }
    public VisualEffect VisualEffect { get => visualEffect; set => visualEffect = value; }
    public bool VisualEffectBool { get => visualEffectBool; set => visualEffectBool = value; }

    void Awake()
    {
        if (!alreadySetOriginal)
        {
            alreadySetOriginal = true;
            var main = particleSystem.main;
            particleSystemStartSizeXMultiplierOriginal = main.startSizeXMultiplier;
            particleSystemStartSizeYMultiplierOriginal = main.startSizeYMultiplier;
            particleSystemStartSizeZMultiplierOriginal = main.startSizeZMultiplier;
            particleSystemStartLifetimeOriginal = main.startLifetime.constantMax;
        }

        if ((particleSystemEvent = GetComponentInChildren<ParticleSystemEvent>()) != null) particleSystemEventBool = true;
        if ((visualEffect = GetComponentInChildren<VisualEffect>()) != null) visualEffectBool = true;
        if ((collideAndDamage = GetComponentInChildren<CollideAndDamage>()) != null) collideAndDamageBool = true;
    }

    void OnEnable()
    {
        animators.ForEach(animators => 
        {
            animators.Play("Effect");
        });
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

    public IEnumerator FollowCoroutine(Transform target, Vector3 offset, bool transformPoint, bool keepTargetRotation)
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            if (transformPoint)
                transform.position = target.TransformPoint(offset);
            else
                transform.position = target.position + offset;

            if (keepTargetRotation)
                transform.rotation = target.rotation;
        }
    }
}