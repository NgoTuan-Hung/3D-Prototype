using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MoveToTarget))]
public class MeleeSimpleAttackWhenNear : MonoBehaviour
{
    Animator animator;
    MoveToTarget moveToTarget;
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
        attackCooldown = 0.5f;
        canAttack = true;
    }

    private void FixedUpdate()
    {
        CheckAttack();
    }

    public void CheckAttack()
    {
        if (canAttack && moveToTarget.DistanceToTarget < distanceToAttack)
        {
            canAttack = false;
            animator.SetBool("Attack", true);
            moveToTarget.CanMove = false;
        }
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        moveToTarget.CanMove = true;
        StartCoroutine(ResetAttack());
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}
