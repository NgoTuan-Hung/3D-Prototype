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
    public Vector2 screenResolution;
    PlayerInputSystem playerInputSystem;
    // Start is called before the first frame update
    void Awake()
    {
        //playerInput = gameObject.GetComponent<PlayerInput>();
        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Control.Jump.performed += Jump;
        //move = playerInput.actions.FindAction("Move");
        rigidbody = GetComponent<Rigidbody>();
        screenResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        View();
    }

    public void Move()
    {
        // var moveVector = move.ReadValue<Vector2>();
        var moveVector = playerInputSystem.Control.Move.ReadValue<Vector2>();
        transform.position += new Vector3(moveVector.x, 0, moveVector.y) * moveSpeed;
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        // callbackContext.
        rigidbody.velocity += new Vector3(0, jumpVelocity);
    }

    public void View()
    {
        mouse = playerInputSystem.Control.View.ReadValue<Vector2>();
        mouse = new Vector2(mouse.x/screenResolution.x, mouse.y/screenResolution.y);
        // transform.rotation = Quaternion.Euler()
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
