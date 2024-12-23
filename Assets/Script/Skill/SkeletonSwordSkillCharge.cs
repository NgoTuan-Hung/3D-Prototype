using System.Collections;
using UnityEngine;

class SkeletonSwordSkillCharge : SkillBase
{
    [SerializeField] SubSkillChangableAttribute chargeCooldown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    [SerializeField] SubSkillChangableAttribute chargeSpeed = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 40f, SubSkillChangableAttribute.SubSkillAttributeType.Speed);

    public override void Awake()
    {
        base.Awake();
        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {ChargeCooldown1, ChargeSpeed, ChargeDistance});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };

        RecommendedAIBehavior.MaxDistanceToTarget = ChargeDistance.FloatValue;
        RecommendedAIBehavior.MinDistanceToTarget = 7f;
        SubSkillCondition.StopMoving = true;
        SubSkillCondition.StopRotating = true;
    }

    public override void Start()
    {
        base.Start();
    }
    [SerializeField] SubSkillChangableAttribute chargeDistance = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 10f, SubSkillChangableAttribute.SubSkillAttributeType.Distance);
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
            RecommendedAIBehavior.MaxDistanceToTarget = value.FloatValue;
        } 
    }
    public float ChargedDistance { get => chargedDistance; set => chargedDistance = value; }
    public float ChargeDistanceOverTime { get => chargeDistanceOverTime; set => chargeDistanceOverTime = value; }
    public Vector3 ChargeTemp { get => chargeTemp; set => chargeTemp = value; }
    public Vector3 ChargeVelocity { get => chargeVelocity; set => chargeVelocity = value; }

    IEnumerator HandleCharge(Transform target)
    {
        chargeTemp = target.position - transform.position;
        chargeVelocity = new Vector3(chargeTemp.x, 0, chargeTemp.z).normalized * chargeSpeed.FloatValue * Time.fixedDeltaTime;
        chargeDistanceOverTime = chargeSpeed.FloatValue * Time.fixedDeltaTime;
        chargedDistance = 0;

        PoolObject poolObject = WeaponPool.PickOne();
        SkeletonSwordWeapon skeletonSwordWeapon = (SkeletonSwordWeapon)poolObject.Weapon;
        skeletonSwordWeapon.CollideAndDamage.ExcludeTags = CustomMonoBehavior.AllyTags;
        Transform skeletonSwordWeaponParent = skeletonSwordWeapon.transform.parent;

        skeletonSwordWeapon.CollideAndDamage.ColliderDamage = 20f;
        skeletonSwordWeaponParent.transform.position = transform.position;
        skeletonSwordWeaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, target.position - skeletonSwordWeaponParent.transform.position + new Vector3(0, 1f, 0));
        skeletonSwordWeapon.ChargeAttack();

        while(chargedDistance < chargeDistance.FloatValue)
        {
            CustomMonoBehavior.Rigidbody.position += chargeVelocity;
            skeletonSwordWeaponParent.transform.position = transform.position;
            chargedDistance += chargeDistanceOverTime;

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        
        skeletonSwordWeapon.StopChargeAttack();
        finishSkillDelegate?.Invoke();
        finishSkillDelegate = null;
        StartCoroutine(BeginCooldown());
    }

    IEnumerator BeginCooldown()
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