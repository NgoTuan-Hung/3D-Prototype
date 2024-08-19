using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomMonoBehavior))]
public class CanUseSkill : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    [SerializeField] private List<SkillBase> skills = new List<SkillBase>();
    [SerializeField] private List<String> skillNames;
    public List<SkillBase> Skills { get => skills; set => skills = value; }

    private SkillBase skillBaseTemp;
    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        if (customMonoBehavior.EntityType.Equals("Player"))
        {

        }
        else
        {
            skillNames.ForEach(skillName =>
            {
                //skills.Add((SkillBase)gameObject.AddComponent(Type.GetType(skillName)));
                skillBaseTemp = (SkillBase)customMonoBehavior.GetComponent(skillName);
                skillBaseTemp.enabled = true;
                skills.Add(skillBaseTemp);
            });
        }
    }

    void Start()
    {
        
    }
}
