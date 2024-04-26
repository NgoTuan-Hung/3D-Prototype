using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator animator;
    protected ParticleSystem attackParticleSystem;
    protected float colliderDamage = 0f;
    protected float baseColliderDamage = 0f;
    new protected Rigidbody rigidbody;
    protected Rigidbody parentRigidBody;
    protected TrailRenderer attackTrail;
    protected List<Collider> colliders = new List<Collider>();
    protected List<string> collideExcludeTags = new List<string>();
    public float ColliderDamage { get => colliderDamage; set => colliderDamage = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public ParticleSystem AttackParticleSystem { get => attackParticleSystem; set => attackParticleSystem = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public Rigidbody ParentRigidBody { get => parentRigidBody; set => parentRigidBody = value; }
    public float BaseColliderDamage { get => baseColliderDamage; set => baseColliderDamage = value; }
    public List<Collider> Colliders { get => colliders; set => colliders = value; }
    public List<string> CollideExcludeTags { get => collideExcludeTags; set => collideExcludeTags = value; }

    public void Awake() 
    {
        animator = GetComponent<Animator>();
        
        rigidbody = GetComponent<Rigidbody>();
        parentRigidBody = transform.parent.gameObject.GetComponent<Rigidbody>();

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
        animator.SetBool("Attack", false);
        transform.parent.gameObject.SetActive(false);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!collideExcludeTags.Contains(collision.gameObject.tag))
        {
            Debug.Log("exclude tag: " + collideExcludeTags[0]);
            GlobalObject.Instance.UpdateCustomonoBehaviorHealth(colliderDamage, collision.gameObject);
        }
    }
}
