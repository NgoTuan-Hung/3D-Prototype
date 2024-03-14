using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject swordPrefab;

    private void Start() 
    {
        swordPrefab = Instantiate(Resources.Load("LongSword")).GameObject();
        weaponPool ??= new ObjectPool<Weapon>(swordPrefab, 20);
    }

    public void Attack(Transform target, Vector3 rotationDirection)
    {
        if (CanAttack)
        {
            CanAttack = false;
            ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
            SwordWeapon swordWeapon = objectPoolClass.Component as SwordWeapon;
            
            swordWeapon.PlayAttackParticleSystem();
            swordWeapon.transform.position = target.position;
            swordWeapon.transform.rotation = Quaternion.FromToRotation(Vector3.forward, rotationDirection);
            swordWeapon.transform.position = swordWeapon.transform.TransformPoint(0, 0, -2);
            swordWeapon.Attack();
            StartCoroutine(ResetAttack());
        }
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }
}
