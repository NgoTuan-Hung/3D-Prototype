using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator animator;
    new ParticleSystem particleSystem;
    private float colliderDamage = 0f;

    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }

    public void StartParent() 
    {
        animator = GetComponent<Animator>();
        particleSystem = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void PlayAttackParticleSystem()
    {
        if (!particleSystem.isPlaying) particleSystem.Play();
    }

    public void Attack()
    {
        animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        transform.parent.gameObject.SetActive(false);
    }

    public void OnCollisionEnterParent(Collision collision)
    {
        GlobalObject.Instance.UpdateCombatEntityHealth(colliderDamage, collision.gameObject);
    }
}
