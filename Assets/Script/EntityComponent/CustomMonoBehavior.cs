using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkeletonSwordSkillNonstopThrust), typeof(SwordSkillSummonBigSword), typeof(SwordSkillThousandSword))]
[RequireComponent(typeof(LightingRain), typeof(FlameRider))]
public class CustomMonoBehavior : MonoBehaviour, IComparable<CustomMonoBehavior>
{
    [SerializeField] private String entityType;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float curentHealth;
    [SerializeField] private List<String> allyTags = new List<string>();
    new protected Rigidbody rigidbody;
    private RotatableObject rotatableObject;
    private Animator animator;
    private GameObject skillCastOriginPoint;
    private SkinnedMeshRenderer mainSkinnedMeshRenderer;
    private HumanLikeJumpableObject humanLikeJumpableObject;
    private HumanLikeAnimatorBrain humanLikeAnimatorBrain;
    private HumanLikeMovable humanLikeMovable;
    private HumanLikeLookable humanLikeLookable;
    private BotHumanLikeSimpleMoveToTarget botHumanLikeSimpleMoveToTarget;
    private BotHumanLikeLookAtTarget botHumanLikeLookAtTarget;
    private BotHumanLikeAttackWhenInRange botHumanLikeAttackWhenInRange;
    private BotHumanLikeJumpRandomly botHumanLikeJumpRandomly;
    private BotUseSkillWheneverPossible botUseSkillWheneverPossible; 
    private CanUseSkill canUseSkill;
    private new Camera camera;
    private GameObject cameraPoint;
    private GameObject lookAtConstraintObjectParent;
    private GameObject lookAtConstraintObject;
    private Vector3 cameraPointToCameraVector;
    private GameObject target;
    public static ObjectPool freeObjectPool;
    private SkeletonSwordSkillNonstopThrust skeletonSwordSkillNonstopThrust;
    private SwordSkillSummonBigSword swordSkillSummonBigSword;
    private SwordSkillThousandSword swordSkillThousandSword;
    private LightingRain lightingRain;
    private FlameRider flameRider;
    private bool botUseSkillWheneverPossibleBool = false;
    private bool canUseSkillBool = false;
    private bool botHumanLikeJumpRandomlyBool = false;
    private bool humanLikeLookableBool = false;
    private bool botHumanLikeSimpleMoveToTargetBool = false;
    private bool botHumanLikeLookAtTargetBool = false;
    private bool botHumanLikeAttackWhenInRangeBool = false;
    private bool humanLikeMovableBool = false;
    private bool canRotateThisFrame;
    private bool isControlByPlayer = false;
    private bool cameraBool = false;
    private bool humanLikeAnimatorBrainBool = false;
    private bool humanLikeJumpableObjectBool = false;
    bool rigidbodyBool = false;
    bool animatorBool = false;
    bool rotatableObjectBool = false;
    bool moveToTargetBool = false;
    bool meleeSimpleAttackWhenNearBool = false;
    bool skillCastOriginPointBool = false;
    public enum CustomMonoBehaviorState {Available, IsUsingSkill}
    private CustomMonoBehaviorState customMonoBehaviorState = CustomMonoBehaviorState.Available;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurentHealth { get => curentHealth; set => curentHealth = value; }
    public string EntityType { get => entityType; set => entityType = value; }
    public List<string> AllyTags { get => allyTags; set => allyTags = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public RotatableObject RotatableObject { get => rotatableObject; set => rotatableObject = value; }
    public bool RigidbodyBool { get => rigidbodyBool; set => rigidbodyBool = value; }
    public bool AnimatorBool { get => animatorBool; set => animatorBool = value; }
    public bool RotatableObjectBool { get => rotatableObjectBool; set => rotatableObjectBool = value; }
    public bool MoveToTargetBool { get => moveToTargetBool; set => moveToTargetBool = value; }
    public bool MeleeSimpleAttackWhenNearBool { get => meleeSimpleAttackWhenNearBool; set => meleeSimpleAttackWhenNearBool = value; }
    public GameObject SkillCastOriginPoint { get => skillCastOriginPoint; set => skillCastOriginPoint = value; }
    public bool SkillCastOriginPointBool { get => skillCastOriginPointBool; set => skillCastOriginPointBool = value; }
    public HumanLikeJumpableObject HumanLikeJumpableObject { get => humanLikeJumpableObject; set => humanLikeJumpableObject = value; }
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
    public HumanLikeLookable HumanLikeLookable { get => humanLikeLookable; set => humanLikeLookable = value; }
    public bool HumanLikeLookableBool { get => humanLikeLookableBool; set => humanLikeLookableBool = value; }
    public bool HumanLikeJumpableObjectBool { get => humanLikeJumpableObjectBool; set => humanLikeJumpableObjectBool = value; }
    public BotHumanLikeJumpRandomly BotHumanLikeJumpRandomly { get => botHumanLikeJumpRandomly; set => botHumanLikeJumpRandomly = value; }
    public bool BotHumanLikeJumpRandomlyBool { get => botHumanLikeJumpRandomlyBool; set => botHumanLikeJumpRandomlyBool = value; }
    public bool CanUseSkillBool { get => canUseSkillBool; set => canUseSkillBool = value; }
    public CanUseSkill CanUseSkill { get => canUseSkill; set => canUseSkill = value; }
    internal SkeletonSwordSkillNonstopThrust SkeletonSwordSkillNonstopThrust { get => skeletonSwordSkillNonstopThrust; set => skeletonSwordSkillNonstopThrust = value; }
    internal SwordSkillSummonBigSword SwordSkillSummonBigSword { get => swordSkillSummonBigSword; set => swordSkillSummonBigSword = value; }
    internal SwordSkillThousandSword SwordSkillThousandSword { get => swordSkillThousandSword; set => swordSkillThousandSword = value; }
    public LightingRain LightingRain { get => lightingRain; set => lightingRain = value; }
    public FlameRider FlameRider { get => flameRider; set => flameRider = value; }
    public CustomMonoBehaviorState CustomMonoBehaviorState1 { get => customMonoBehaviorState; set => customMonoBehaviorState = value; }
    public SkinnedMeshRenderer MainSkinnedMeshRenderer { get => mainSkinnedMeshRenderer; set => mainSkinnedMeshRenderer = value; }
    internal BotUseSkillWheneverPossible BotUseSkillWheneverPossible { get => botUseSkillWheneverPossible; set => botUseSkillWheneverPossible = value; }
    public bool BotUseSkillWheneverPossibleBool { get => botUseSkillWheneverPossibleBool; set => botUseSkillWheneverPossibleBool = value; }

    // Start is called before the first frame update
    public void Awake()
    {
        curentHealth = maxHealth;
        allyTags.Add(gameObject.tag);

        if (TryGetComponent<Rigidbody>(out rigidbody)) rigidbodyBool = true;
        if (TryGetComponent<Animator>(out animator)) animatorBool = true;
        if (TryGetComponent<RotatableObject>(out rotatableObject)) rotatableObjectBool = true;
        if (TryGetComponent<HumanLikeAnimatorBrain>(out humanLikeAnimatorBrain))
        {
            humanLikeAnimatorBrainBool = true;
            FreezeDelegateMethod += () => humanLikeAnimatorBrain.Freeze();
            UnFreezeDelegateMethod += () => humanLikeAnimatorBrain.UnFreeze();
        }
        if (TryGetComponent<HumanLikeMovable>(out humanLikeMovable)) humanLikeMovableBool = true;
        if (TryGetComponent<HumanLikeLookable>(out humanLikeLookable)) humanLikeLookableBool = true;
        if (TryGetComponent<HumanLikeJumpableObject>(out humanLikeJumpableObject)) humanLikeJumpableObjectBool = true;
        if (TryGetComponent<BotHumanLikeSimpleMoveToTarget>(out botHumanLikeSimpleMoveToTarget)) botHumanLikeSimpleMoveToTargetBool = true;
        if (TryGetComponent<BotHumanLikeLookAtTarget>(out botHumanLikeLookAtTarget)) botHumanLikeLookAtTargetBool = true;
        if (TryGetComponent<BotHumanLikeAttackWhenInRange>(out botHumanLikeAttackWhenInRange)) botHumanLikeAttackWhenInRangeBool = true;
        if (TryGetComponent<BotHumanLikeJumpRandomly>(out botHumanLikeJumpRandomly)) botHumanLikeJumpRandomlyBool = true;
        if (TryGetComponent<BotUseSkillWheneverPossible>(out botUseSkillWheneverPossible)) botUseSkillWheneverPossibleBool = true;
        if (TryGetComponent<CanUseSkill>(out canUseSkill)) canUseSkillBool = true;

        if ((skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject) != null) skillCastOriginPointBool = true;

        if (entityType.Equals("Player"))
        {
            camera = Camera.main;
            cameraBool = true;
            isControlByPlayer = true;
        }
        else
        {
            if (botHumanLikeAttackWhenInRangeBool)
            {
                FreezeDelegateMethod += () => botHumanLikeAttackWhenInRange.Freeze();
                UnFreezeDelegateMethod += () => botHumanLikeAttackWhenInRange.UnFreeze();
            }

            if (botHumanLikeJumpRandomlyBool)
            {
                FreezeDelegateMethod += () => botHumanLikeJumpRandomly.Freeze();
                UnFreezeDelegateMethod += () => botHumanLikeJumpRandomly.UnFreeze();
            }

            if (botHumanLikeLookAtTargetBool)
            {
                FreezeDelegateMethod += () => botHumanLikeLookAtTarget.Freeze();
                UnFreezeDelegateMethod += () => botHumanLikeLookAtTarget.UnFreeze();
            }

            if (botHumanLikeSimpleMoveToTargetBool)
            {
                FreezeDelegateMethod += () => botHumanLikeSimpleMoveToTarget.Freeze();
                UnFreezeDelegateMethod += () => botHumanLikeSimpleMoveToTarget.UnFreeze();
            }

            if (botUseSkillWheneverPossibleBool)
            {
                FreezeDelegateMethod += () => botUseSkillWheneverPossible.Freeze();
                UnFreezeDelegateMethod += () => botUseSkillWheneverPossible.UnFreeze();
            }
        }

        cameraPoint = transform.Find("CameraPoint").gameObject;
        lookAtConstraintObjectParent = transform.Find("LookAtConstraintObjectParent").gameObject;
        lookAtConstraintObject = transform.Find("LookAtConstraintObject").gameObject;
        mainSkinnedMeshRenderer = transform.Find("MainMesh").GetComponent<SkinnedMeshRenderer>();

        GameObject freeObjectPrefab = Resources.Load("FreeObject") as GameObject;
        freeObjectPool ??= new ObjectPool(freeObjectPrefab, 20, new PoolArgument(typeof(GameObject), PoolArgument.WhereComponent.Self));

        DeactivateAllSkill();
        FreezeDelegateMethod += () =>
        {
            skeletonSwordSkillNonstopThrust.Freeze();
            lightingRain.Freeze();
            flameRider.Freeze();
        };
        UnFreezeDelegateMethod += () =>
        {
            skeletonSwordSkillNonstopThrust.UnFreeze();
            lightingRain.UnFreeze();
            flameRider.UnFreeze();
        };
    }

    public void DeactivateAllSkill()
    {
        skeletonSwordSkillNonstopThrust = GetComponent<SkeletonSwordSkillNonstopThrust>();
        swordSkillSummonBigSword = GetComponent<SwordSkillSummonBigSword>();
        swordSkillThousandSword = GetComponent<SwordSkillThousandSword>();
        lightingRain = GetComponent<LightingRain>();
        flameRider = GetComponent<FlameRider>();
        skeletonSwordSkillNonstopThrust.enabled = false;
        swordSkillSummonBigSword.enabled = false;
        swordSkillThousandSword.enabled = false;
        lightingRain.enabled = false;
        flameRider.enabled = false;
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

    public Coroutine NullCoroutine()
    {
        return StartCoroutine(NullIenumerator());
    }

    private IEnumerator NullIenumerator()
    {
        yield return new WaitForSeconds(0);
    }

    public void ChangeCustomonobehaviorState(CustomMonoBehaviorState state)
    {
        customMonoBehaviorState = state;
    }

    public void StopCustomonobehaviorState(CustomMonoBehaviorState state)
    {
        customMonoBehaviorState = CustomMonoBehaviorState.Available;
    }

    public delegate void FreezeDelegate();
    public FreezeDelegate FreezeDelegateMethod;
    public void Freeze()
    {
        FreezeDelegateMethod?.Invoke();
    }

    public delegate void UnFreezeDelegate();
    public UnFreezeDelegate UnFreezeDelegateMethod;
    public void UnFreeze()
    {
        UnFreezeDelegateMethod?.Invoke();
    }
}