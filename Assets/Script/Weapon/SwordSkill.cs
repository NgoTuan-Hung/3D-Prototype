using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject swordPrefab;
    private Transform swordWeaponParent;

    private void Start() 
    {
        swordPrefab = Instantiate(Resources.Load("LongSword")).GameObject();
        swordPrefab.SetActive(false);
        weaponPool ??= new ObjectPool<Weapon>(swordPrefab, 20);

        SkillableObject = GetComponent<SkillableObject>();
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

            skillCastVector = (SkillableObject.PlayerScript.playerInputSystem.Control.View.ReadValue<Vector2>() - (Vector2)Camera.main.WorldToScreenPoint(SkillableObject.SkillCastOriginPoint.transform.position)).normalized;
            skillCast.transform.position = transform.position;
            skillCast.SetActive(true);
            skillCastAngle = -Vector2.SignedAngle(Vector2.up, skillCastVector);
            skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        }

        skillCast.SetActive(false);
        SkillableObject.UseOnlySkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
        SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
        // we won't use swordWeaponParent variable because it will affect attack logic
        Transform swordWeaponParent1 = swordWeapon.transform.parent;

        swordWeapon.ColliderDamage = 90f;
        swordWeaponParent1.position = SkillableObject.SkillCastOriginPoint.transform.position;
        swordWeaponParent1.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle - 90, 0));
        swordWeapon.Animator.SetBool("BigSword", true);
        SkillableObject.PlayerScript.animator.SetBool("CastSkillBlownDown", true);
        SkillableObject.PlayerScript.animator.Play("UpperBody.CastSkillBlowDown", 1, 0);
        StartCoroutine(StopSummon());
        StartCoroutine(StopSword(swordWeapon));
    }

    public IEnumerator StopSword(SwordWeapon swordWeapon)
    {
        yield return new WaitForSeconds(swordWeapon.BigSwordClip.length);
        swordWeapon.Animator.SetBool("BigSword", false);
        SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        
        yield return new WaitForSeconds(1);
        swordWeapon.transform.parent.gameObject.SetActive(false);
        swordWeapon.transform.localScale = Vector3.one;
    }

    IEnumerator StopSummon()
    {
        yield return new WaitForSeconds(SkillableObject.CastSkillBlownDown.length);

        SkillableObject.PlayerScript.animator.SetBool("CastSkillBlownDown", false);
    }

    [SerializeField] private Vector3[] thousandSwordOriginalRotation = {new Vector3(-45, -90, 90), new Vector3(-90, 0, 0), new Vector3(-45, 90, -90)};
    [SerializeField] private float startFlyingForce = 30f;
    public void ThousandSword(InputAction.CallbackContext callbackContext)
    {
        List<ObjectPoolClass<Weapon>> objectPoolClasses = weaponPool.Pick(3);
        SwordWeapon swordWeapon;
        Transform swordWeaponParent1;
        GameObject target = SkillableObject.PlayerScript.TargetableObject.TargetChecker.NearestTarget;
        SkillableObject.PlayerScript.animator.SetBool("HandUpCast", true);
        SkillableObject.PlayerScript.animator.Play("UpperBody.HandUpCast", 1, 0);
        SkillableObject.UseOnlySkillAnimator((int)SkillableObject.SkillID.ThousandSword);
        for (int i=0;i<thousandSwordOriginalRotation.Length;i++)
        {
            swordWeapon = (SwordWeapon)objectPoolClasses[i].Component;
            swordWeapon.ColliderDamage = 20f;
            swordWeaponParent1 = swordWeapon.transform.parent;
            swordWeaponParent1.transform.rotation = Quaternion.Euler(thousandSwordOriginalRotation[i]);
            swordWeaponParent1.position = SkillableObject.SkillCastOriginPoint.transform.position;
            swordWeapon.FlyingTrail.enabled = true;
            swordWeapon.Animator.SetBool("ThousandSword", true);
            swordWeapon.ParentRigidBody.AddForce(swordWeaponParent1.transform.forward * startFlyingForce, ForceMode.Impulse);
            CoroutineWrapper coroutineWrapper = new CoroutineWrapper();
            IEnumerator thousandSwordCoroutine = ThousandSwordCoroutine(swordWeapon, swordWeaponParent1, target, coroutineWrapper);
            coroutineWrapper.coroutine = StartCoroutine(thousandSwordCoroutine);
        }
    }

    [SerializeField] private float flyingAtTargetSpeed = 1f;
    [SerializeField] private float rotateSpeed = 10;
    private float rotateSpeedPerDeltaTime;
    public IEnumerator ThousandSwordCoroutine(SwordWeapon swordWeapon, Transform swordWeaponParent1, GameObject target, CoroutineWrapper coroutineWrapper)
    {
        yield return new WaitForSeconds(1);

        Vector3 moveDirection = target.transform.position - swordWeaponParent1.transform.position;
        moveDirection = moveDirection.normalized * flyingAtTargetSpeed;
        rotateSpeedPerDeltaTime = rotateSpeed * Time.fixedDeltaTime;
        swordWeapon.ParentRigidBody.velocity = Vector3.zero;
        StartCoroutine(StopThousandSword(swordWeapon, swordWeaponParent1, coroutineWrapper.coroutine));

        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            swordWeaponParent1.transform.position += moveDirection;
            swordWeaponParent1.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards
            (
                swordWeaponParent1.transform.forward
                , target.transform.position - swordWeaponParent1.transform.position, rotateSpeedPerDeltaTime, 0f
            ));
        }
    }

    public IEnumerator StopThousandSword(SwordWeapon swordWeapon, Transform swordWeaponParent1, Coroutine thousandSwordCoroutine)
    {
        yield return new WaitForSeconds(2);

        swordWeapon.Animator.SetBool("ThousandSword", false);
        SkillableObject.PlayerScript.animator.SetBool("HandUpCast", false);
        SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.ThousandSword);
        StopCoroutine(thousandSwordCoroutine);
        swordWeaponParent1.rotation = Quaternion.Euler(0, 0, 0);
        swordWeapon.transform.localPosition = Vector3.zero;
        swordWeapon.FlyingTrail.enabled = false;
        swordWeaponParent1.gameObject.SetActive(false);
    }
}
