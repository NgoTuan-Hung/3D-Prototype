using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            swordWeaponParent.position = target.position;
            swordWeaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, rotationDirection);
            swordWeaponParent.position = swordWeapon.transform.TransformPoint(0, 0, -2);
            swordWeapon.Attack();
            StartCoroutine(ResetAttack());

            Debug.Log("attacked");
        }
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }
}
