using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CustomMonoBehavior))]
public class SkillableObject : MonoBehaviour
{
    public List<WeaponSkill> weaponSkills = new List<WeaponSkill>();
    [SerializeField] private List<String> weaponSkillNames = new List<string>();
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool canAttack = true;
    CustomMonoBehavior customMonoBehavior;
    private PlayerScript playerScript;
    private GameObject skillCastOriginPoint;
    private UtilObject utilObject = new UtilObject();
    private AnimationClip castSkillBlownDown;
    private int animatorIsUsingSkill = 0;
    public enum SkillID {SummonBigSword = 1, ThousandSword = 2}
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public PlayerScript PlayerScript { get => playerScript; set => playerScript = value; }
    public GameObject SkillCastOriginPoint { get => skillCastOriginPoint; set => skillCastOriginPoint = value; }
    public UtilObject UtilObject { get => utilObject; set => utilObject = value; }
    public AnimationClip CastSkillBlownDown { get => castSkillBlownDown; set => castSkillBlownDown = value; }
    public int AnimatorIsUsingSkill { get => animatorIsUsingSkill; set => animatorIsUsingSkill = value; }

    private void Start() 
    {
        // weaponSkills.ForEach(weaponSkill => 
        // {
        //     gameObject.AddComponent(weaponSkill.GetType());
        // });
        customMonoBehavior = GetComponent<CustomMonoBehavior>();

        if (customMonoBehavior.EntityType.Equals("Player"))
        {
            playerScript = GetComponent<PlayerScript>();
            LoadPlayerSkillData();

            castSkillBlownDown = playerScript.animator.runtimeAnimatorController.animationClips.FirstOrDefault((animatorClip) => animatorClip.name.Equals("CastSkillBlowDown"));
            skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject;
        }
        else
        {
            weaponSkillNames.ForEach(weaponSkillName => 
            {
                gameObject.AddComponent(Type.GetType(weaponSkillName));
                weaponSkills.Add((WeaponSkill)GetComponent(weaponSkillName));
            });
        }

        
    }

    public void LoadPlayerSkillData()
    {
        List<PlayerSkillData> playerSkillDatas = GlobalObject.Instance.FromJson<PlayerSkillData>(GlobalObject.Instance.playerSkillDataPath, true);

        playerSkillDatas.ForEach(playerSkillData => 
        {
            Type classType = Type.GetType(playerSkillData.skillName);
            WeaponSkill weaponSkill = (WeaponSkill)gameObject.AddComponent(classType);
            weaponSkills.Add(weaponSkill);

            utilObject.BindKey(playerScript.playerInputSystem, playerSkillData.keybind, playerSkillData.functionName, classType, weaponSkill);
        });
    }

    private void FixedUpdate()
    {
        if (weaponSkills[0].CanAttack)
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

    public void UseSkillAnimator(int skillID)
    {
        AnimatorIsUsingSkill |= (1 << skillID);
    }

    public void UseOnlySkillAnimator(int skillID)
    {
        AnimatorIsUsingSkill = 1 << skillID;
    }

    public void StopSkillAnimator(int skillID)
    {
        AnimatorIsUsingSkill &= ~(1 << skillID);
    }
}
