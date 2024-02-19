using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float jumpVelocity = 1f;
    private new Rigidbody rigidbody;
    [SerializeField] private GameObject cameraOfPlayer;
    PlayerInputSystem playerInputSystem;
    private UtilObject utilObject = new UtilObject();
    private Animator animator;
    [SerializeField] private Vector3 directionVector;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private RotatableObject rotatableObject;
    [SerializeField] private GameObject viewRotationPoint;
    [SerializeField] private bool strafeHorizontal;
    private GlobalObject globalObject;
    private TargetableObject targetableObject;
    MultiAimConstraint multiAimConstraint;
    public MultiAimConstraintData multiAimConstraintData;
    public RigBuilder rigBuilder;
    public delegate void CameraDelegate(GameObject target);
    public static CameraDelegate cameraDelegate;
    // Start is called before the first frame update
    void Awake()
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        playerInputSystem.Control.ViewDirection.performed += ViewDirection;
        playerInputSystem.Control.Target.performed += Target;
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rotatableObject = new RotatableObject(transform);
        globalObject = GlobalObject.Instance;
        targetableObject = GetComponentInChildren<TargetableObject>();
        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        multiAimConstraintData = multiAimConstraint.data;
        rigBuilder = GetComponentInChildren<RigBuilder>();
        //MultiAimConstraintData multiAimConstraint = GetComponentInChildren<MultiAimConstraintData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        Move();
    }
    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        moveVector = playerInputSystem.Control.Move.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveVector.magnitude);
        animator.SetFloat("MoveVectorX", moveVector.x);
        animator.SetFloat("MoveVectorY", moveVector.y);

        if (moveVector != Vector2.zero)
        {
            if (isTarget)
            {
                if (moveVector == Vector2.up) moveVector = new Vector2(targetableObject.nearestTarget.transform.position.x - transform.position.x, 
                targetableObject.nearestTarget.transform.position.y - transform.position.y).normalized;
            }
            directionVector = new Vector3(moveVector.x, 0, moveVector.y);
            
            transform.position +=  directionVector * moveSpeed;
            
            if (!isViewDirection) rotatableObject.RotateToDirection(utilObject, directionVector);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("this");
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }

    [SerializeField] private bool isTarget = false;
    public void Target(InputAction.CallbackContext callbackContext)
    {
        if (!isTarget)
        {
            isTarget = true;
            //animator.SetBool("Target", true);
            StartCoroutine(TargetHandler());
        }
        else
        {
            isTarget = false;
            cameraDelegate?.Invoke(null);
            //animator.SetBool("Target", false);
        }
    }

    [SerializeField] private Vector3 tH_directionVector = Vector3.zero;
    public IEnumerator TargetHandler()
    {
        // discard c#
        var nearestTarget = targetableObject.nearestTarget;
        cameraDelegate?.Invoke(nearestTarget);
        
        var weightedTransformArray = multiAimConstraintData.sourceObjects;
        weightedTransformArray.SetTransform(0, nearestTarget.transform);
        multiAimConstraintData.sourceObjects = weightedTransformArray;
        multiAimConstraint.data = multiAimConstraintData;
        rigBuilder.Build();
        // while (isTarget)
        // {
        //     yield return new WaitForSeconds(Time.fixedDeltaTime);

        //     tH_directionVector.x = nearestTarget.transform.position.x - transform.position.x;
        //     tH_directionVector.z = nearestTarget.transform.position.z - transform.position.z;
        //     rotatableObject.RotateToDirection(utilObject, tH_directionVector);
        // }
        yield return new WaitForSeconds(0);
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
            
            rotatableObject.RotateToDirection(utilObject, this.viewDirection);
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
