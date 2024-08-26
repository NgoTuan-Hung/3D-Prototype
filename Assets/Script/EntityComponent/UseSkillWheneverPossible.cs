using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(CanUseSkill))]
class UseSkillWheneverPossible : EntityAction
{
    private float useSkillChanceRequired = 50f;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        UseAnySkill();
    }

    void UseAnySkill()
    {        
        CustomMonoBehavior.CanUseSkill.Skills.ForEach(skill => 
        {
            if (skill.CheckCanUseSkill())
            {
                if (UseSkillChance(skill.RecommendedAIBehavior) >= skill.UseSkillChance)
                {
                    HandleSubSkillCondition(skill);
                    skill.Trigger(GetSubSkillRequiredParameter(skill.SubSkillRequiredParameter));
                }
            }
        });
    }

    float UseSkillChance(RecommendedAIBehavior recommendedAIBehavior)
    {
        float chance = 10f;

        if (recommendedAIBehavior.MaxDistanceToTarget != 0)
        {
            if (CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.DistanceToTarget < recommendedAIBehavior.MaxDistanceToTarget)
            {
                chance += Random.Range(20, 30);
            }
        }  
        
        if (CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.DistanceToTarget > recommendedAIBehavior.MinDistanceToTarget)
        {
            chance += Random.Range(20, 30);
        }

        if (recommendedAIBehavior.IsLookingAtTarget)
        {
            if (CustomMonoBehavior.BotHumanLikeLookAtTarget.IsLookingAtTarget)
            {
                chance += Random.Range(20, 30);
            }
        }

        return chance;
    }

    SubSkillParameter GetSubSkillRequiredParameter(SubSkillRequiredParameter subSkillRequiredParameter)
    {
        SubSkillParameter subSkillParameter = new SubSkillParameter();

        if (subSkillRequiredParameter.Target)
        {
            subSkillParameter.Target = CustomMonoBehavior.Target.transform;
        }

        return subSkillParameter;
    }

    public void HandleSubSkillCondition(SkillBase skill)
    {
        if (skill.SubSkillCondition.StopMoving)
        {
            CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.CanMove = false;
            skill.finishSkillDelegate += () => CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.CanMove = true;
        }
        if (skill.SubSkillCondition.StopRotating)
        {
            // CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.CanRotate = false;
            // skill.finishSkillDelegate += () => CustomMonoBehavior.BotHumanLikeSimpleMoveToTarget.CanRotate = true;
        }
    }
}