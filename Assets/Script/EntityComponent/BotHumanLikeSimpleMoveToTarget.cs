using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(BotHumanLikeLookAtTarget), typeof(MoveToTarget))]
public class BotHumanLikeSimpleMoveToTarget : MonoBehaviour 
{
    private CustomMonoBehavior customMonoBehavior;
    private int zone;

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }    

    private void FixedUpdate() 
    {
        distanceToTarget = Vector3.Distance(customMonoBehavior.Target.transform.position, transform.position);
        if (distanceToTarget < minAcceptableDistance) zone = 1;
        else if (distanceToTarget < maxAcceptableDistance) zone = 2;
        else zone = 3;

        if (canMove) MoveToTarget();
    }

    private bool canMove = true;
    [SerializeField] private float minAcceptableDistance = 3;
    [SerializeField] private float maxAcceptableDistance = 4.5f;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float walkAwayMinChance = 0.3f;
    [SerializeField] private float acceptableWalkChance = 0.5f;
    [SerializeField] private float walkToMaxChance = 0.3f;
    public void MoveToTarget()
    {
        if (walkToPatternBlock || runToPatternBlock) return;
        if (zone == 1)
        {
            if (Random.value < walkAwayMinChance) StartCoroutine(WalkToPattern(new Vector3(Random.Range(-1f,1f), 0, -1), Random.Range(0.1f, 1f)));
            else StartCoroutine(RunToPattern(new Vector3(Random.Range(-1f,1f), 0, -1), Random.Range(0.1f, 1f)));
        }
        else
        {
            if (zone == 2)
            {
                if (Random.value < acceptableWalkChance) StartCoroutine(WalkToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
                else StartCoroutine(RunToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
            }
            else
            {
                if (Random.value < walkToMaxChance) StartCoroutine(WalkToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
                else StartCoroutine(RunToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
            }
        }
    }

    private bool walkToPatternBlock = false;
    public IEnumerator WalkToPattern(Vector3 direction, float duration)
    {
        walkToPatternBlock = true;
        float passedTime = 0;
        while (true)
        {
            customMonoBehavior.HumanLikeMovable.Move(direction);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime >= duration) break;
        }
        walkToPatternBlock = false;
    }

    private bool runToPatternBlock = false;

    public bool CanMove { get => canMove; set => canMove = value; }
    public float MinAcceptableDistance { get => minAcceptableDistance; set => minAcceptableDistance = value; }
    public float MaxAcceptableDistance { get => maxAcceptableDistance; set => maxAcceptableDistance = value; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float WalkAwayMinChance { get => walkAwayMinChance; set => walkAwayMinChance = value; }
    public float AcceptableWalkChance { get => acceptableWalkChance; set => acceptableWalkChance = value; }
    public float WalkToMaxChance { get => walkToMaxChance; set => walkToMaxChance = value; }
    public int Zone { get => zone; set => zone = value; }

    public IEnumerator RunToPattern(Vector3 direction, float duration)
    {
        runToPatternBlock = true;
        float passedTime = 0;
        while (true)
        {
            customMonoBehavior.HumanLikeMovable.Run(direction);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime >= duration) break;
        }
        runToPatternBlock = false;
    }

    public IEnumerator WalkOrRunWithCondition(Vector3 direction, float walkChance, Func<bool> condition)
    {
        while (true)
        {
            if (condition()) 
            {
                StopAllMovement();
                canMove = true;
                break;
            }

            if (walkToPatternBlock || runToPatternBlock) {}
            else
            {
                if (Random.value < walkChance) StartCoroutine(WalkToPattern(direction, Random.Range(0.1f, 1f)));
                else StartCoroutine(RunToPattern(direction, Random.Range(0.1f, 1f)));
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void StopAllMovement()
    {
        canMove = false;
        StopAllCoroutines();
        runToPatternBlock = false;
        walkToPatternBlock = false;
    }
}