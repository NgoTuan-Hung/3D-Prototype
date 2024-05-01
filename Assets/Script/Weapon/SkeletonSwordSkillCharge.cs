using System.Collections;
using UnityEngine;

class SkeletonSwordSkillCharge : WeaponSubSkill
{
    SubSkillChangableAttribute chargeCooldown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    SubSkillChangableAttribute chargeSpeed = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 2f, SubSkillChangableAttribute.SubSkillAttributeType.Speed);

    public override void Start()
    {
        base.Start();
        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {ChargeCooldown1, ChargeSpeed, ChargeDistance});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };

        RecommendedAIBehavior.DistanceToTarget = ChargeDistance.FloatValue;
        SubSkillCondition.StopMoving = true;
    }
    SubSkillChangableAttribute chargeDistance = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 10f, SubSkillChangableAttribute.SubSkillAttributeType.Distance);
    float chargedDistance;
    float chargeDistanceOverTime;
    Vector3 chargeTemp;
    Vector3 chargeVelocity;

    public SubSkillChangableAttribute ChargeCooldown1 { get => chargeCooldown; set => chargeCooldown = value; }
    public SubSkillChangableAttribute ChargeSpeed { get => chargeSpeed; set => chargeSpeed = value; }
    public SubSkillChangableAttribute ChargeDistance 
    { 
        get  {return chargeDistance;}
        set
        {
            chargeDistance = value; 
            RecommendedAIBehavior.DistanceToTarget = value.FloatValue;
        } 
    }
    public float ChargedDistance { get => chargedDistance; set => chargedDistance = value; }
    public float ChargeDistanceOverTime { get => chargeDistanceOverTime; set => chargeDistanceOverTime = value; }
    public Vector3 ChargeTemp { get => chargeTemp; set => chargeTemp = value; }
    public Vector3 ChargeVelocity { get => chargeVelocity; set => chargeVelocity = value; }

    IEnumerator HandleCharge(Transform target)
    {
        chargeTemp = target.position - transform.position;
        chargeVelocity = chargeTemp.normalized * chargeSpeed.FloatValue;
        chargeDistanceOverTime = chargeSpeed.FloatValue;
        chargedDistance = 0;

        while(chargedDistance < chargeDistance.FloatValue)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            CustomMonoBehavior.Rigidbody.velocity = chargeVelocity;
            chargedDistance += chargeDistanceOverTime;
        }

        finishSkillDelegate?.Invoke();
        finishSkillDelegate = null;
        StartCoroutine(ChargeCooldown());
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(chargeCooldown.FloatValue);

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