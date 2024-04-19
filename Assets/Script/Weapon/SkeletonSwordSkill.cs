using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSwordSkill : WeaponSkill
{
    [SerializeField] private static ObjectPool<Weapon> weaponPool {get; set;}
    [SerializeField] private GameObject prefab;
    private Transform weaponParent;

    public override void Attack(Transform location, Vector3 rotateDirection)
    {
        
    }

    public bool testAttack = false;
    public void TestAttack()
    {
        if (testAttack)
        {
            testAttack = false;
            ObjectPoolClass<Weapon> objectPoolClass = weaponPool.PickOne();
            SkeletonSwordWeapon skeletonSwordWeapon = (SkeletonSwordWeapon)objectPoolClass.Component;
            weaponParent = skeletonSwordWeapon.transform.parent;
            skeletonSwordWeapon.ColliderDamage = 10f;

            weaponParent.transform.position = transform.position;
            skeletonSwordWeapon.Attack();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        prefab = Instantiate(Resources.Load("SkeletonSword")) as GameObject;
        prefab.SetActive(false);
        weaponPool ??= new ObjectPool<Weapon>(prefab, 20);

        SkillableObject = GetComponent<SkillableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        TestAttack();
    }
}
