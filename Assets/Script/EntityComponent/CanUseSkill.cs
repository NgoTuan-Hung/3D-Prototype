using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanUseSkill : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private List<SkillBase> skills;
    [SerializeField] private List<String> skillNames;

    internal List<SkillBase> Skills { get => skills; set => skills = value; }

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    void Start()
    {
        if (customMonoBehavior.EntityType.Equals("Player"))
        {

        }
        else
        {
            skillNames.ForEach(skillName =>
            {
                skills.Add((SkillBase)gameObject.AddComponent(Type.GetType(skillName)));
            });
        }
    }
}
