using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior))]
public class BotHumanLikeAttackWhenInRange : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();    
    }

    private float attackChancePerInterval = 0.5f;
    private float attackInterval = 1;
    private bool canAttack = true;
    [SerializeField] private float stopMoveWhenAtDistance = 0.4f;

    private void FixedUpdate() 
    {
        if (freeze) return;
        if (customMonoBehavior.BotHumanLikeSimpleMoveToTarget.Zone == 2 && customMonoBehavior.BotHumanLikeLookAtTarget.IsLookingAtTarget)
        {
            if (canAttack && customMonoBehavior.HumanLikeAnimatorBrain.CheckTransitionUpper(State.Attack) && customMonoBehavior.CustomMonoBehaviorState1 == CustomMonoBehavior.CustomMonoBehaviorState.Available) Attack();
        }
    }

    private Coroutine walkOrRunWithConditionCoroutine;

    public bool CanAttack { get => canAttack; set => canAttack = value; }

    private void Attack()
    {
        if (Random.value < attackChancePerInterval)
        {
            customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Attack);
            walkOrRunWithConditionCoroutine = StartCoroutine(customMonoBehavior.BotHumanLikeSimpleMoveToTarget.WalkOrRunWithCondition
            (
                new Vector3(0, 0, 1), 0.2f, 
                () => 
                    {
                        return customMonoBehavior.BotHumanLikeSimpleMoveToTarget.DistanceToTarget < stopMoveWhenAtDistance;
                    }
            ));
            canAttack = false;
        }
        else StartCoroutine(ResetAttack());
    }

    public void StopAttack()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Attack);
        if (CoroutineWrapper.CheckCoroutineNotNull(walkOrRunWithConditionCoroutine)) StopCoroutine(walkOrRunWithConditionCoroutine);
        customMonoBehavior.BotHumanLikeSimpleMoveToTarget.InitDefaultBehavior();
        canAttack = true;
    }

    public IEnumerator ResetAttack()
    {
        while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }

    private bool freeze = false;
    public void Freeze() => freeze = true;
    public void UnFreeze() => freeze = false;
}
