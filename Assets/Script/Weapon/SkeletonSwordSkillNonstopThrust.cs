
using System.Collections;
using UnityEngine;

class SkeletonSwordSkillNonstopThrust : WeaponSubSkill
{
    private static ObjectPool effectPool;
    private static ObjectPool skeletonThrustingEffectPool;
    public override void Awake() 
    {
        base.Awake();
        GameObject effectPrefab = Resources.Load("Effect/NonstopThrust") as GameObject;
        effectPool ??= new ObjectPool(effectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        GameObject skeletonThrustingEffectPrefab = Resources.Load("Effect/SkeletonThrustingEffect") as GameObject;
        skeletonThrustingEffectPool ??= new ObjectPool(skeletonThrustingEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {coolDown});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };
        
        RecommendedAIBehavior.MaxDistanceToTarget = 5f;
    }

    public override void Start()
    {
        base.Start();
    }

    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        if (CanUse)
        {
            CanUse = false;
            
            GameEffect gameEffect = effectPool.PickOne().GameEffect;
            gameEffect.VisualEffect.Stop();

            GameEffect skeletonThrustingEffect = skeletonThrustingEffectPool.PickOne().GameEffect;
            skeletonThrustingEffect.transform.position = transform.position;
            skeletonThrustingEffect.Animator.speed = 0;

            Vector3 travelToDirection = subSkillParameter.Target.transform.position - skeletonThrustingEffect.transform.position;
            float angle = Quaternion.LookRotation(travelToDirection).eulerAngles.y;
            skeletonThrustingEffect.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            
            skeletonThrustingEffect.TravelToDirection(travelToDirection, 5);
            skeletonThrustingEffect.triggerActionDelegate += () => TriggerActionDelegateParam(skeletonThrustingEffect);
            skeletonThrustingEffect.triggerActionDelegate1 += () => TriggerActionDelegate1Param(skeletonThrustingEffect);
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

    public void TriggerActionDelegate1Param(GameEffect skeletonThrustingEffect)
    {
        skeletonThrustingEffect.gameObject.SetActive(false);
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