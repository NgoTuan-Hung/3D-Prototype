using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(SkillableObject), typeof(RotatableObject), typeof(TargetableObject))]
[RequireComponent(typeof(CombatEntity))]
public class PlayerScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float jumpVelocity = 1f;
    private new Rigidbody rigidbody;
    [SerializeField] private GameObject cameraOfPlayer;
    PlayerInputSystem playerInputSystem;
    private Animator animator;
    [SerializeField] private Vector3 directionVector;
    [SerializeField] private Vector2 moveVector;
    private RotatableObject rotatableObject;
    // [SerializeField] private GameObject viewRotationPoint;
    private TargetableObject targetableObject;
    MultiAimConstraint multiAimConstraint;
    public MultiAimConstraintData multiAimConstraintData;
    public RigBuilder rigBuilder;
    public delegate void CameraDelegate(GameObject target);
    public static CameraDelegate cameraDelegate;
    [SerializeField] private Transform attackPosition;
    public SkillableObject skillableObject;
    UtilObject utilObject = new UtilObject();
    // Start is called before the first frame update
    void Awake()
    {
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        playerInputSystem.Control.Attack.performed += Attack;
        playerInputSystem.Control._1.performed += ClickOne;
        //MultiAimConstraintData multiAimConstraint = GetComponentInChildren<MultiAimConstraintData>();
    }

    private void Start() 
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        rotatableObject = GetComponent<RotatableObject>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        targetableObject = GetComponent<TargetableObject>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        multiAimConstraintData = multiAimConstraint.data;
        rigBuilder = GetComponentInChildren<RigBuilder>();
        skillableObject = GetComponent<SkillableObject>();
        attackPosition = GameObject.Find("AttackPosition").transform;
        attackCoroutine = StartCoroutine(NullCoroutine());
        clickOneCoroutine = StartCoroutine(NullCoroutine());
        skillCastOriginPoint = transform.Find("SkillCastOriginPoint").gameObject;
    }

    public bool isClickOne = false;
    public Coroutine clickOneCoroutine;
    public GameObject skillCastOriginPoint;
    public void ClickOne(InputAction.CallbackContext callbackContext)
    {
        if (!isClickOne)
        {
            StartCoroutine(HandleClickOne());
        } else isClickOne = false;
    }

    public Vector2 skillCastVector;
    public GameObject skillCast;
    public float skillCastAngle;
    public IEnumerator HandleClickOne()
    {
        isClickOne = true;
        while (isClickOne)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            skillCastVector = (playerInputSystem.Control.View.ReadValue<Vector2>() - (Vector2)Camera.main.WorldToScreenPoint(skillCastOriginPoint.transform.position)).normalized;
            skillCast.transform.position = transform.position;
            skillCast.SetActive(true);
            skillCastAngle = Vector2.SignedAngle(Vector2.up, skillCastVector);
            skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        }

        skillCast.SetActive(false);
    }

    private Coroutine attackCoroutine;
    private IEnumerator NullCoroutine() {yield return new WaitForSeconds(0);}
    [SerializeField] private bool isAttack = false;
    void Attack(InputAction.CallbackContext callbackContext)
    {
        if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(AttackHandler());
        }
        else isAttack = false;
    }

    IEnumerator AttackHandler()
    {
        while (isAttack)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            if (skillableObject.CanAttack)
            {
                Vector3 rotateDirection = targetableObject.TargetChecker.nearestTarget.transform.position - transform.position;
                skillableObject.PerformAttack(targetableObject.TargetChecker.nearestTarget.transform, rotateDirection);
                if (UnityRandom.Range(0, 2) == 0) animator.SetBool("Attack_Mirror", true);
                else animator.SetBool("Attack_Mirror", false);
                if (!targetableObject.IsTarget)
                {
                    targetableObject.Target();
                }
                animator.SetBool("Attack", true);
                animator.Play("UpperBody.Attack", 1, 0);
                StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(StopAttack());
            }
        }
    }


    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(3);

        targetableObject.Reset();
    }

    public void StopAttackAnimation()
    {
        animator.SetBool("Attack", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        Move();
    }

    [SerializeField] private bool canMove = true;
    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        moveVector = playerInputSystem.Control.Move.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveVector.magnitude);
        animator.SetFloat("MoveVectorX", moveVector.x);
        animator.SetFloat("MoveVectorY", moveVector.y);

        if (moveVector != Vector2.zero && canMove)
        {
            directionVector = new Vector3(moveVector.x, 0, moveVector.y);
            
            transform.position +=  directionVector * moveSpeed;
            
            rotatableObject.RotateToDirectionAxisXZ(directionVector);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("this");
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }


    public IEnumerator test(object value)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(value);
    }

    void OnEnable()
    {
        playerInputSystem.Control.Enable();
    }

    void OnDisable()
    {
        playerInputSystem.Control.Disable();
    }
}

// public bool isViewDirection = false;
    // public Vector3 viewDirection;
    // public IEnumerator ViewDirectionHandler(InputAction viewDirection)
    // {
    //     isViewDirection = true;
    //     animator.SetBool("ViewDirection", isViewDirection);
    //     while (viewDirection.IsPressed())
    //     {
    //         yield return new WaitForSeconds(Time.fixedDeltaTime);

    //         var rawPose = Camera.main.WorldToScreenPoint(viewRotationPoint.transform.position);
    //         rawPose.Scale(new Vector3(1 / GlobalObject.Instance.screenResolution.x, 1 / GlobalObject.Instance.screenResolution.y, 1f));
    //         this.viewDirection = new Vector3(GlobalObject.Instance.mouse.x - rawPose.x, 0, GlobalObject.Instance.mouse.y - rawPose.y);
            
    //         rotatableObject.RotateToDirectionAxisXZ(this.viewDirection);
    //     }
    //     isViewDirection = false;
    //     animator.SetBool("ViewDirection", isViewDirection);
    // }
