using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
class SwordSkillThousandSword : SkillBase
{
    [SerializeField] private static ObjectPool freeObjectPool {get; set;}
    public SubSkillChangableAttribute CoolDown { get => coolDown; set => coolDown = value; }
    public SubSkillChangableAttribute CastRange { get => castRange; set => castRange = value; }
    public SubSkillChangableAttribute Timers { get => timers; set => timers = value; }
    public SubSkillChangableAttribute MidPointPosition { get => midPointPosition; set => midPointPosition = value; }
    public SubSkillChangableAttribute TimeScale { get => timeScale; set => timeScale = value; }

    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 3f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    [SerializeField] private SubSkillChangableAttribute castRange = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 20f, SubSkillChangableAttribute.SubSkillAttributeType.CastRange);
    public override void Awake()
    {
        base.Awake();
        GameObject freeObjectPrefab = Resources.Load("FreeObject") as GameObject;
        freeObjectPool ??= new ObjectPool(freeObjectPrefab, 20, new PoolArgument(typeof(Transform), PoolArgument.WhereComponent.Self));
        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {coolDown, castRange, timers, midPointPosition, timeScale});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };

        RecommendedAIBehavior.MaxDistanceToTarget = castRange.FloatValue;
    }
    
    public override void Start()
    {
        base.Start();
    }
    private Vector3 A;
    private Vector3 B;
    private Vector3 C;
    [SerializeField] private SubSkillChangableAttribute timers = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.FloatArray, new float[] {0f, 0.5f, 1f}, SubSkillChangableAttribute.SubSkillAttributeType.Timers);

    public void Trigger(InputAction.CallbackContext callbackContext)
    {
        if (CanUse)
        {
            // GameObject target = CustomMonoBehavior.PlayerScript.TargetableObject.TargetChecker.NearestTarget.SkillCastOriginPoint;

            // if (Vector3.Distance(CustomMonoBehavior.SkillCastOriginPoint.transform.position, target.transform.position) < castRange.FloatValue)
            // {
            //     CanUse = false;

            //     //CustomMonoBehavior.Animator.SetBool("HandUpCast", true);
            //     //CustomMonoBehavior.Animator.Play("UpperBody.HandUpCast", 1, 0);
            //     for (int i = 0; i < timers.FloatArray.Length; i++)
            //     {
            //         StartCoroutine(Flying(timers.FloatArray[i], target));
            //     }

            //     StartCoroutine(BeginCooldown());
            // }
        }
    }

    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        if (CanUse)
        {
            GameObject target = subSkillParameter.Target.gameObject;

            if (Vector3.Distance(CustomMonoBehavior.SkillCastOriginPoint.transform.position, target.transform.position) < castRange.FloatValue)
            {
                CanUse = false;

                //CustomMonoBehavior.Animator.SetBool("HandUpCast", true);
                //CustomMonoBehavior.Animator.Play("UpperBody.HandUpCast", 1, 0);
                for (int i = 0; i < timers.FloatArray.Length; i++)
                {
                    StartCoroutine(Flying(timers.FloatArray[i], target));
                }

                StartCoroutine(BeginCooldown());
            }
        }
    }

    public IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }

    // todo :
    [SerializeField] private SubSkillChangableAttribute midPointPosition = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Vector3, new Vector3(0, 4, 1), SubSkillChangableAttribute.SubSkillAttributeType.Position);
    [SerializeField] private SubSkillChangableAttribute timeScale = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 2f, SubSkillChangableAttribute.SubSkillAttributeType.TimeScale);
    public IEnumerator Flying(float timer, GameObject target)
    {
        yield return new WaitForSeconds(timer);
        SwordWeapon swordWeapon = WeaponPool.PickOne().Weapon as SwordWeapon;
        GameObject freeObject = freeObjectPool.PickOne().GameObject;
        Transform swordWeaponParent = swordWeapon.transform.parent;
        swordWeapon.FlyingTrail.enabled = true;
        swordWeapon.CollideAndDamage.ColliderDamage = 20f;
        swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
        swordWeapon.Animator.SetBool("ThousandSword", true);

        Vector3 targetVector; 
        float targetDistance = Vector3.Distance
        (
            CustomMonoBehavior.SkillCastOriginPoint.transform.position
            , target.transform.position
        );

        A = CustomMonoBehavior.SkillCastOriginPoint.transform.position;
        freeObject.transform.position = A;
        targetVector = target.transform.position - freeObject.transform.position; 
        freeObject.transform.rotation = Quaternion.LookRotation(targetVector);
        freeObject.transform.position = freeObject.transform.TransformPoint
        (
            midPointPosition.Vector3.x,
            midPointPosition.Vector3.y,
            midPointPosition.Vector3.z * targetDistance
        );
        freeObject.transform.RotateAround(target.transform.position, targetVector, Random.Range(-90, 90));
        B = freeObject.transform.position;
        C = target.transform.position;

        float t = 0, realTimeIncrement = Time.fixedDeltaTime * timeScale.FloatValue;
        Vector3 prevPosition;
        while (t < 1)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            prevPosition = swordWeaponParent.position;
            // move it from a to c using bezier curve with mid point b
            swordWeaponParent.position = Mathf.Pow(1 - t, 2) * A + 2 * (1 - t) * t * B + Mathf.Pow(t, 2) * C;
            swordWeaponParent.rotation = Quaternion.LookRotation(swordWeaponParent.position - prevPosition);
            t += realTimeIncrement;
        }

        swordWeapon.Animator.SetBool("ThousandSword", false);
        swordWeapon.FlyingTrail.enabled = false;
        swordWeaponParent.gameObject.SetActive(false);
        freeObject.SetActive(false);
    }
}