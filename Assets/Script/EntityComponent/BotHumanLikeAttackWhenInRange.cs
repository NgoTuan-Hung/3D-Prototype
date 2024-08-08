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

    private void FixedUpdate() 
    {
        if (canAttack && customMonoBehavior.BotHumanLikeSimpleMoveToTarget.IsInsideAcceptableRange) Attack();
    }

    private Coroutine walkOrRunWithConditionCoroutine;
    private void Attack()
    {
        if (Random.value < attackChancePerInterval)
        {
            customMonoBehavior.BotHumanLikeSimpleMoveToTarget.StopAllMovement();
            customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Attack);
            walkOrRunWithConditionCoroutine = StartCoroutine(customMonoBehavior.BotHumanLikeSimpleMoveToTarget.WalkOrRunWithCondition
            (
                new Vector3(0, 0, 1), 0.3f, 
                () => 
                    {
                        return customMonoBehavior.BotHumanLikeSimpleMoveToTarget.DistanceToTarget < 0.1f;
                    }
            ));
        }
        else StartCoroutine(ResetAttack());
    }

    public void StopAttack()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Attack);
        StopCoroutine(walkOrRunWithConditionCoroutine);
        customMonoBehavior.BotHumanLikeSimpleMoveToTarget.CanMove = true;
    }

    public IEnumerator ResetAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
}
