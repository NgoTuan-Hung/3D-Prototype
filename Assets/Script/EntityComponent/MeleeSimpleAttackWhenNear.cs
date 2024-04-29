using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeSimpleAttackWhenNear : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float attackCooldown;
    [SerializeField] private bool canAttack;

    public float DistanceToAttack { get => distanceToAttack; set => distanceToAttack = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    // Start is called before the first frame update
    void Awake()
    {
        distanceToAttack = 1.5f;
        canAttack = true;
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void FixedUpdate()
    {
        CheckAttack();
    }

    public void CheckAttack()
    {
        if (customMonoBehavior.SkillableObjectBool && customMonoBehavior.MoveToTargetBool)
        {
            if (customMonoBehavior.SkillableObject.CanAttack && customMonoBehavior.MoveToTarget.DistanceToTarget < distanceToAttack)
            {
                customMonoBehavior.Animator.Play("Base.Attack", 0, 0);
                customMonoBehavior.SkillableObject.PerformAttack(customMonoBehavior.MoveToTarget.Target, customMonoBehavior.MoveToTarget.Target.position - transform.position);
                customMonoBehavior.MoveToTarget.CanMove = false;
            }
        }
    }

    public void StopAttack()
    {
        customMonoBehavior.MoveToTarget.CanMove = true;
        StartCoroutine(ResetAttack());
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}
