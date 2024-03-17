using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : MonoBehaviour
{
    private bool canAttack = true;
    private float attackCooldown = 1f;
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    public abstract void Attack(Transform location, Vector3 rotateDirection);
}
