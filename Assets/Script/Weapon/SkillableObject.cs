using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillableObject : MonoBehaviour
{
    public List<WeaponSkill> weaponSkills = new List<WeaponSkill>();
    private void Start() 
    {
        // weaponSkills.ForEach(weaponSkill => 
        // {
        //     gameObject.AddComponent(weaponSkill.GetType());
        // });

        gameObject.AddComponent(Type.GetType("SwordSkill"));
        weaponSkills.Add((WeaponSkill)GetComponent("SwordSkill"));
    }
    public void PerformAttack(Transform location)
    {
        weaponSkills[0].Attack(location);
    }
}
