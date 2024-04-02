using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomMonoBehavior))]
public class SkillableObject : MonoBehaviour
{
    public List<WeaponSkill> weaponSkills = new List<WeaponSkill>();
    public List<String> weaponSkillNames = new List<string>();
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool canAttack = true;
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    CustomMonoBehavior customMonoBehavior;
    public PlayerScript playerScript;
    public GameObject skillCastOriginPoint;
    UtilObject utilObject = new UtilObject();
    private void Start() 
    {
        // weaponSkills.ForEach(weaponSkill => 
        // {
        //     gameObject.AddComponent(weaponSkill.GetType());
        // });
        customMonoBehavior = GetComponent<CustomMonoBehavior>();

        if (customMonoBehavior.entityType.Equals("Player"))
        {
            playerScript = GetComponent<PlayerScript>();
            LoadPlayerSkillData();
        }
        else
        {
            weaponSkillNames.ForEach(weaponSkillName => 
            {
                gameObject.AddComponent(Type.GetType(weaponSkillName));
                weaponSkills.Add((WeaponSkill)GetComponent(weaponSkillName));
            });
        }

        skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject;
    }

    public void LoadPlayerSkillData()
    {
        List<PlayerSkillData> playerSkillDatas = GlobalObject.Instance.FromJson<PlayerSkillData>(GlobalObject.Instance.playerSkillDataPath, true);

        playerSkillDatas.ForEach(playerSkillData => 
        {
            Type classType = Type.GetType(playerSkillData.skillName);
            gameObject.AddComponent(classType);
            WeaponSkill weaponSkill = (WeaponSkill)GetComponent(playerSkillData.skillName);
            weaponSkills.Add(weaponSkill);

            utilObject.BindKey(playerScript.playerInputSystem, playerSkillData.keybind, playerSkillData.functionName, classType, weaponSkill);
        });
    }

    private void FixedUpdate()
    {
        if (weaponSkills.Count != 0 && weaponSkills[0].CanAttack)
        {
            CanAttack = true;
        } else CanAttack = false;
    }

    public void PerformAttack(Transform location, Vector3 rotateDirection)
    {
        if (weaponSkills[0].CanAttack)
        {
            weaponSkills[0].Attack(location, rotateDirection);
        }
    }
}
