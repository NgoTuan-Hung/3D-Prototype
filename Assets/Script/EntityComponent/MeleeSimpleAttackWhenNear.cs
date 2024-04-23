using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveToTarget), typeof(CustomMonoBehavior), typeof(SkillableObject))]
public class MeleeSimpleAttackWhenNear : MonoBehaviour
{
    private Animator animator;
    private MoveToTarget moveToTarget;
    private CustomMonoBehavior customMonoBehavior;
    private SkillableObject skillableObject;
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float attackCooldown;
    [SerializeField] private bool canAttack;

    public float DistanceToAttack { get => distanceToAttack; set => distanceToAttack = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        moveToTarget = GetComponent<MoveToTarget>();
        distanceToAttack = 1.5f;
        canAttack = true;
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        skillableObject = GetComponent<SkillableObject>();
    }

    private void FixedUpdate()
    {
        CheckAttack();
    }

    public void CheckAttack()
    {
        if (skillableObject.CanAttack && moveToTarget.DistanceToTarget < distanceToAttack)
        {
            animator.Play("Base.Attack", 0, 0);
            skillableObject.PerformAttack(moveToTarget.Target, moveToTarget.Target.position - transform.position);
            moveToTarget.CanMove = false;
        }
    }

    public void StopAttack()
    {
        moveToTarget.CanMove = true;
        StartCoroutine(ResetAttack());
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}
