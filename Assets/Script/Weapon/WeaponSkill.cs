using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class WeaponSkill : MonoBehaviour
{
    private bool canAttack = true;
    private float attackCooldown = 1f;
    private float attackDamage = 10f;
    private CustomMonoBehavior customMonoBehavior;
    private List<WeaponSubSkill> weaponSubSkills = new List<WeaponSubSkill>();
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    internal List<WeaponSubSkill> WeaponSubSkills { get => weaponSubSkills; set => weaponSubSkills = value; }

    public abstract void Attack(Transform location, Vector3 rotateDirection);
    public virtual void Awake()
    {
        CustomMonoBehavior = GetComponent<CustomMonoBehavior>();
    }
}
