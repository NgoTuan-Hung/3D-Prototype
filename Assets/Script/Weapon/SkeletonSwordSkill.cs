using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class SkeletonSwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject prefab;
    private Transform weaponParent;

    public override void Attack(Transform location, Vector3 rotateDirection)
    {
        if (CanAttack)
        {
            CanAttack = false;
            ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
            SkeletonSwordWeapon skeletonSwordWeapon = (SkeletonSwordWeapon)objectPoolClass.Component;
            weaponParent = skeletonSwordWeapon.transform.parent;

            weaponParent.transform.position = transform.position;
            weaponParent.rotation = Quaternion.FromToRotation(Vector3.forward, location.position - skeletonSwordWeapon.transform.position + new Vector3(0, 1f, 0));
            skeletonSwordWeapon.ColliderDamage = 10f;
            
            skeletonSwordWeapon.Attack();
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(AttackCooldown);

        CanAttack = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        prefab = Instantiate(Resources.Load("SkeletonSword")) as GameObject;
        prefab.SetActive(false);
        weaponPool ??= new ObjectPool<Weapon>(prefab, 20, ObjectPool<Weapon>.WhereComponent.Child);
        Debug.Log("count: " + weaponPool.pool.Count);
        

        SkillableObject = GetComponent<SkillableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
    
    }
}
