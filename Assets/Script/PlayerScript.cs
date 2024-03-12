using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(SkillableObject), typeof(RotatableObject), typeof(TargetableObject))]
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
    }

    void Attack(InputAction.CallbackContext callbackContext)
    {
        // Vector3 rotateDirection = targetableObject.nearestTarget.transform.position - transform.position;
        // skillableObject.PerformAttack(targetableObject.nearestTarget.transform, rotateDirection);
        // if (UnityRandom.Range(0, 2) == 0) animator.SetBool("Attack_Mirror", true);
        // else animator.SetBool("Attack_Mirror", false);
        // targetableObject.Target();
        // animator.SetBool("Attack", true);
    }

    public void StopAttack()
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
