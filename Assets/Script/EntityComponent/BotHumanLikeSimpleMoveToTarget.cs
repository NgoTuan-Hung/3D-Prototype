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
    public delegate void MoveDelegate();
    public MoveDelegate MoveDelegateMethod;
    public enum MoveMode {MoveToTargetWithCaution, FreeMove, MoveToTargetMindlessly, MoveAroundTarget, None}
    private MoveMode moveMode = MoveMode.None;
    [SerializeField] private float modeMovingToTargetWithCautionChance = 0.8f;
    [SerializeField] private float XSecond = 10;
    private bool canChangeModeEveryXSecond = true;
    public enum Intelligence {Dumb, Average, Smart}
    [SerializeField] private Intelligence intelligence;
    public Intelligence IntelligenceMode {get => intelligence; set 
    {
        intelligence = value;
        switch (value)
        {
            case Intelligence.Dumb:
                modeMovingToTargetWithCautionChance = 0.3f;
                XSecond = 10;
                break;
            case Intelligence.Average:
                modeMovingToTargetWithCautionChance = 0.5f;
                XSecond = 5;
                break;
            case Intelligence.Smart:
                modeMovingToTargetWithCautionChance = 0.8f;
                XSecond = 10;
                break;
        }
    }}
    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        walkToPatternCoroutine = customMonoBehavior.NullCoroutine();
        runToPatternCoroutine = customMonoBehavior.NullCoroutine();
    }

    void Start()
    {
        InitDefaultBehavior();
    }

    public void ChangeMode(MoveMode mode)
    {
        if (mode != moveMode) 
        {
            StopAllMovement();
            switch (mode)
            {
                case MoveMode.MoveToTargetMindlessly:
                    moveMode = MoveMode.MoveToTargetMindlessly;
                    customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode(BotHumanLikeLookAtTarget.LookMode.LookAtTarget);
                    MoveDelegateMethod = MoveToTargetMindlessly;
                    break;
                case MoveMode.MoveToTargetWithCaution:
                    moveMode = MoveMode.MoveToTargetWithCaution;
                    customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode(BotHumanLikeLookAtTarget.LookMode.LookAtTarget);
                    MoveDelegateMethod = MoveToTargetWithCaution;
                    break;
                case MoveMode.FreeMove:
                    moveMode = MoveMode.FreeMove;
                    customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode(BotHumanLikeLookAtTarget.LookMode.FreeLook);
                    MoveDelegateMethod = FreeMove;
                    break;
                case MoveMode.MoveAroundTarget:
                    moveMode = MoveMode.MoveAroundTarget;
                    customMonoBehavior.BotHumanLikeLookAtTarget.ChangeMode(BotHumanLikeLookAtTarget.LookMode.LookAroundTarget);
                    MoveDelegateMethod = MoveAroundTarget;
                    break;
                case MoveMode.None:
                    moveMode = MoveMode.None;
                    MoveDelegateMethod = null;
                    break;
                default: return;
            }
        }
    }

    public Coroutine changeBetweenTwoDefaultModeEveryXSecondCoroutine;
    public void InitDefaultBehavior()
    {
        /* Default behavior is moving with caution to target 
        or just look and move randomly */
        changeBetweenTwoDefaultModeEveryXSecondCoroutine = StartCoroutine(ChangeBetweenTwoDefaultModeEveryXSecond());
    }

    public void StopDefaultBehavior()
    {
        if (changeBetweenTwoDefaultModeEveryXSecondCoroutine != null) StopCoroutine(changeBetweenTwoDefaultModeEveryXSecondCoroutine);
    }

    public IEnumerator ChangeBetweenTwoDefaultModeEveryXSecond()
    {
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);

            if (Random.value < modeMovingToTargetWithCautionChance) ChangeMode(MoveMode.MoveToTargetWithCaution);
            else ChangeMode(MoveMode.FreeMove);
            yield return new WaitForSeconds(XSecond);
        }
    }

    private void FixedUpdate() 
    {
        distanceToTarget = Vector3.Distance(customMonoBehavior.Target.transform.position, transform.position);
        if (distanceToTarget < minAcceptableDistance) zone = 1;
        else if (distanceToTarget < maxAcceptableDistance) zone = 2;
        else zone = 3;

        if (freeze) return;
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
    public void MoveToTargetWithCaution()
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

    public void MoveToTargetMindlessly()
    {
        if (walkToPatternBlock || runToPatternBlock) return;
        /* Just move or run forward while looking at target */
        if (Random.value < 0.5f) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
        else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
    }

    public void MoveAroundTarget()
    {
        if (walkToPatternBlock || runToPatternBlock) return;
        /* Just move forward while looking around target, see ChangeMode() for more info */
        if (Random.value < 0.5f) walkToPatternCoroutine = StartCoroutine(WalkToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
        else runToPatternCoroutine = StartCoroutine(RunToPattern(new Vector3(0, 0, 1), Random.Range(0.1f, 1f)));
    }

    private bool walkToPatternBlock = false;
    private bool canWalkToPattern = true;
    public IEnumerator WalkToPattern(Vector3 direction, float duration)
    {
        walkToPatternBlock = true;
        float passedTime = 0;
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
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
    public bool CanChangeMode { get => canChangeModeEveryXSecond; set => canChangeModeEveryXSecond = value; }
    public MoveMode MoveMode1 { get => moveMode; set => moveMode = value; }
    public bool CanRunToPattern { get => canRunToPattern; set => canRunToPattern = value; }
    public bool CanWalkOrRunWithCondition { get => canWalkOrRunWithCondition; set => canWalkOrRunWithCondition = value; }

    private bool canRunToPattern = true;
    public IEnumerator RunToPattern(Vector3 direction, float duration)
    {
        runToPatternBlock = true;
        float passedTime = 0;
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
            customMonoBehavior.HumanLikeMovable.Run(direction);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime >= duration) break;
        }
        runToPatternBlock = false;
    }

    private bool canWalkOrRunWithCondition = true;
    public IEnumerator WalkOrRunWithCondition(Vector3 direction, float walkChance, Func<bool> condition)
    {
        /* Walk or run in direction while condition is true */
        StopDefaultBehavior();
        ChangeMode(MoveMode.None);
        /* This loop run every frame until the condition is true */
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
            if (condition()) 
            {
                InitDefaultBehavior();
                break;
            }

            /* Our walk or run logic */
            if (walkToPatternBlock || runToPatternBlock) {}
            else
            {
                if (Random.value < walkChance) walkToPatternCoroutine = StartCoroutine(WalkToPattern(direction, Random.Range(0.1f, 1f)));
                else runToPatternCoroutine = StartCoroutine(RunToPattern(direction, Random.Range(0.1f, 1f)));
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private bool customModeWithConditionTimeOut = false;
    private bool canCustomModeWithCondition = true;
    public IEnumerator CustomModeWithCondition(bool conditionBool, Func<bool> condition, bool timerBool, float timer, Action customMode, float interval)
    {
        StopAllMovement();
        StopDefaultBehavior();
        if (conditionBool)
        {
            while (true)
            {
                while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
                if (condition()) 
                {
                    break;
                }

                customMode();
                yield return new WaitForSeconds(interval);
            }
        }
        else if (timerBool)
        {
            customModeWithConditionTimeOut = true;
            StartCoroutine(CustomModeWithConditionTimer(timer));
            while (customModeWithConditionTimeOut)
            {
                while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
                customMode();
                yield return new WaitForSeconds(interval);
            }
        }
        StopAllMovement();
        InitDefaultBehavior();
    }

    public IEnumerator CustomModeWithConditionTimer(float totalTime)
    {
        float passedTime = 0;
        while (true)
        {
            while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime > totalTime)
            {
                customModeWithConditionTimeOut = false;
                break;
            }
        }
    }

    public void StopAllMovement()
    {
        if (CoroutineWrapper.CheckCoroutineNotNull(walkToPatternCoroutine)) StopCoroutine(walkToPatternCoroutine);
        if (CoroutineWrapper.CheckCoroutineNotNull(runToPatternCoroutine)) StopCoroutine(runToPatternCoroutine);
        runToPatternBlock = false;
        walkToPatternBlock = false;
    }

    private bool freeze = false;
    public void Freeze() => freeze = true;

    public void UnFreeze() => freeze = false;
}