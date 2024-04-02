using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillableObject : MonoBehaviour
{
    public List<WeaponSkill> weaponSkills = new List<WeaponSkill>();
    public List<String> weaponSkillNames = new List<string>();
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool canAttack = true;
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    CustomMonoBehavior customMonoBehavior;
    public PlayerScript playerScript;
    public GameObject skillCastOriginPoint;
    private void Start() 
    {
        // weaponSkills.ForEach(weaponSkill => 
        // {
        //     gameObject.AddComponent(weaponSkill.GetType());
        // });
        customMonoBehavior = GetComponent<CustomMonoBehavior>();

        if (customMonoBehavior.entityType.Equals("Player"))
        {
            gameObject.AddComponent(Type.GetType("SwordSkill"));
            weaponSkills.Add((WeaponSkill)GetComponent("SwordSkill"));

            playerScript = GetComponent<PlayerScript>();
        }
        else
        {
            weaponSkillNames.ForEach(weaponSkillName => 
            {
                gameObject.AddComponent(Type.GetType(weaponSkillName));
                weaponSkills.Add((WeaponSkill)GetComponent(weaponSkillName));
            });
        }

        skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject;
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
