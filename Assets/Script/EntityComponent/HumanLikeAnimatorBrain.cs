using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum State {Idle = 0, Walk = 1, Attack = 2, Jump = 3, Land = 4, Run = 5, CastSpellShort = 6, CastSpellMiddle = 7, CastSpellLong = 8};
public class HumanLikeAnimatorBrain : MonoBehaviour
{
    public CustomMonoBehavior customMonoBehavior;
    public bool onAir;
    public bool prevOnAir = false;
    public State currentLowerBodyState;
    public State defaultLowerBodyState = State.Idle;
    public List<GroundCheck> groundChecks = new List<GroundCheck>();
    public TransitionRule[][] lowerBodyTransitionRules;
    public State currentUpperBodyState;
    public State defaultUpperBodyState = State.Idle;
    public TransitionRule[][] upperBodyTransitionRules;

    public bool ChangeState(State newState)
    {
        if (currentLowerBodyState != newState)
        {
            if (onAir)
            {
                if (lowerBodyTransitionRules[(int)currentLowerBodyState][(int)newState].OnAirOutCome)
                {
                    PlayLowerState(newState);
                }
            }
            else
            {
                if (lowerBodyTransitionRules[(int)currentLowerBodyState][(int)newState].OnGroundOutCome)
                {
                    PlayLowerState(newState);
                }
            }
        }

        if (currentUpperBodyState != newState)
        {
            if (onAir)
            {
                if (upperBodyTransitionRules[(int)currentUpperBodyState][(int)newState].OnAirOutCome)
                {
                    PlayUpperState(newState);
                    return true;
                }
            }
            else
            {
                if (upperBodyTransitionRules[(int)currentUpperBodyState][(int)newState].OnGroundOutCome)
                {
                    PlayUpperState(newState);
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckTransitionUpper(State newState)
    {
        if (currentUpperBodyState != newState)
        {
            if (onAir) return upperBodyTransitionRules[(int)currentUpperBodyState][(int)newState].OnAirOutCome;
            else return upperBodyTransitionRules[(int)currentUpperBodyState][(int)newState].OnGroundOutCome;
        }

        return false;
    }

    public bool CheckTransitionLower(State newState)
    {
        if (currentLowerBodyState != newState)
        {
            if (onAir) return lowerBodyTransitionRules[(int)currentLowerBodyState][(int)newState].OnAirOutCome;
            else return lowerBodyTransitionRules[(int)currentLowerBodyState][(int)newState].OnGroundOutCome;
        }

        return false;
    }

    public void PlayLowerState(State state)
    {
        switch (state)
        {
            case State.Idle:
                customMonoBehavior.Animator.Play("Idle", 0, 0);
                break;
            case State.Walk:
                customMonoBehavior.Animator.Play("Walk", 0, 0);
                break;
            case State.Attack:
                customMonoBehavior.Animator.Play("Attack", 0, 0);
                break;
            case State.Jump:
                customMonoBehavior.Animator.Play("Jump", 0, 0);
                break;
            case State.Land:
                customMonoBehavior.Animator.Play("Land", 0, 0);
                break;
            case State.Run:
                customMonoBehavior.Animator.Play("Run", 0, 0);
                break;
            case State.CastSpellShort:
                customMonoBehavior.Animator.Play("CastSpellShort", 0, 0);
                break;
            case State.CastSpellMiddle:
                customMonoBehavior.Animator.Play("CastSpellMiddle", 0, 0);
                break;
            case State.CastSpellLong:
                customMonoBehavior.Animator.Play("CastSpellLong", 0, 0);
                break;
        }
        currentLowerBodyState = state;
    }

    public void PlayUpperState(State state)
    {
        switch (state)
        {
            case State.Idle:
                customMonoBehavior.Animator.Play("Idle", 1, 0);
                break;
            case State.Walk:
                customMonoBehavior.Animator.Play("Walk", 1, 0);
                break;
            case State.Attack:
                customMonoBehavior.Animator.Play("Attack", 1, 0);
                break;
            case State.Jump:
                customMonoBehavior.Animator.Play("Jump", 1, 0);
                break;
            case State.Land:
                customMonoBehavior.Animator.Play("Land", 1, 0);
                break;
            case State.Run:
                customMonoBehavior.Animator.Play("Run", 1, 0);
                break;
            case State.CastSpellShort:
                customMonoBehavior.Animator.Play("CastSpellShort", 1, 0);
                break;
            case State.CastSpellMiddle:
                customMonoBehavior.Animator.Play("CastSpellMiddle", 1, 0);
                break;
            case State.CastSpellLong:
                customMonoBehavior.Animator.Play("CastSpellLong", 1, 0);
                break;
        }
        currentUpperBodyState = state;
    }

    public delegate void StopStateDelegate();
    public StopStateDelegate stopStateDelegate;
    public void StopState(State state)
    {
        if (currentLowerBodyState == state)
        {
            if (onAir) customMonoBehavior.Animator.Play("Jump", 0, 0);
            else customMonoBehavior.Animator.Play("Idle", 0, 0);
            currentLowerBodyState = State.Idle;
        }

        if (currentUpperBodyState == state)
        {
            if (onAir) customMonoBehavior.Animator.Play("OnAir", 1, 0);
            else customMonoBehavior.Animator.Play("Idle", 1, 0);
            currentUpperBodyState = State.Idle;
        }

        stopStateDelegate?.Invoke();
        stopStateDelegate = null;
    }

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void Start() 
    {
        lowerBodyTransitionRules = GlobalObject.Instance.LowerBodyTransitionRules;
        upperBodyTransitionRules = GlobalObject.Instance.UpperBodyTransitionRules;
    }

    private void FixedUpdate() 
    {
        GroundCheck();
    }

    public float acceptableGroundDistance = 0.05f;
    public void GroundCheck()
    {
        onAir = false;
        for (int i=0;i<groundChecks.Count;i++)
        {
            if (groundChecks[i].hit.distance > acceptableGroundDistance)
            {
                onAir = true;
                break;
            }
        }
        if (!onAir && prevOnAir) {ChangeState(State.Land);}
        prevOnAir = onAir;
    }

    public enum AddEventForClipOfStateTimeType {Start, End, Value}
    public void AddEventForClipOfState(String functionName, State state, AddEventForClipOfStateTimeType timeType, float timeValue)
    {
        string clipName = state switch
        {
            State.Idle => gameObject.name + "Idle",
            State.Walk => gameObject.name + "Walk",
            State.Attack => gameObject.name + "Attack",
            State.Jump => gameObject.name + "Jump",
            State.Land => gameObject.name + "Land",
            State.Run => gameObject.name + "Run",
            State.CastSpellShort => gameObject.name + "CastSpellShort",
            State.CastSpellMiddle => gameObject.name + "CastSpellMiddle",
            State.CastSpellLong => gameObject.name + "CastSpellLong",
            _ => gameObject.name + "Idle",
        };

        AnimationClip clip = customMonoBehavior.Animator.runtimeAnimatorController.animationClips.First(clip => clip.name.Equals(clipName));
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;

        switch (timeType)
        {
            case AddEventForClipOfStateTimeType.Start:
                animationEvent.time = 0;
                break;
            case AddEventForClipOfStateTimeType.End:
                animationEvent.time = clip.length;
                break;
            case AddEventForClipOfStateTimeType.Value:
                animationEvent.time = timeValue;
                break;
        }

        clip.AddEvent(animationEvent);
    }

    public void StopLand()
    {
        StopState(State.Land);
    }

    public void Freeze()
    {
        customMonoBehavior.Animator.speed = 0;
    }

    public void UnFreeze()
    {
        customMonoBehavior.Animator.speed = 1;
    }
}

public class TransitionRule
{
    public bool OnAirOutCome;
    public bool OnGroundOutCome;

    public TransitionRule(bool onAirOutCome, bool onGroundOutCome)
    {
        OnAirOutCome = onAirOutCome;
        OnGroundOutCome = onGroundOutCome;
    }
}
