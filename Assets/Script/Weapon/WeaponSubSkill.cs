using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Video;

class WeaponSubSkill : MonoBehaviour
{
    private ObjectPool<Weapon> weaponPool;
    private SubSkillRequiredParameter subSkillRequiredParameter;
    private RecommendedAIBehavior recommendedAIBehavior;
    private bool canUse = true;
    private List<ISubSkillChangableAttribute> subSkillChangableAttributes = new List<ISubSkillChangableAttribute>();
    private CustomMonoBehavior customMonoBehavior;

    public ObjectPool<Weapon> WeaponPool { get => weaponPool; set => weaponPool = value; }
    public SubSkillRequiredParameter SubSkillRequiredParameter { get => subSkillRequiredParameter; set => subSkillRequiredParameter = value; }
    public RecommendedAIBehavior RecommendedAIBehavior { get => recommendedAIBehavior; set => recommendedAIBehavior = value; }
    public bool CanUse { get => canUse; set => canUse = value; }
    public List<ISubSkillChangableAttribute> SubSkillChangableAttributes { get => subSkillChangableAttributes; set => subSkillChangableAttributes = value; }
    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }

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
    Transform target;

    public Transform Target { get => target; set => target = value; }
}

interface ISubSkillChangableAttribute{}

public class SubSkillChangableAttribute<T> : ISubSkillChangableAttribute
{
    private T value;
    public enum SubSkillAttributeType {Cooldown, Speed, Distance}
    private SubSkillAttributeType subSkillAttributeType;

    public SubSkillChangableAttribute(T value, SubSkillAttributeType subSkillAttributeType)
    {
        this.value = value;
        this.subSkillAttributeType = subSkillAttributeType;
    }

    public T Value { get => value; set => this.value = value; }
    private SubSkillAttributeType SubSkillAttributeType1 { get => subSkillAttributeType; set => subSkillAttributeType = value; }
}