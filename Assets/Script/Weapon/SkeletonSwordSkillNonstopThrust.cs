
using System.Collections;
using UnityEngine;

class SkeletonSwordSkillNonstopThrust : WeaponSubSkill
{
    private static ObjectPool effectPool;
    public override void Awake() 
    {
        base.Awake();
        GameObject effectPrefab = Resources.Load("Effect/NonstopThrust") as GameObject;
        effectPool ??= new ObjectPool(effectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

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
            gameEffect.transform.position = CustomMonoBehavior.SkillCastOriginPoint.transform.position;

            gameEffect.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
            float angle = Quaternion.LookRotation(subSkillParameter.Target.transform.position - gameEffect.transform.position).eulerAngles.y;
            gameEffect.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            gameEffect.VisualEffect.Play();

            StartCoroutine(StopCoroutine());
        }
    }

    public IEnumerator StopCoroutine()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }
}