using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //PlayerInput playerInput;
    public InputAction move;
    public float moveSpeed = 0.1f;
    public float jumpVelocity = 1f;
    private new Rigidbody rigidbody;
    public Vector2 mouse;
    public Vector2 prevMouse = Vector2.zero;
    public Vector2 mouseMoved;
    public float mouseSpeed = 5f;
    public GameObject cameraOfPlayer;
    public Vector2 screenResolution;
    PlayerInputSystem playerInputSystem;
    public Vector2 zoom;
    public float zoomAmount = 0.5f;
    private UtilObject utilObject = new UtilObject();
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        cameraOfPlayer = GameObject.Find("Main Camera");
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        rigidbody = GetComponent<Rigidbody>();
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        View();
    }

    private void FixedUpdate() 
    {
        Zoom();
    }

    public float rotateAmountAbs = 3f;
    public float rotateAmount;
    public float curentAngle = 0;
    public float toAngle;
    public float absToAngle;
    public float prevToAngle = 0;
    public enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    public directionEnum rotateDirection;
    public Vector3 directionVector;
    public float rotationTempValue = 0;
    public float moveAngle;
    public float minMoveAngle;
    public float movedAngle = 0;
    public Vector2 moveVector;
    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        moveVector = playerInputSystem.Control.Move.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveVector.magnitude);

        if (moveVector != Vector2.zero)
        {
            directionVector = new Vector3(moveVector.x, 0, moveVector.y);
            transform.position +=  directionVector * moveSpeed;

            toAngle = Vector3.SignedAngle(Vector3.forward, directionVector, Vector3.up);
            absToAngle = Math.Abs(toAngle);
            if (toAngle != prevToAngle)
            {
                moveAngle = Math.Abs(toAngle - curentAngle);
                rotateDirection = toAngle >= curentAngle ? directionEnum.Clockwise : directionEnum.CounterClockwise;
                minMoveAngle = 360 - moveAngle;
                if (minMoveAngle < moveAngle)
                {
                    rotateDirection = rotateDirection == directionEnum.Clockwise ? directionEnum.CounterClockwise : directionEnum.Clockwise;
                    moveAngle = minMoveAngle;
                }
                rotateAmount = (int)rotateDirection * rotateAmountAbs;
                movedAngle = 0;
            }
            prevToAngle = toAngle;

            if (movedAngle < moveAngle)
            {
                transform.Rotate(new Vector3(0, rotateAmount, 0));
                curentAngle += rotateAmount;
                movedAngle += rotateAmountAbs;
            }
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("this");
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }
    public void View()
    {
        mouse = playerInputSystem.Control.View.ReadValue<Vector2>();
        mouse = new Vector2(mouse.x/screenResolution.x, mouse.y/screenResolution.y);
        mouseMoved = mouse - prevMouse; mouseMoved *= mouseSpeed;
        // cameraOfPlayer.transform.rotation = Quaternion.Euler(new Vector3(cameraOfPlayer.transform.rotation.eulerAngles.x - mouseMoved.y
        // , cameraOfPlayer.transform.rotation.eulerAngles.y + mouseMoved.x, 0));
        utilObject.RotateByAmount(cameraOfPlayer.transform, -mouseMoved.y, mouseMoved.x);
        prevMouse = mouse;
    }

    float tp = 0; float tl = 1f;
    float i = 1;
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
