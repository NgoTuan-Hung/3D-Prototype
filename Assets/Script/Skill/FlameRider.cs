
using System.Collections;
using UnityEngine;

public class FlameRider : SkillBase
{
    private static ObjectPool fireShieldEffectPool;
    public override void Awake()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAnimationTransition = true;
        // ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Awake();
        GameObject fireShieldEffectPrefab = Resources.Load("Effect/FlameRiderSkill/FireShield") as GameObject;
        fireShieldEffectPool ??= new ObjectPool(fireShieldEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        
        RecommendedAIBehavior.MoveForwardOnly = true;
        UseSkillChance = 25f;
    }

    [SerializeField] private SubSkillChangableAttribute duration = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Duration);

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
        fireShieldEffect.VisualEffect.Stop();
        //fireShieldEffect.

        CustomMonoBehavior.HumanLikeMovable.MoveSpeedPerFrame *= 2;
        CustomMonoBehavior.HumanLikeMovable.RunSpeedPerFrame *= 2;

        float timePassed = 0;
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            timePassed += Time.fixedDeltaTime;
            if (timePassed >= duration.FloatValue) break;
        }

        CustomMonoBehavior.HumanLikeMovable.SetSpeedDefault();
        CustomMonoBehavior.StopCustomonobehaviorState(CustomMonoBehavior.CustomMonoBehaviorState.IsUsingSkill);
    }
}