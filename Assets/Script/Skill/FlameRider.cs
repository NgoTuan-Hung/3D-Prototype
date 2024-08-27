
using System.Collections;
using UnityEngine;

public class FlameRider : SkillBase
{
    private static ObjectPool fireShieldEffectPool;
    private static ObjectPool objectBurningEffectPool;
    public override void Awake()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAnimationTransition = true;
        ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Awake();
        GameObject fireShieldEffectPrefab = Resources.Load("Effect/FlameRiderSkill/FireShield") as GameObject;
        fireShieldEffectPool ??= new ObjectPool(fireShieldEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject objectBurningEffectPrefab = Resources.Load("Effect/FlameRiderSkill/ObjectBurning") as GameObject;
        objectBurningEffectPool ??= new ObjectPool(objectBurningEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        
        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = false
        };

        RecommendedAIBehavior.MoveForwardOnly = true;
        UseSkillChance = 25f;
    }

    [SerializeField] private SubSkillChangableAttribute duration = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Duration);
    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);

    public override void Start()
    {
        base.Start();

        CustomMonoBehavior.HumanLikeAnimatorBrain.AddEventForClipOfState
        (
            "FlameRiderStopAnimationEvent", AnimatorStateForSkill, HumanLikeAnimatorBrain.AddEventForClipOfStateTimeType.End, 0
        );
    }

    public void FlameRiderStopAnimationEvent()
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.StopState(AnimatorStateForSkill);
    }

    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.ChangeState(AnimatorStateForSkill);
        CustomMonoBehavior.ChangeCustomonobehaviorState(CustomMonoBehavior.CustomMonoBehaviorState.IsUsingSkill);
        StartCoroutine(ExecuteAfterAnimationFrame(subSkillParameter));
    }

    public IEnumerator ExecuteAfterAnimationFrame(SubSkillParameter subSkillParameter)
    {
        yield return new WaitForSeconds(ExecutionTimeAfterAnimationFrame);

        CanUse = false;
        GameEffect fireShieldEffect = fireShieldEffectPool.PickOne().GameEffect;
        fireShieldEffect.Follow(transform, new Vector3(0, 0.75f, -4.15f), true, true);
        fireShieldEffect.VisualEffect.Play();
        GameEffect objectBurningEffect = objectBurningEffectPool.PickOne().GameEffect;
        objectBurningEffect.VisualEffect.SetSkinnedMeshRenderer("SkinnedMeshRenderer", CustomMonoBehavior.MainSkinnedMeshRenderer);
        objectBurningEffect.VisualEffect.Play();
        
        CustomMonoBehavior.HumanLikeMovable.MoveSpeedPerFrame *= 1.5f;
        CustomMonoBehavior.HumanLikeMovable.RunSpeedPerFrame *= 1.5f;
        StartCoroutine(CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.CustomModeWithCondition
        (
            false, null, true, duration.FloatValue, 
            () => 
            {
                if (Random.value < 0.5f) CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.ChangeMode(BotHumanLikeSimpleMoveToTarget.MoveMode.MoveToTargetMindlessly);
                else CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.ChangeMode(BotHumanLikeSimpleMoveToTarget.MoveMode.MoveAroundTarget);
            }, 1f
        ));

        yield return new WaitForSeconds(duration.FloatValue);

        CustomMonoBehavior.HumanLikeMovable.SetSpeedDefault();
        CustomMonoBehavior.StopCustomonobehaviorState(CustomMonoBehavior.CustomMonoBehaviorState.IsUsingSkill);
        fireShieldEffect.VisualEffect.Stop();
        fireShieldEffect.gameObject.SetActive(false);
        StartCoroutine(StartCooldown());

        yield return new WaitForSeconds(2f);
        objectBurningEffect.gameObject.SetActive(false);
    }

    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }
}