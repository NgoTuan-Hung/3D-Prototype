using System.Collections;
using UnityEngine;

class SkeletonSwordSkillCharge : WeaponSubSkill
{
    SubSkillChangableAttribute<float> chargeCooldown = new SubSkillChangableAttribute<float>(3f, SubSkillChangableAttribute<float>.SubSkillAttributeType.Cooldown);
    SubSkillChangableAttribute<float> chargeSpeed = new SubSkillChangableAttribute<float>(2f, SubSkillChangableAttribute<float>.SubSkillAttributeType.Speed);

    public override void Start()
    {
        base.Start();
        SubSkillChangableAttributes.AddRange(new ISubSkillChangableAttribute[] {ChargeCooldown1, ChargeSpeed, ChargeDistance});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };
    }
    SubSkillChangableAttribute<float> chargeDistance = new SubSkillChangableAttribute<float>(10f, SubSkillChangableAttribute<float>.SubSkillAttributeType.Distance);
    float chargedDistance;
    float chargeDistanceOverTime;
    Vector3 chargeTemp;
    Vector3 chargeVelocity;

    public SubSkillChangableAttribute<float> ChargeCooldown1 { get => chargeCooldown; set => chargeCooldown = value; }
    public SubSkillChangableAttribute<float> ChargeSpeed { get => chargeSpeed; set => chargeSpeed = value; }
    public SubSkillChangableAttribute<float> ChargeDistance 
    { 
        get  {return chargeDistance;}
        set
        {
            chargeDistance = value; 
            RecommendedAIBehavior.DistanceToTarget = value.Value;
        } 
    }
    public float ChargedDistance { get => chargedDistance; set => chargedDistance = value; }
    public float ChargeDistanceOverTime { get => chargeDistanceOverTime; set => chargeDistanceOverTime = value; }
    public Vector3 ChargeTemp { get => chargeTemp; set => chargeTemp = value; }
    public Vector3 ChargeVelocity { get => chargeVelocity; set => chargeVelocity = value; }

    IEnumerator HandleCharge(Transform target)
    {
        chargeTemp = target.position - transform.position;
        chargeVelocity = chargeTemp.normalized * chargeSpeed.Value;
        chargeDistanceOverTime = chargeSpeed.Value;
        chargedDistance = 0;

        while(chargedDistance < chargeDistance.Value)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            CustomMonoBehavior.Rigidbody.velocity = chargeVelocity;
            chargedDistance += chargeDistanceOverTime;
        }

        StartCoroutine(ChargeCooldown());
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown.Value);

        CanUse = true;
    }
    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        if (CanUse)
        {
            CanUse = false;

            StartCoroutine(HandleCharge(subSkillParameter.Target));
        }
    }
}