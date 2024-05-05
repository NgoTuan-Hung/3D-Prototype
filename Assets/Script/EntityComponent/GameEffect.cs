using System.Collections;
using UnityEngine;

public class GameEffect : MonoBehaviour
{
    new private ParticleSystem particleSystem;
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }
    
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        if (!alreadySetOriginal)
        {
            alreadySetOriginal = true;
            var main = particleSystem.main;
            particleSystemStartSizeXMultiplierOriginal = main.startSizeXMultiplier;
            particleSystemStartSizeYMultiplierOriginal = main.startSizeYMultiplier;
            particleSystemStartSizeZMultiplierOriginal = main.startSizeZMultiplier;
            particleSystemStartLifetimeOriginal = main.startLifetime.constantMax;
        }
    }

    void OnEnable()
    {

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