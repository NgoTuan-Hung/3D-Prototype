using System;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Video;

class WeaponSubSkill : MonoBehaviour
{
    private ObjectPool<Weapon> weaponPool;
    private SubSkillRequiredParameter subSkillRequiredParameter;
    private RecommendedAIBehavior recommendedAIBehavior;
    private bool canUse = true;

    public ObjectPool<Weapon> WeaponPool { get => weaponPool; set => weaponPool = value; }
    public SubSkillRequiredParameter SubSkillRequiredParameter { get => subSkillRequiredParameter; set => subSkillRequiredParameter = value; }
    public RecommendedAIBehavior RecommendedAIBehavior { get => recommendedAIBehavior; set => recommendedAIBehavior = value; }
    public bool CanUse { get => canUse; set => canUse = value; }

    public virtual void Trigger(SubSkillParameter subSkillParameter)
    {

    }
}

public class SubSkillRequiredParameter
{
    bool target;

    public bool Target { get => target; set => target = value; }
}

public class SubSkillParameter
{
    Transform target;
}

public class SubSkillChangableAttribute
{
    private object value;
    public enum SubSkillAttributeType {Cooldown, Speed, Distance}
    private SubSkillAttributeType subSkillAttributeType;

    public SubSkillChangableAttribute(object value, SubSkillAttributeType subSkillAttributeType)
    {
        this.value = value;
        this.subSkillAttributeType = subSkillAttributeType;
    }

    public object Value { get => value; set => this.value = value; }
    private SubSkillAttributeType SubSkillAttributeType1 { get => subSkillAttributeType; set => subSkillAttributeType = value; }
}