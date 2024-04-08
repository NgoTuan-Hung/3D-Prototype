using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator animator;
    new ParticleSystem particleSystem;
    private float colliderDamage = 0f;
    new private Rigidbody rigidbody;
    private Rigidbody parentRigidBody;
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public ParticleSystem ParticleSystem { get => particleSystem; set => particleSystem = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public Rigidbody ParentRigidBody { get => parentRigidBody; set => parentRigidBody = value; }

    public void StartParent() 
    {
        animator = GetComponent<Animator>();
        particleSystem = transform.GetChild(1).GetComponent<ParticleSystem>();
        //rigidbody = GetComponent<Rigidbody>();
        parentRigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();
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
