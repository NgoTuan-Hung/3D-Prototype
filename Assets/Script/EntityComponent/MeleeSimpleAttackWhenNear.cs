using System.Collections;
using UnityEngine;

public class MeleeSimpleAttackWhenNear : EntityAction
{
    [SerializeField] private float distanceToAttack;
    [SerializeField] private float attackCooldown;
    [SerializeField] private bool canAttack;

    public float DistanceToAttack { get => distanceToAttack; set => distanceToAttack = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        distanceToAttack = 1.5f;
        canAttack = true;
    }

    public override void Start()
    {
        base.Start();
        if (CustomMonoBehavior.SkillableObjectBool && CustomMonoBehavior.MoveToTargetBool) attackDelegate += CheckAttack;
    }

    private void FixedUpdate()
    {
        attackDelegate?.Invoke();
    }

    private delegate void AttackDelegate();
    private AttackDelegate attackDelegate;
    public void CheckAttack()
    {
        if (canAttack)
            if (CustomMonoBehavior.SkillableObject.CanAttack && CustomMonoBehavior.MoveToTarget.DistanceToTarget < distanceToAttack)
            {
                CustomMonoBehavior.Animator.Play("Base.Attack", 0, 0);
                CustomMonoBehavior.SkillableObject.PerformAttack(CustomMonoBehavior.MoveToTarget.Target, CustomMonoBehavior.MoveToTarget.Target.position - transform.position);
                CustomMonoBehavior.MoveToTarget.CanMove = false;
            }
    }

    public void StopAttack()
    {
        CustomMonoBehavior.MoveToTarget.CanMove = true;
    }
}
