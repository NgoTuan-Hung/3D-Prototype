using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillableObject : MonoBehaviour
{
    public List<WeaponSkill> weaponSkills = new List<WeaponSkill>();
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool canAttack = true;
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    private void Start() 
    {
        // weaponSkills.ForEach(weaponSkill => 
        // {
        //     gameObject.AddComponent(weaponSkill.GetType());
        // });

        gameObject.AddComponent(Type.GetType("SwordSkill"));
        weaponSkills.Add((WeaponSkill)GetComponent("SwordSkill"));
    }

    private void FixedUpdate()
    {
        if (weaponSkills[0].CanAttack)
        {
            CanAttack = true;
        } else CanAttack = false;
    }
    public void PerformAttack(Transform location, Vector3 rotateDirection)
    {
        if (weaponSkills[0].CanAttack)
        {
            weaponSkills[0].Attack(location, rotateDirection);
        }
    }
}
