using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CustomMonoBehavior))]
public class SkillBase : MonoBehaviour
{
    public delegate void FinishSkillDelegate();
    public FinishSkillDelegate finishSkillDelegate;
    private ObjectPool weaponPool;
    private SubSkillRequiredParameter subSkillRequiredParameter;
    private RecommendedAIBehavior recommendedAIBehavior = new RecommendedAIBehavior();
    private bool canUse = true;
    private List<SubSkillChangableAttribute> subSkillChangableAttributes = new List<SubSkillChangableAttribute>();
    private SubSkillCondition subSkillCondition = new SubSkillCondition();
    private CustomMonoBehavior customMonoBehavior;
    private State animatorStateForSkill;
    private bool upperBodyCheckForAnimationTransition;
    [SerializeField] private float executionTimeAfterAnimationFrame;
    [SerializeField] private float useSkillChance;
    protected bool freeze = false;
    public ObjectPool WeaponPool { get => weaponPool; set => weaponPool = value; }
    public SubSkillRequiredParameter SubSkillRequiredParameter { get => subSkillRequiredParameter; set => subSkillRequiredParameter = value; }
    public RecommendedAIBehavior RecommendedAIBehavior { get => recommendedAIBehavior; set => recommendedAIBehavior = value; }
    public bool CanUse { get => canUse; set => canUse = value; }
    public List<SubSkillChangableAttribute> SubSkillChangableAttributes { get => subSkillChangableAttributes; set => subSkillChangableAttributes = value; }
    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public SubSkillCondition SubSkillCondition { get => subSkillCondition; set => subSkillCondition = value; }
    public State AnimatorStateForSkill { get => animatorStateForSkill; set => animatorStateForSkill = value; }
    public float ExecutionTimeAfterAnimationFrame { get => executionTimeAfterAnimationFrame; set => executionTimeAfterAnimationFrame = value; }
    public float UseSkillChance { get => useSkillChance; set => useSkillChance = value; }
    public bool UpperBodyCheckForAnimationTransition { get => upperBodyCheckForAnimationTransition; set => upperBodyCheckForAnimationTransition = value; }

    public virtual void Trigger(SubSkillParameter subSkillParameter)
    {

    }

    public virtual void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        if (upperBodyCheckForAnimationTransition) checkForAnimationTransitionDelegate = () => customMonoBehavior.HumanLikeAnimatorBrain.CheckTransitionUpper(animatorStateForSkill);
        else checkForAnimationTransitionDelegate = () => customMonoBehavior.HumanLikeAnimatorBrain.CheckTransitionLower(animatorStateForSkill);
    }

    public delegate bool CheckForAnimationTransitionDelegate();
    private CheckForAnimationTransitionDelegate checkForAnimationTransitionDelegate;
    public bool CheckForAnimationTransition()
    {
        return checkForAnimationTransitionDelegate();
    }

    public virtual bool CheckCanUseSkill()
    {
        if (canUse && CheckForAnimationTransition() && customMonoBehavior.CustomMonoBehaviorState1 == CustomMonoBehavior.CustomMonoBehaviorState.Available)
        return true; 

        return false;
    }

    public virtual void Start()
    {
        
    }

    public void Freeze () => freeze = true;
    public void UnFreeze () => freeze = false;
}

public class SubSkillRequiredParameter
{
    bool target;

    public bool Target { get => target; set => target = value; }
}

public class SubSkillParameter
{
    Transform target = null;

    public Transform Target { get => target; set => target = value; }
}

public class SubSkillCondition
{
    bool stopMoving = false;
    bool stopRotating = false;
    public bool StopMoving { get => stopMoving; set => stopMoving = value; }
    public bool StopRotating { get => stopRotating; set => stopRotating = value; }
}

[Serializable]
public class SubSkillChangableAttribute
{
    [SerializeField] private int intValue;
    [SerializeField] private float floatValue;
    [SerializeField] private float[] floatArray;
    [SerializeField] private Vector3 vector3;
    public enum SubSkillAttributeType {Cooldown, Speed, Distance, Timers, Position, TimeScale, CastRange, ParticleSystemTimeEnd, Duration}
    public enum SubSkillAttributeValueType {Int, Float, FloatArray, Vector3}
    private SubSkillAttributeType subSkillAttributeType;
    private SubSkillAttributeValueType subSkillAttributeValueType;

    public SubSkillChangableAttribute(SubSkillAttributeValueType type, object value, SubSkillAttributeType subSkillAttributeType)
    {
        this.subSkillAttributeValueType = type;
        switch (subSkillAttributeValueType)
        {
            case SubSkillAttributeValueType.Int:
                intValue = (int)value;
                break;
            case SubSkillAttributeValueType.Float:
                floatValue = (float)value;
                break;
            case SubSkillAttributeValueType.FloatArray:
                floatArray = (float[])value;
                break;
            case SubSkillAttributeValueType.Vector3:
                vector3 = (Vector3)value;
                break;
            default: break;
        }
        this.subSkillAttributeType = subSkillAttributeType;
    }
    private SubSkillAttributeType SubSkillAttributeType1 { get => subSkillAttributeType; set => subSkillAttributeType = value; }
    public int IntValue { get => intValue; set => intValue = value; }
    public float FloatValue { get => floatValue; set => floatValue = value; }
    public float[] FloatArray { get => floatArray; set => floatArray = value; }
    public Vector3 Vector3 { get => vector3; set => vector3 = value; }
}