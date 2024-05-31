using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
class SwordSkillThousandSword : WeaponSubSkill
{
    [SerializeField] private static ObjectPool freeObjectPool {get; set;}
    public override void Awake()
    {
        base.Awake();
        GameObject freeObjectPrefab = Resources.Load("FreeObject") as GameObject;
        freeObjectPool ??= new ObjectPool(freeObjectPrefab, 20, new PoolArgument(typeof(Transform), PoolArgument.WhereComponent.Self));
    }
    
    public override void Start()
    {
        base.Start();
    }
    private Vector3 A;
    private Vector3 B;
    private Vector3 C;
    [SerializeField] private float[] timers = {0f, 1f, 2f};
    public void Trigger(InputAction.CallbackContext callbackContext)
    {
        List<PoolObject> weaponPoolObjects = WeaponPool.PickNWithoutActive(3);
        List<PoolObject> freePoolObjects = freeObjectPool.Pick(3);
        SwordWeapon swordWeapon;

        GameObject target = CustomMonoBehavior.PlayerScript.TargetableObject.TargetChecker.NearestTarget;
        //CustomMonoBehavior.Animator.SetBool("HandUpCast", true);
        //CustomMonoBehavior.Animator.Play("UpperBody.HandUpCast", 1, 0);
        for (int i = 0; i < timers.Length; i++)
        {
            swordWeapon = (SwordWeapon)weaponPoolObjects[i].Weapon;
            
            swordWeapon.transform.parent.gameObject.SetActive(true);
            Debug.Log("swordWeapon : " + swordWeapon.CollideAndDamage.ColliderDamage);            

            StartCoroutine(Flying(timers[i], swordWeapon, freePoolObjects[i].GameObject, target));
        }
    }

    // todo : set freeobject as child
    // turn off flying trail
    private Vector3 midPointPosition = new Vector3(0, 2, 1);
    public IEnumerator Flying(float timer, SwordWeapon swordWeapon, GameObject freeObject, GameObject target)
    {
        yield return new WaitForSeconds(timer);
        swordWeapon.gameObject.SetActive(true);
        swordWeapon.CollideAndDamage.ColliderDamage = 20f;
        swordWeapon.FlyingTrail.enabled = true;
        swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.SkillableObject.CustomMonoBehavior.AllyTags;
        swordWeapon.Animator.SetBool("ThousandSword", true);

        Transform swordWeaponParent = swordWeapon.transform.parent;
        Vector3 targetVector; 
        float targetDistance = Vector3.Distance
        (
            CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position
            , target.transform.position
        );

        A = CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position;
        freeObject.transform.position = CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position;
        targetVector = target.transform.position - freeObject.transform.position; 
        freeObject.transform.rotation = Quaternion.LookRotation
        (
            targetVector
        );
        freeObject.transform.position = freeObject.transform.TransformPoint
        (
            midPointPosition.x,
            midPointPosition.y,
            midPointPosition.z * targetDistance
        );
        freeObject.transform.RotateAround(target.transform.position, targetVector, Random.Range(0, 360));
        B = freeObject.transform.position;
        C = target.transform.position;

        float t = 0;
        while (t < 1)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            // move it from a to c using bezier curve with mid point b
            swordWeaponParent.position = Mathf.Pow(1 - t, 2) * A + 2 * (1 - t) * t * B + Mathf.Pow(t, 2) * C;
            t += Time.fixedDeltaTime;
        }

        swordWeapon.Animator.SetBool("ThousandSword", false);
        swordWeapon.FlyingTrail.enabled = false;
        swordWeaponParent.gameObject.SetActive(false);
        freeObject.SetActive(false);
    }
}