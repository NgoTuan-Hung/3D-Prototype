using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMonoBehavior : MonoBehaviour
{
    [SerializeField] private String entityType;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float curentHealth;
    [SerializeField] private List<String> allyTags = new List<string>();
    new protected Rigidbody rigidbody;
    private SkillableObject skillableObject;
    private RotatableObject rotatableObject;
    private Animator animator;
    private MoveToTarget moveToTarget;
    private MeleeSimpleAttackWhenNear meleeSimpleAttackWhenNear;
    private PlayerScript playerScript;
    private GameObject skillCastOriginPoint;
    bool rigidbodyBool = false;
    bool skillableObjectBool = false;
    bool animatorBool = false;
    bool rotatableObjectBool = false;
    bool moveToTargetBool = false;
    bool meleeSimpleAttackWhenNearBool = false;
    bool playerInputSystemBool = false;
    bool playerScriptBool = false;
    bool skillCastOriginPointBool = false;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurentHealth { get => curentHealth; set => curentHealth = value; }
    public string EntityType { get => entityType; set => entityType = value; }
    public List<string> AllyTags { get => allyTags; set => allyTags = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public SkillableObject SkillableObject { get => skillableObject; set => skillableObject = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public RotatableObject RotatableObject { get => rotatableObject; set => rotatableObject = value; }
    public bool RigidbodyBool { get => rigidbodyBool; set => rigidbodyBool = value; }
    public bool SkillableObjectBool { get => skillableObjectBool; set => skillableObjectBool = value; }
    public bool AnimatorBool { get => animatorBool; set => animatorBool = value; }
    public bool RotatableObjectBool { get => rotatableObjectBool; set => rotatableObjectBool = value; }
    public MoveToTarget MoveToTarget { get => moveToTarget; set => moveToTarget = value; }
    public bool MoveToTargetBool { get => moveToTargetBool; set => moveToTargetBool = value; }
    public MeleeSimpleAttackWhenNear MeleeSimpleAttackWhenNear { get => meleeSimpleAttackWhenNear; set => meleeSimpleAttackWhenNear = value; }
    public bool MeleeSimpleAttackWhenNearBool { get => meleeSimpleAttackWhenNearBool; set => meleeSimpleAttackWhenNearBool = value; }
    public bool PlayerInputSystemBool { get => playerInputSystemBool; set => playerInputSystemBool = value; }
    public bool PlayerScriptBool { get => playerScriptBool; set => playerScriptBool = value; }
    public PlayerScript PlayerScript { get => playerScript; set => playerScript = value; }
    public GameObject SkillCastOriginPoint { get => skillCastOriginPoint; set => skillCastOriginPoint = value; }
    public bool SkillCastOriginPointBool { get => skillCastOriginPointBool; set => skillCastOriginPointBool = value; }

    // Start is called before the first frame update
    public void Awake()
    {
        curentHealth = maxHealth;
        allyTags.Add(gameObject.tag);

        if (TryGetComponent<Rigidbody>(out rigidbody)) rigidbodyBool = true;
        if (TryGetComponent<SkillableObject>(out skillableObject)) skillableObjectBool = true;
        if (TryGetComponent<Animator>(out animator)) animatorBool = true;
        if (TryGetComponent<RotatableObject>(out rotatableObject)) rotatableObjectBool = true;
        if (TryGetComponent<MoveToTarget>(out moveToTarget)) moveToTargetBool = true;
        if (TryGetComponent<MeleeSimpleAttackWhenNear>(out meleeSimpleAttackWhenNear)) meleeSimpleAttackWhenNearBool = true;
        if (TryGetComponent<PlayerScript>(out playerScript)) playerScriptBool = true;

        if ((skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject) != null) skillCastOriginPointBool = true;
    }

    public void UpdateHealth(float value)
    {
        curentHealth -= value;

        if (curentHealth <= 0) {gameObject.SetActive(false); curentHealth = maxHealth;}
        else if (curentHealth > maxHealth) curentHealth = maxHealth;
    }
}