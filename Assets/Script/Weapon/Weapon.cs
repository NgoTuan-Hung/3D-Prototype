using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public ObjectPool weaponPool;
    public GameObject weaponPrefab;
    public bool canAttack = true;
    public float attackCooldown;

    public Weapon(GameObject weaponPrefab, int poolSize, float attackCooldown)
    {
        this.weaponPrefab = weaponPrefab;
        weaponPool = new ObjectPool(weaponPrefab, poolSize);
        this.attackCooldown = attackCooldown;
    }
}
