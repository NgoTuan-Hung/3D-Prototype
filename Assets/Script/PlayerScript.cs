using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private InputAction move;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float jumpVelocity = 1f;
    private new Rigidbody rigidbody;
    [SerializeField] private Vector2 mouse;
    [SerializeField] private Vector2 prevMouse = Vector2.zero;
    [SerializeField] private Vector2 mouseMoved;
    [SerializeField] private float mouseSpeed = 5f;
    [SerializeField] private GameObject cameraOfPlayer;
    [SerializeField] private Vector2 screenResolution;
    PlayerInputSystem playerInputSystem;
    [SerializeField] private Vector2 zoom;
    [SerializeField] private float zoomAmount = 0.5f;
    private UtilObject utilObject = new UtilObject();
    private Animator animator;
    [SerializeField] private Vector3 directionVector;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private RotatableObject rotatableObject;
    [SerializeField] private GameObject viewRotationPoint;

    // Start is called before the first frame update
    void Awake()
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        playerInputSystem.Control.ViewDirection.performed += ViewDirection;
        rigidbody = GetComponent<Rigidbody>();
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        animator = GetComponent<Animator>();
        rotatableObject = new RotatableObject(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        Move();
        View();
        Zoom();
    }
    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        moveVector = playerInputSystem.Control.Move.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveVector.magnitude);


        if (moveVector != Vector2.zero)
        {
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

    public Vector2 rawMouse;
    public void View()
    {
        rawMouse = playerInputSystem.Control.View.ReadValue<Vector2>();
        mouse = new Vector2(rawMouse.x/screenResolution.x, rawMouse.y/screenResolution.y);
        mouseMoved = mouse - prevMouse; mouseMoved *= mouseSpeed;
        // cameraOfPlayer.transform.rotation = Quaternion.Euler(new Vector3(cameraOfPlayer.transform.rotation.eulerAngles.x - mouseMoved.y
        // , cameraOfPlayer.transform.rotation.eulerAngles.y + mouseMoved.x, 0));
        utilObject.RotateByAmount(cameraOfPlayer.transform, -mouseMoved.y, mouseMoved.x);
        prevMouse = mouse;
    }
    public void Zoom()
    {
        zoom = playerInputSystem.Control.Zoom.ReadValue<Vector2>();
        // var current = cameraOfPlayer.transform;
        // utilObject.BackWardByAmount(cameraOfPlayer.transform, new Vector3(0, 0, zoom.y > 0 ? -zoomAmount : (zoom.y < 0 ? zoomAmount : 0)));
        // if (current != cameraOfPlayer.transform) Debug.Log("OK");
        var z = zoom.y > 0 ? zoomAmount : (zoom.y < 0 ? -zoomAmount : 0);
        //var temp = cameraOfPlayer.transform.TransformPoint(0, 0, z);
        //cameraOfPlayer.transform.position = cameraOfPlayer.transform.TransformPoint(-1, 0, 0);
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
        while (viewDirection.IsPressed())
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            var rawPose = Camera.main.WorldToScreenPoint(viewRotationPoint.transform.position);
            rawPose.Scale(new Vector3(1 / screenResolution.x, 1 / screenResolution.y, 1f));
            this.viewDirection = new Vector3(mouse.x - rawPose.x, 0, mouse.y - rawPose.y);
            
            rotatableObject.RotateToDirection(utilObject, this.viewDirection);
        }
        isViewDirection = false;
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
