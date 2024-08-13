// using UnityEngine;

// class UseSkillWheneverPossible : EntityAction
// {
//     private float useSkillChanceRequired = 50f;
//     private delegate void UseAnySkillDelegate();
//     private UseAnySkillDelegate useAnySkillDelegate;
//     public float UseSkillChanceRequired { get => useSkillChanceRequired; set => useSkillChanceRequired = value; }

//     public override void Awake()
//     {
//         base.Awake();
//     }

//     public override void Start()
//     {
//         base.Start();
//         if (CustomMonoBehavior.SkillableObjectBool && CustomMonoBehavior.MoveToTargetBool) useAnySkillDelegate += UseAnySkill;
//     }

//     void FixedUpdate()
//     {
//         useAnySkillDelegate?.Invoke();
//     }

//     void UseAnySkill()
//     {
//         CustomMonoBehavior.SkillableObject.WeaponSkills.ForEach(weaponSkill => 
//         {
//             weaponSkill.WeaponSubSkills.ForEach(weaponSubSkill =>
//             {
//                 if (weaponSubSkill.CanUse)
//                 {
//                     if (UseSkillChance(weaponSubSkill.RecommendedAIBehavior) >= UseSkillChanceRequired)
//                     {
//                         HandleSubSkillCondition(weaponSubSkill);
//                         weaponSubSkill.Trigger(GetSubSkillRequiredParameter(weaponSubSkill.SubSkillRequiredParameter));
//                     }
//                 }
//             });
//         });
//     }

//     float UseSkillChance(RecommendedAIBehavior recommendedAIBehavior)
//     {
//         float chance = 0f;

//         if (recommendedAIBehavior.MaxDistanceToTarget != 0)
//         {
//             if (CustomMonoBehavior.MoveToTarget.DistanceToTarget < recommendedAIBehavior.MaxDistanceToTarget)
//             {
//                 chance += Random.Range(20, 30);
//             }
//         }  
        
//         if (CustomMonoBehavior.MoveToTarget.DistanceToTarget > recommendedAIBehavior.MinDistanceToTarget)
//         {
//             chance += Random.Range(20, 30);
//         }

//         return chance;
//     }

//     SubSkillParameter GetSubSkillRequiredParameter(SubSkillRequiredParameter subSkillRequiredParameter)
//     {
//         SubSkillParameter subSkillParameter = new SubSkillParameter();

//         if (subSkillRequiredParameter.Target)
//         {
//             subSkillParameter.Target = CustomMonoBehavior.MoveToTarget.Target;
//         }

//         return subSkillParameter;
//     }

//     public void HandleSubSkillCondition(WeaponSubSkill weaponSubSkill)
//     {
//         if (weaponSubSkill.SubSkillCondition.StopMoving)
//         {
//             CustomMonoBehavior.MoveToTarget.CanMove = false;
//             weaponSubSkill.finishSkillDelegate += () => CustomMonoBehavior.MoveToTarget.CanMove = true;
//         }
//         if (weaponSubSkill.SubSkillCondition.StopRotating)
//         {
//             CustomMonoBehavior.MoveToTarget.CanRotate = false;
//             weaponSubSkill.finishSkillDelegate += () => CustomMonoBehavior.MoveToTarget.CanRotate = true;
//         }
//     }
// }