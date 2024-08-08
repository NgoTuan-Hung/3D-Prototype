using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMonoBehavior : MonoBehaviour, IComparable<CustomMonoBehavior>
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
    private FeetChecker feetChecker;
    private JumpableObject jumpableObject;
    private HumanLikeAnimatorBrain humanLikeAnimatorBrain;
    private HumanLikeMovable humanLikeMovable;
    private BotHumanLikeSimpleMoveToTarget botHumanLikeSimpleMoveToTarget;
    private BotHumanLikeLookAtTarget botHumanLikeLookAtTarget;
    private BotHumanLikeAttackWhenInRange botHumanLikeAttackWhenInRange;
    private new Camera camera;
    private GameObject cameraPoint;
    private GameObject lookAtConstraintObjectParent;
    private GameObject lookAtConstraintObject;
    private Vector3 cameraPointToCameraVector;
    private GameObject target;
    public static ObjectPool freeObjectPool;
    private bool botHumanLikeSimpleMoveToTargetBool = false;
    private bool botHumanLikeLookAtTargetBool = false;
    private bool botHumanLikeAttackWhenInRangeBool = false;
    private bool humanLikeMovableBool = false;
    private bool canRotateThisFrame;
    private bool isControlByPlayer = false;
    private bool cameraBool = false;
    private bool humanLikeAnimatorBrainBool = false;
    private bool jumpableObjectBool = false;
    private bool feetCheckerBool = false;
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
    public FeetChecker FeetChecker { get => feetChecker; set => feetChecker = value; }
    public bool FeetCheckerBool { get => feetCheckerBool; set => feetCheckerBool = value; }
    public JumpableObject JumpableObject { get => jumpableObject; set => jumpableObject = value; }
    public bool JumpableObjectBool { get => jumpableObjectBool; set => jumpableObjectBool = value; }
    public HumanLikeAnimatorBrain HumanLikeAnimatorBrain { get => humanLikeAnimatorBrain; set => humanLikeAnimatorBrain = value; }
    public Camera Camera { get => camera; set => camera = value; }
    public GameObject CameraPoint { get => cameraPoint; set => cameraPoint = value; }
    public GameObject LookAtConstraintObjectParent { get => lookAtConstraintObjectParent; set => lookAtConstraintObjectParent = value; }
    public GameObject LookAtConstraintObject { get => lookAtConstraintObject; set => lookAtConstraintObject = value; }
    public Vector3 CameraPointToCameraVector { get => cameraPointToCameraVector; set => cameraPointToCameraVector = value; }
    public bool CameraBool { get => cameraBool; set => cameraBool = value; }
    public bool HumanLikeAnimatorBrainBool { get => humanLikeAnimatorBrainBool; set => humanLikeAnimatorBrainBool = value; }
    public bool CanRotateThisFrame { get => canRotateThisFrame; set => canRotateThisFrame = value; }
    public bool IsControlByPlayer { get => isControlByPlayer; set => isControlByPlayer = value; }
    public GameObject Target { get => target; set => target = value; }
    public bool HumanLikeMovableBool { get => humanLikeMovableBool; set => humanLikeMovableBool = value; }
    public HumanLikeMovable HumanLikeMovable { get => humanLikeMovable; set => humanLikeMovable = value; }
    public BotHumanLikeSimpleMoveToTarget BotHumanLikeSimpleMoveToTarget { get => botHumanLikeSimpleMoveToTarget; set => botHumanLikeSimpleMoveToTarget = value; }
    public BotHumanLikeLookAtTarget BotHumanLikeLookAtTarget { get => botHumanLikeLookAtTarget; set => botHumanLikeLookAtTarget = value; }
    public BotHumanLikeAttackWhenInRange BotHumanLikeAttackWhenInRange { get => botHumanLikeAttackWhenInRange; set => botHumanLikeAttackWhenInRange = value; }
    public bool BotHumanLikeSimpleMoveToTargetBool { get => botHumanLikeSimpleMoveToTargetBool; set => botHumanLikeSimpleMoveToTargetBool = value; }
    public bool BotHumanLikeLookAtTargetBool { get => botHumanLikeLookAtTargetBool; set => botHumanLikeLookAtTargetBool = value; }
    public bool BotHumanLikeAttackWhenInRangeBool { get => botHumanLikeAttackWhenInRangeBool; set => botHumanLikeAttackWhenInRangeBool = value; }

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
        if (TryGetComponent<HumanLikeAnimatorBrain>(out humanLikeAnimatorBrain)) humanLikeAnimatorBrainBool = true;
        if (TryGetComponent<HumanLikeMovable>(out humanLikeMovable)) humanLikeMovableBool = true;
        if (TryGetComponent<BotHumanLikeSimpleMoveToTarget>(out botHumanLikeSimpleMoveToTarget)) botHumanLikeSimpleMoveToTargetBool = true;
        if (TryGetComponent<BotHumanLikeLookAtTarget>(out botHumanLikeLookAtTarget)) botHumanLikeLookAtTargetBool = true;
        if (TryGetComponent<BotHumanLikeAttackWhenInRange>(out botHumanLikeAttackWhenInRange)) botHumanLikeAttackWhenInRangeBool = true;

        if ((skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject) != null) skillCastOriginPointBool = true;
        if ((feetChecker = GetComponentInChildren<FeetChecker>()) != null) feetCheckerBool = true;
        if ((jumpableObject = GetComponentInChildren<JumpableObject>()) != null) jumpableObjectBool = true;

        if (entityType.Equals("Player"))
        {
            camera = Camera.main;
            cameraBool = true;
            isControlByPlayer = true;
        }
        else
        {
            
        }

        cameraPoint = transform.Find("CameraPoint").gameObject;
        lookAtConstraintObjectParent = transform.Find("LookAtConstraintObjectParent").gameObject;
        lookAtConstraintObject = transform.Find("LookAtConstraintObject").gameObject;

        GameObject freeObjectPrefab = Resources.Load("FreeObject") as GameObject;
        freeObjectPool ??= new ObjectPool(freeObjectPrefab, 20, new PoolArgument(typeof(GameObject), PoolArgument.WhereComponent.Self));
    }

    public void UpdateHealth(float value)
    {
        curentHealth -= value;

        if (curentHealth <= 0) {gameObject.SetActive(false); curentHealth = maxHealth;}
        else if (curentHealth > maxHealth) curentHealth = maxHealth;
    }

    public int CompareTo(CustomMonoBehavior other)
    {
        return gameObject.GetInstanceID().CompareTo(other.gameObject.GetInstanceID());
    }
    private void FixedUpdate() 
    {

    }
}