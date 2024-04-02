using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject swordPrefab;
    private Transform swordWeaponParent;
    SkillableObject skillableObject;
    private void Start() 
    {
        swordPrefab = Instantiate(Resources.Load("LongSword")).GameObject();
        swordPrefab.SetActive(false);
        weaponPool ??= new ObjectPool<Weapon>(swordPrefab, 20);

        skillableObject = GetComponent<SkillableObject>();
        skillCast = Instantiate(Resources.Load("BigSwordSkillCast")).GameObject();
        skillCast.SetActive(false);
    }


    public override void Attack(Transform target, Vector3 rotationDirection)
    {
        if (CanAttack)
        {
            CanAttack = false;
            ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
            SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
            swordWeaponParent = swordWeapon.transform.parent;

            swordWeapon.PlayAttackParticleSystem();
            swordWeapon.ColliderDamage = 10f;
            swordWeaponParent.position = target.position;
            swordWeaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, rotationDirection);
            swordWeaponParent.position = swordWeaponParent.transform.TransformPoint(0, 0, -2);
            swordWeapon.Attack();
            StartCoroutine(ResetAttack());
        }
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }

    public bool isWaiting = false;
    public Coroutine summonCoroutine;
    public Vector2 skillCastVector;
    public GameObject skillCast;
    public float skillCastAngle;
    public void SummonBigSword(InputAction.CallbackContext callbackContext)
    {
        if (!isWaiting)
        {
            StartCoroutine(HandleSummonSword());
        } else isWaiting = false;
    }

    public IEnumerator HandleSummonSword()
    {
        isWaiting = true;
        while (isWaiting)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            skillCastVector = (skillableObject.playerScript.playerInputSystem.Control.View.ReadValue<Vector2>() - (Vector2)Camera.main.WorldToScreenPoint(skillableObject.skillCastOriginPoint.transform.position)).normalized;
            skillCast.transform.position = transform.position;
            skillCast.SetActive(true);
            skillCastAngle = -Vector2.SignedAngle(Vector2.up, skillCastVector);
            skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        }

        skillCast.SetActive(false);
        ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
        SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
        // we won't use swordWeaponParent variable because it will affect attack logic
        Transform swordWeaponParent1 = swordWeapon.transform.parent;

        swordWeapon.ColliderDamage = 90f;
        swordWeaponParent1.position = skillableObject.skillCastOriginPoint.transform.position;
        swordWeaponParent1.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle - 90, 0));
        swordWeapon.SummonBigSword();
    }
}
