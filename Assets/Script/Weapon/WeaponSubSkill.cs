using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

class WeaponSubSkill : MonoBehaviour
{
    public delegate void FinishSkillDelegate();
    public FinishSkillDelegate finishSkillDelegate;
    private ObjectPool<Weapon> weaponPool;
    private SubSkillRequiredParameter subSkillRequiredParameter;
    private RecommendedAIBehavior recommendedAIBehavior = new RecommendedAIBehavior();
    private bool canUse = true;
    private List<SubSkillChangableAttribute> subSkillChangableAttributes = new List<SubSkillChangableAttribute>();
    private SubSkillCondition subSkillCondition = new SubSkillCondition();
    private CustomMonoBehavior customMonoBehavior;

    public ObjectPool<Weapon> WeaponPool { get => weaponPool; set => weaponPool = value; }
    public SubSkillRequiredParameter SubSkillRequiredParameter { get => subSkillRequiredParameter; set => subSkillRequiredParameter = value; }
    public RecommendedAIBehavior RecommendedAIBehavior { get => recommendedAIBehavior; set => recommendedAIBehavior = value; }
    public bool CanUse { get => canUse; set => canUse = value; }
    public List<SubSkillChangableAttribute> SubSkillChangableAttributes { get => subSkillChangableAttributes; set => subSkillChangableAttributes = value; }
    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public SubSkillCondition SubSkillCondition { get => subSkillCondition; set => subSkillCondition = value; }

    public virtual void Trigger(SubSkillParameter subSkillParameter)
    {

    }

    public virtual void Start()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }
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
    public bool StopMoving { get => stopMoving; set => stopMoving = value; }
}


public class SubSkillChangableAttribute
{
    private int intValue;
    private float floatValue;
    public enum SubSkillAttributeType {Cooldown, Speed, Distance}
    public enum SubSkillAttributeValueType {Int, Float}
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
            default: break;
        }
        this.subSkillAttributeType = subSkillAttributeType;
    }
    private SubSkillAttributeType SubSkillAttributeType1 { get => subSkillAttributeType; set => subSkillAttributeType = value; }
    public int IntValue { get => intValue; set => intValue = value; }
    public float FloatValue { get => floatValue; set => floatValue = value; }
}