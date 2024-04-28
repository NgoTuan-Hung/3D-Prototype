using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

class SkeletonSwordSkillCharge : WeaponSubSkill
{
    SubSkillChangableAttribute chargeCooldown;
    SubSkillChangableAttribute chargeSpeed;

    void Start()
    {
        chargeCooldown = new SubSkillChangableAttribute(3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
        chargeSpeed = new SubSkillChangableAttribute(2f, SubSkillChangableAttribute.SubSkillAttributeType.Speed);
        chargeDistance = new SubSkillChangableAttribute(10f, SubSkillChangableAttribute.SubSkillAttributeType.Distance);
    }
    
    public void Charge(Transform target)
    {
        if (CanUse)
        {
            CanUse = false;

            StartCoroutine(HandleCharge(target));
        }
    }
    SubSkillChangableAttribute chargeDistance;
    float chargedDistance;
    float chargeDistanceOverTime;
    Vector3 chargeTemp;
    Vector3 chargeVelocity;

    IEnumerator HandleCharge(Transform target)
    {
        chargeTemp = target.position - transform.position;
        chargeVelocity = chargeTemp.normalized * (float)chargeSpeed.Value;
        chargeDistanceOverTime = (float)chargeSpeed.Value;
        chargedDistance = 0;

        while(chargedDistance < (float)chargeDistance.Value)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            //SkillableObject.CustomMonoBehavior.Rigidbody.velocity = chargeVelocity;
            chargedDistance += chargeDistanceOverTime;
        }

        StartCoroutine(ChargeCooldown());
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds((float)chargeCooldown.Value);

        CanUse = true;
    }
    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        
    }
}