
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

class SwordSkillSummonBigSword : WeaponSubSkill
{
    private bool isWaiting = false;
    private Coroutine summonCoroutine;
    private Vector2 skillCastVector;
    private GameObject skillCast;
    private float skillCastAngle;

    public bool IsWaiting { get => isWaiting; set => isWaiting = value; }
    public Coroutine SummonCoroutine { get => summonCoroutine; set => summonCoroutine = value; }
    public Vector2 SkillCastVector { get => skillCastVector; set => skillCastVector = value; }
    public GameObject SkillCast { get => skillCast; set => skillCast = value; }
    public float SkillCastAngle { get => skillCastAngle; set => skillCastAngle = value; }

    public override void Start()
    {
        base.Start();
    }
    public void SummonBigSword(InputAction.CallbackContext callbackContext)
    {
        if (!isWaiting)
        {
            StartCoroutine(HandleSummonSwordPlayer());
        } else isWaiting = false;
    }

    public IEnumerator HandleSummonSwordPlayer()
    {
        isWaiting = true;
        while (isWaiting)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            skillCastVector = (CustomMonoBehavior.SkillableObject.PlayerScript.playerInputSystem.Control.View.ReadValue<Vector2>() 
            - (Vector2)Camera.main.WorldToScreenPoint(CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position)).normalized;
            skillCast.transform.position = transform.position;
            skillCast.SetActive(true);
            skillCastAngle = -Vector2.SignedAngle(Vector2.up, skillCastVector);
            skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        }

        skillCast.SetActive(false);
        CustomMonoBehavior.SkillableObject.UseOnlySkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        ObjectPoolClass<Weapon> objectPoolClass = WeaponPool.PickOneWithoutActive();

        SwordWeapon swordWeapon = (SwordWeapon)objectPoolClass.Component;
        swordWeapon.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.SkillableObject.CustomMonoBehavior.AllyTags;
        objectPoolClass.GameObject.SetActive(true);
        // we won't use swordWeaponParent variable because it will affect attack logic
        Transform swordWeaponParent1 = swordWeapon.transform.parent;

        swordWeapon.CollideAndDamage.ColliderDamage = 90f;
        swordWeaponParent1.position = CustomMonoBehavior.SkillableObject.SkillCastOriginPoint.transform.position;
        swordWeaponParent1.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle - 90, 0));
        swordWeapon.Animator.SetBool("BigSword", true);
        CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", true);
        CustomMonoBehavior.Animator.Play("UpperBody.CastSkillBlowDown", 1, 0);
        StartCoroutine(StopSummon());
        StartCoroutine(StopSword(swordWeapon));
    }

    public IEnumerator StopSword(SwordWeapon swordWeapon)
    {
        yield return new WaitForSeconds(swordWeapon.BigSwordClip.length);
        swordWeapon.Animator.SetBool("BigSword", false);
        CustomMonoBehavior.SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        
        yield return new WaitForSeconds(1);
        swordWeapon.transform.parent.gameObject.SetActive(false);
        swordWeapon.transform.localScale = Vector3.one;
    }

    IEnumerator StopSummon()
    {
        yield return new WaitForSeconds(CustomMonoBehavior.SkillableObject.CastSkillBlownDown.length);

        CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", false);
    }
}