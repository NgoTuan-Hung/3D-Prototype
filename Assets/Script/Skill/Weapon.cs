using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator animator;
    protected ParticleSystem attackParticleSystem;
    new protected Rigidbody rigidbody;
    protected Rigidbody parentRigidBody;
    private TrailRenderer attackTrail;
    private CollideAndDamage collideAndDamage;
    protected List<Collider> colliders = new List<Collider>();
    public delegate void OnEnableDelegate();
    public OnEnableDelegate onEnableDelegate;
    public delegate void OnDisableDelegate();
    public OnDisableDelegate onDisableDelegate;
    public delegate void AfterEnableDelegate();
    public AfterEnableDelegate afterEnableDelegate;
    public delegate void AfterDisableDelegate();
    public AfterDisableDelegate afterDisableDelegate;
    public Animator Animator { get => animator; set => animator = value; }
    public ParticleSystem AttackParticleSystem { get => attackParticleSystem; set => attackParticleSystem = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public Rigidbody ParentRigidBody { get => parentRigidBody; set => parentRigidBody = value; }
    public List<Collider> Colliders { get => colliders; set => colliders = value; }
    public CollideAndDamage CollideAndDamage { get => collideAndDamage; set => collideAndDamage = value; }
    public TrailRenderer AttackTrail { get => attackTrail; set => attackTrail = value; }

    public virtual void Awake() 
    {
        animator = GetComponent<Animator>();
        
        rigidbody = GetComponent<Rigidbody>();
        parentRigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();
        collideAndDamage = GetComponent<CollideAndDamage>();

        if (transform.GetChild(1).TryGetComponent<ParticleSystem>(out attackParticleSystem))
        {
            attackParticleSystem.Stop();
        }
        if (transform.GetChild(3).TryGetComponent<TrailRenderer>(out attackTrail))
        {
            attackTrail.enabled = false;
        }
    }

    public void PlayAttackParticleSystem()
    {
        //if (!particleSystem.isPlaying) particleSystem.Play();
    }

    public virtual void Attack()
    {
        animator.SetBool("Attack", true);
    }

    public virtual void StopAttack()
    {
        Debug.Log("Stop Attack Called");
        animator.SetBool("Attack", false);
        transform.parent.gameObject.SetActive(false);
    }
}
