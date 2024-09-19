using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenSlash : SkillBase
{
    // Start is called before the first frame update
    private static ObjectPool iceReaperEffectPool;
    private static ObjectPool iceReaperShowupEffectPool;
    private static ObjectPool scytheSlashEffectPool;
    private static ObjectPool scytheSlashEffectParticlePool;
    private static ObjectPool scytheSlashImpactEffectPool;
    public override void Start()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAnimationTransition = true;
        ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Start();
        GameObject iceReaperEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/IceReaper") as GameObject;
        iceReaperEffectPool ??= new ObjectPool(iceReaperEffectPrefab, 10, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject iceReaperShowupEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/IceReaperShowup") as GameObject;
        iceReaperShowupEffectPool ??= new ObjectPool(iceReaperShowupEffectPrefab, 10, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject scytheSlashEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/ScytheSlash") as GameObject;
        scytheSlashEffectPool ??= new ObjectPool(scytheSlashEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject scytheSlashEffectParticlePrefab = Resources.Load("Effect/FrozenSlashSkill/ScytheSlashParticle") as GameObject;
        scytheSlashEffectParticlePool ??= new ObjectPool(scytheSlashEffectParticlePrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject scytheSlashImpactEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/ScytheSlashImpact") as GameObject;
        scytheSlashImpactEffectPool ??= new ObjectPool(scytheSlashImpactEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
    }

    public override void Awake() 
    {
        base.Awake();
    }

    public bool valid = false;
    
    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.K) && valid)
        {
            Trigger();
            valid = false;
            StartCoroutine(Valid());
        }
    }

    public void Trigger()
    {
        GameEffect iceReaperEffect = iceReaperEffectPool.PickOne().GameEffect;
        iceReaperEffect.transform.position = transform.position;
        iceReaperEffect.TargetChecker.ExcludeTags = new List<string> {"Team1"};
        iceReaperEffect.Animator.Play("ReaperAppear", 0, 0);
        StartCoroutine(ShowUpCoroutine(iceReaperEffect));
    }

    public void EffectEvent(GameEffect scytheSlashEffect, GameEffect scytheSlashParticleEffect, GameEffect iceReaperEffect, Vector3 transformDirection, Vector3 rotationEuler, float inflictEffectDuration, float inflictEffectChance)
    {
        scytheSlashEffect.transform.position = iceReaperEffect.transform.position + iceReaperEffect.transform.TransformDirection(transformDirection);
        scytheSlashParticleEffect.transform.position = scytheSlashEffect.transform.position;
        scytheSlashEffect.transform.rotation = Quaternion.Euler(iceReaperEffect.transform.rotation.eulerAngles + rotationEuler);
        scytheSlashParticleEffect.transform.rotation = scytheSlashEffect.transform.rotation;
        scytheSlashEffect.transform.parent = iceReaperEffect.transform;
        scytheSlashEffect.CollideAndDamage.InflictEffectDuration = inflictEffectDuration;
        scytheSlashEffect.CollideAndDamage.InflictEffectChance = inflictEffectChance;
        scytheSlashEffect.PlayVFX();
        scytheSlashParticleEffect.PlayVFX();
    }

    public void EffectEvent1(List<GameEffect> scytheSLashEffects, List<GameEffect> scytheSlashParticleEffects, List<GameEffect> scytheSLashImpactEffects, GameEffect iceReaperEffect, GameEffect reaperShowupEffect)
    {
        scytheSLashEffects.ForEach(scytheSlashEffect => {scytheSlashEffect.gameObject.SetActive(false); scytheSlashEffect.transform.parent = null;});
        scytheSlashParticleEffects.ForEach(scytheSlashParticleEffect => {scytheSlashParticleEffect.gameObject.SetActive(false);});
        scytheSLashImpactEffects.ForEach(scytheSLashImpactEffect => {scytheSLashImpactEffect.gameObject.SetActive(false);});
        iceReaperEffect.gameObject.SetActive(false);
        reaperShowupEffect.gameObject.SetActive(false);
    }

    public void EffectEvent2(GameEffect iceReaperEffect, float rotateAndMoveValue)
    {
        StartCoroutine(iceReaperEffect.RotateAndMoveTowardTarget(iceReaperEffect.TargetChecker.NearestTarget.transform.position, rotateAndMoveValue));
    }

    public void EffectEvent3(GameEffect scytheSlashEffect, List<GameEffect> scytheSlashImpactEffects)
    {
        scytheSlashEffect.CollideAndDamage.spawnImpactEffectDelegate = (Collider collider) => 
        {
            GameEffect scytheSlashImpactEffect = scytheSlashImpactEffectPool.PickOne().GameEffect;
            scytheSlashImpactEffects.Add(scytheSlashImpactEffect);
            scytheSlashImpactEffect.transform.position = collider.ClosestPoint(scytheSlashEffect.transform.position);
            scytheSlashImpactEffect.PlayVFX();
        };
    }

    public IEnumerator Valid()
    {
        yield return new WaitForSeconds(0.5f);
        valid = true;
    }

    [SerializeField] private float reaperShowUpTime = 0.5f;
    [SerializeField] private float reaperMaxAlpha = 0.5f;
    [SerializeField] private Vector3 showUpEffectOffset = new Vector3(0, 1f, 0);
    public IEnumerator ShowUpCoroutine(GameEffect iceReaperEffect)
    {
        float time = 0, alpha = 0, progressCalculation = reaperMaxAlpha / reaperShowUpTime;

        GameEffect iceReaperShowupEffect = iceReaperShowupEffectPool.PickOne().GameEffect;
        iceReaperShowupEffect.transform.position = transform.position + showUpEffectOffset;
        iceReaperShowupEffect.PlayVFX();

        while (!(time > reaperShowUpTime))
        {
            alpha = time * progressCalculation;
            iceReaperEffect.ShowUpMaterials.ForEach(material => 
            {
                material.SetFloat("_Alpha", alpha);
            });

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time += Time.fixedDeltaTime;
        }

        iceReaperEffect.Animator.Play("ReaperAttack", 0, 0);

        List<PoolObject> scytheSlashEffectPoolObjects = scytheSlashEffectPool.Pick(3),
        scytheSlashEffectParticlePoolObjects = scytheSlashEffectParticlePool.Pick(3);

        List<GameEffect> scytheSlashEffects = new List<GameEffect>(3),
        scytheSlashEffectParticles = new List<GameEffect>(3),
        scytheSlashImpactEffects = new();

        for (int i=0;i<3;i++)
        {
            scytheSlashEffects.Add(scytheSlashEffectPoolObjects[i].GameEffect);
            scytheSlashEffectParticles.Add(scytheSlashEffectParticlePoolObjects[i].GameEffect);
        }

        scytheSlashEffects[0].transform.position = scytheSlashEffects[1].transform.position = scytheSlashEffects[2].transform.position = new Vector3(999, 999, 999);
        scytheSlashEffects[0].VisualEffect.Stop();
        scytheSlashEffects[1].VisualEffect.Stop();
        scytheSlashEffects[2].VisualEffect.Stop();
        scytheSlashEffectParticles[0].transform.position = scytheSlashEffectParticles[1].transform.position = scytheSlashEffectParticles[2].transform.position = new Vector3(999, 999, 999);
        scytheSlashEffectParticles[0].VisualEffect.Stop();
        scytheSlashEffectParticles[1].VisualEffect.Stop();
        scytheSlashEffectParticles[2].VisualEffect.Stop();

        EffectEvent3(scytheSlashEffects[0], scytheSlashImpactEffects);
        EffectEvent3(scytheSlashEffects[1], scytheSlashImpactEffects);
        EffectEvent3(scytheSlashEffects[2], scytheSlashImpactEffects);
        iceReaperEffect.animationEvent1Delegate = () => EffectEvent(scytheSlashEffects[0], scytheSlashEffectParticles[0], iceReaperEffect, new Vector3(-0.88f, 1.625f, 1.08f), new Vector3(50, 31, -108), 1.5f, 0.3f);
        iceReaperEffect.animationEvent2Delegate = () => EffectEvent(scytheSlashEffects[1], scytheSlashEffectParticles[1], iceReaperEffect, new Vector3(1.59f, 0.09f, 0.35f), new Vector3(352.85f, 349.6f, 57), 1.5f, 0.3f);        
        iceReaperEffect.animationEvent3Delegate = () => EffectEvent(scytheSlashEffects[2], scytheSlashEffectParticles[2], iceReaperEffect, new Vector3(-0.46f, 0.68f, -0.66f), new Vector3(12.57f, 312.8f, 291.17f), 3f, 0.7f);

        iceReaperEffect.animationEvent4Delegate = () => EffectEvent1(scytheSlashEffects, scytheSlashEffectParticles, scytheSlashImpactEffects, iceReaperEffect, iceReaperShowupEffect);

        iceReaperEffect.animationEvent5Delegate = () => EffectEvent2(iceReaperEffect, 0.283f);
        iceReaperEffect.animationEvent6Delegate = () => EffectEvent2(iceReaperEffect, 0.233f);
        iceReaperEffect.animationEvent7Delegate = () => EffectEvent2(iceReaperEffect, 0.316f);
    }
}
