using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRain : SkillBase
{
    private static ObjectPool lightingRainEffectPool;

    public override void Awake()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAnimationTransition = true;
        ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Awake();
        GameObject effectPrefab = Resources.Load("Effect/LightingRain") as GameObject;
        lightingRainEffectPool ??= new ObjectPool(effectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };

        RecommendedAIBehavior.IsLookingAtTarget = true;
        UseSkillChance = 25f;
    }

    public override void Start()
    {
        base.Start();

        CustomMonoBehavior.HumanLikeAnimatorBrain.AddEventForClipOfState
        (
            "LightingRainStopAnimationEvent", AnimatorStateForSkill, HumanLikeAnimatorBrain.AddEventForClipOfStateTimeType.End, 0
        );
    }

    public void LightingRainStopAnimationEvent()
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.StopState(AnimatorStateForSkill);
    }

    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    [SerializeField] private SubSkillChangableAttribute position = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Vector3, new Vector3(0, 5, 0), SubSkillChangableAttribute.SubSkillAttributeType.Position);
    [SerializeField] private SubSkillChangableAttribute particleSystemTimeEnd = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 6f, SubSkillChangableAttribute.SubSkillAttributeType.ParticleSystemTimeEnd);

    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.ChangeState(AnimatorStateForSkill);
        StartCoroutine(ExecuteAfterAnimationFrame(subSkillParameter));
    }

    public IEnumerator ExecuteAfterAnimationFrame(SubSkillParameter subSkillParameter)
    {
        float totalTime = 0;
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            totalTime += Time.fixedDeltaTime;
            if (totalTime >= ExecutionTimeAfterAnimationFrame) break;
        }

        if (CanUse)
        {
            CanUse = false;

            GameEffect lightingRainEffect = lightingRainEffectPool.PickOne().GameEffect;
            lightingRainEffect.ParticleSystem.Stop();

            lightingRainEffect.transform.position = subSkillParameter.Target.transform.position + position.Vector3;
            lightingRainEffect.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
            lightingRainEffect.PlayParticleSystem();

            yield return new WaitForSeconds(particleSystemTimeEnd.FloatValue);
            lightingRainEffect.gameObject.SetActive(false);
        }

        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }
}
