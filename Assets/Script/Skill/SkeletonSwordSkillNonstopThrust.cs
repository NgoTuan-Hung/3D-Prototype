using System.Collections;
using UnityEngine;

class SkeletonSwordSkillNonstopThrust : SkillBase
{
    private static ObjectPool thrustingEffectPool;
    private static ObjectPool skeletonThrustingEffectPool;
    public override void Awake() 
    {
        AnimatorStateForSkill = State.CastSpellMiddle;
        UpperBodyCheckForAction = true;
        ExecutionTimeAfterAnimationFrame = 1.1f;
        base.Awake();
        GameObject effectPrefab = Resources.Load("Effect/NonstopThrust") as GameObject;
        thrustingEffectPool ??= new ObjectPool(effectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        GameObject skeletonThrustingEffectPrefab = Resources.Load("Effect/SkeletonThrustingEffect") as GameObject;
        skeletonThrustingEffectPool ??= new ObjectPool(skeletonThrustingEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {coolDown});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };
        
        RecommendedAIBehavior.MaxDistanceToTarget = 5f;
        RecommendedAIBehavior.IsLookingAtTarget = true;
        
        CustomMonoBehavior.HumanLikeAnimatorBrain.AddEventForClipOfState
        (
            "SkeletonSwordSkillNonstopThrustStopAnimationEvent", AnimatorStateForSkill, HumanLikeAnimatorBrain.AddEventForClipOfStateTimeType.End, 0
        );
    }

    public override void Start()
    {
        base.Start();
    }

    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    [SerializeField] private SubSkillChangableAttribute flySpeed = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Speed);
    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.ChangeState(AnimatorStateForSkill);
        StartCoroutine(ExecuteAfterAnimationFrame(subSkillParameter));
    }

    public void SkeletonSwordSkillNonstopThrustStopAnimationEvent()
    {
        CustomMonoBehavior.HumanLikeAnimatorBrain.StopState(AnimatorStateForSkill);
    }

    public IEnumerator ExecuteAfterAnimationFrame(SubSkillParameter subSkillParameter)
    {
        yield return new WaitForSeconds(ExecutionTimeAfterAnimationFrame);

        if (CanUse)
        {
            CanUse = false;
            
            GameEffect gameEffect = thrustingEffectPool.PickOne().GameEffect;
            gameEffect.VisualEffect.Stop();

            GameEffect skeletonThrustingEffect = skeletonThrustingEffectPool.PickOne().GameEffect;
            skeletonThrustingEffect.transform.position = transform.position;
            skeletonThrustingEffect.Animator.speed = 0;

            Vector3 travelToDirection = subSkillParameter.Target.transform.position - skeletonThrustingEffect.transform.position;
            float angle = Quaternion.LookRotation(travelToDirection).eulerAngles.y;
            skeletonThrustingEffect.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            
            skeletonThrustingEffect.TravelToDirection(travelToDirection, flySpeed.FloatValue);
            skeletonThrustingEffect.triggerActionDelegate += () => TriggerActionDelegateParam(skeletonThrustingEffect);
            skeletonThrustingEffect.triggerActionDelegate1 += () => TriggerActionDelegate1Param(gameEffect, skeletonThrustingEffect);
            skeletonThrustingEffect.animationEvent1Delegate += () => AnimationEvent1DelegateParam(gameEffect, skeletonThrustingEffect);
            skeletonThrustingEffect.animationEvent2Delegate += () => AnimationEvent2DelegateParam(skeletonThrustingEffect);
            skeletonThrustingEffect.TriggerActionWithCondition1(false, null, 0, true, 5f);
            skeletonThrustingEffect.TriggerActionWithCondition(true, subSkillParameter.Target.gameObject, 2f, false, 0);

            StartCoroutine(StopCoroutine());
        }
    }

    public void TriggerActionDelegateParam(GameEffect skeletonThrustingEffect)
    {
        skeletonThrustingEffect.Animator.speed = 1;
        skeletonThrustingEffect.TravelToDirectionBool = false;
        skeletonThrustingEffect.StopCoroutine(skeletonThrustingEffect.triggerActionWithConditionCoroutine1);
    }

    public void TriggerActionDelegate1Param(GameEffect gameEffect, GameEffect skeletonThrustingEffect)
    {
        skeletonThrustingEffect.gameObject.SetActive(false);
        gameEffect.gameObject.SetActive(false);
    }

    public void AnimationEvent1DelegateParam(GameEffect gameEffect, GameEffect skeletonThrustingEffect)
    {
        gameEffect.transform.position = skeletonThrustingEffect.transform.position + new Vector3(0, 1, 0);
        gameEffect.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
        gameEffect.transform.rotation = skeletonThrustingEffect.transform.rotation;
        gameEffect.VisualEffect.Play();
        gameEffect.triggerActionDelegate += () => 
        {
            gameEffect.gameObject.SetActive(false);
        };
        gameEffect.TriggerActionWithCondition(false, null, 0, true, 5f);
    }
    
    public void AnimationEvent2DelegateParam(GameEffect skeletonThrustingEffect)
    {
        skeletonThrustingEffect.gameObject.SetActive(false);
    }

    public IEnumerator StopCoroutine()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }
}