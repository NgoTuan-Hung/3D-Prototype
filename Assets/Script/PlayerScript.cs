using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

[RequireComponent(typeof(SkillableObject))]
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
    [SerializeField] private RotatableObject rotatableObject;
    [SerializeField] private GameObject viewRotationPoint;
    [SerializeField] private bool strafeHorizontal;
    private TargetableObject targetableObject;
    MultiAimConstraint multiAimConstraint;
    public MultiAimConstraintData multiAimConstraintData;
    public RigBuilder rigBuilder;
    public delegate void CameraDelegate(GameObject target);
    public static CameraDelegate cameraDelegate;
    [SerializeField] private Transform attackPosition;
    public SkillableObject skillableObject;
    [SerializeField] private GameObject bodyRotationSourceObject;
    UtilObject utilObject = new UtilObject();
    // Start is called before the first frame update
    void Awake()
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        playerInputSystem.Control.ViewDirection.performed += ViewDirection;
        playerInputSystem.Control.Target.performed += Target;
        playerInputSystem.Control.Attack.performed += Attack;
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rotatableObject = new RotatableObject(transform);
        targetableObject = GetComponentInChildren<TargetableObject>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        multiAimConstraintData = multiAimConstraint.data;
        rigBuilder = GetComponentInChildren<RigBuilder>();
        skillableObject = GetComponent<SkillableObject>();
        //MultiAimConstraintData multiAimConstraint = GetComponentInChildren<MultiAimConstraintData>();
    }
    void Attack(InputAction.CallbackContext callbackContext)
    {
        skillableObject.PerformAttack(attackPosition);
        if (UnityRandom.Range(0, 2) == 0) animator.SetBool("Attack_Mirror", true);
        else animator.SetBool("Attack_Mirror", false);
        animator.SetBool("Attack", true);
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
            // if (isTarget)
            // {
            //     if (moveVector == Vector2.up) moveVector = new Vector2(currentTarget.transform.position.x - transform.position.x, 
            //     currentTarget.transform.position.z - transform.position.z).normalized;            
            // }
            directionVector = new Vector3(moveVector.x, 0, moveVector.y);
            
            transform.position +=  directionVector * moveSpeed;
            
            if (!isTarget) bodyRotationSourceObject.transform.rotation = Quaternion.Euler(0, rotatableObject.CurentAngle, 0);
            if (!isViewDirection) rotatableObject.RotateToDirectionAxisXZ(directionVector);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("this");
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }

    [SerializeField] private bool isTarget = false;
    [SerializeField] private GameObject currentTarget;
    private Vector3 targetTempVec;
    public void Target(InputAction.CallbackContext callbackContext)
    {
        if (!isTarget)
        {
            isTarget = true;
            //animator.SetBool("Target", true);
            currentTarget = targetableObject.nearestTarget;
            //cameraDelegate?.Invoke(currentTarget);
            StartCoroutine(TargetHandler());
            
            var weightedTransformArray = multiAimConstraintData.sourceObjects;
            weightedTransformArray.SetTransform(0, currentTarget.transform);
            multiAimConstraintData.sourceObjects = weightedTransformArray;
            multiAimConstraint.data = multiAimConstraintData;
            rigBuilder.Build();
        }
        else
        {
            isTarget = false;
            //cameraDelegate?.Invoke(null);
            //animator.SetBool("Target", false);
        }
    }

    public IEnumerator TargetHandler()
    {
        while (isTarget)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            bodyRotationSourceObject.transform.position = transform.position;
            var direction = new Vector2(currentTarget.transform.position.x - transform.position.x,
            currentTarget.transform.position.z - transform.position.z);
            var angle = utilObject.CalculateAngle(Vector3.forward, new Vector3(direction.x, 0, direction.y), Vector3.up);
            object[][] returnValue = rotatableObject.GetOptimalRotateDirectionAndMoveAngle(angle);
            bodyRotationSourceObject.transform.rotation = Quaternion.Euler(0
            , rotatableObject.CurentAngle + Math.Clamp((int)returnValue[0][0] * (float)returnValue[0][1], -90, 90), 0);
        }
    }

    public void ViewDirection(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine(ViewDirectionHandler(callbackContext.action));
    }

    public bool isViewDirection = false;
    public Vector3 viewDirection;
    public IEnumerator ViewDirectionHandler(InputAction viewDirection)
    {
        isViewDirection = true;
        animator.SetBool("ViewDirection", isViewDirection);
        while (viewDirection.IsPressed())
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            var rawPose = Camera.main.WorldToScreenPoint(viewRotationPoint.transform.position);
            rawPose.Scale(new Vector3(1 / GlobalObject.Instance.screenResolution.x, 1 / GlobalObject.Instance.screenResolution.y, 1f));
            this.viewDirection = new Vector3(GlobalObject.Instance.mouse.x - rawPose.x, 0, GlobalObject.Instance.mouse.y - rawPose.y);
            
            rotatableObject.RotateToDirectionAxisXZ(this.viewDirection);
        }
        isViewDirection = false;
        animator.SetBool("ViewDirection", isViewDirection);
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
