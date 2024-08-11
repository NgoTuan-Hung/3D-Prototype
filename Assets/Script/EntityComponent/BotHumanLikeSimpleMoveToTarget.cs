using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(BotHumanLikeLookAtTarget))]
public class BotHumanLikeSimpleMoveToTarget : MonoBehaviour 
{
    private CustomMonoBehavior customMonoBehavior;
    private int zone;
    private bool modeMovingToTarget = true;
    public delegate void MoveDelegate();
    public MoveDelegate MoveDelegateMethod;
    [SerializeField] private float modeMovingToTargetChance = 0.8f;
    [SerializeField] private float XSecond = 10;
    private bool canChangeMode = true;
    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        StartCoroutine(ChangeModeEveryXSecond());
        walkToPatternCoroutine = customMonoBehavior.NullCoroutine();
        runToPatternCoroutine = customMonoBehavior.NullCoroutine();
        MoveDelegateMethod = MoveToTarget;
    }

    public IEnumerator ChangeModeEveryXSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(XSecond);
            while (!canChangeMode)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            if (Random.value < modeMovingToTargetChance)
            {
                modeMovingToTarget = true;
                MoveDelegateMethod = MoveToTarget;
                if (!customMonoBehavior.BotHumanLikeLookAtTarget.IsLookingAtTarget) customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode();
            }
            else
            {
                modeMovingToTarget = false;
                MoveDelegateMethod = FreeMove;
                if (customMonoBehavior.BotHumanLikeLookAtTarget.IsLookingAtTarget) customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode();
            }
        }
    }

    private void FixedUpdate() 
    {
        distanceToTarget = Vector3.Distance(customMonoBehavior.Target.transform.position, transform.position);
        if (distanceToTarget < minAcceptableDistance) zone = 1;
        else if (distanceToTarget < maxAcceptableDistance) zone = 2;
        else zone = 3;

        if (canMove) MoveDelegateMethod?.Invoke();
    }

    private bool canMove = true;
    [SerializeField] private float minAcceptableDistance = 3;
    [SerializeField] private float maxAcceptableDistance = 4.5f;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float walkAwayMinChance = 0.3f;
    [SerializeField] private float acceptableWalkChance = 0.5f;
    [SerializeField] private float walkToMaxChance = 0.3f;
    private Coroutine walkToPatternCoroutine;
    private Coroutine runToPatternCoroutine;
    public void MoveToTarget()
    {
        if (walkToPatternBlock || runToPatternBlock) return;
        if (zone == 1)
        {
            if (Random.value < walkAwayMinChance) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(Random.Range(-1f,1f), 0, -1), Random.Range(0.1f, 1f)));
            else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(Random.Range(-1f,1f), 0, -1), Random.Range(0.1f, 1f)));
        }
        else
        {
            if (zone == 2)
            {
                if (Random.value < acceptableWalkChance) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
                else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
            }
            else
            {
                if (Random.value < walkToMaxChance) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
                else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
            }
        }
    }

    public void FreeMove()
    {
        if (walkToPatternBlock || runToPatternBlock) return;
        if (Random.value < 0.5f) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
        else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f)), Random.Range(0.1f, 2f)));
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
    public bool CanChangeMode { get => canChangeMode; set => canChangeMode = value; }

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
                canChangeMode = true;
                break;
            }

            if (walkToPatternBlock || runToPatternBlock) {}
            else
            {
                if (Random.value < walkChance) walkToPatternCoroutine = StartCoroutine(WalkToPattern(direction, Random.Range(0.1f, 1f)));
                else runToPatternCoroutine = StartCoroutine(RunToPattern(direction, Random.Range(0.1f, 1f)));
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public void StopAllMovement()
    {
        canMove = false;
        canChangeMode = false;
        StopCoroutine(walkToPatternCoroutine);
        StopCoroutine(runToPatternCoroutine);
        runToPatternBlock = false;
        walkToPatternBlock = false;
    }
}