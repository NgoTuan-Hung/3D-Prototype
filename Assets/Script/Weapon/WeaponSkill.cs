using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : MonoBehaviour
{
    private bool canAttack = true;
    private float attackCooldown = 1f;
    private float attackDamage = 10f;
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }

    public abstract void Attack(Transform location, Vector3 rotateDirection);
}
