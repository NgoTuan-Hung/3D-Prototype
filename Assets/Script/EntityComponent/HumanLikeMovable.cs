using Unity.Mathematics;
using UnityEngine;

public class HumanLikeMovable : MonoBehaviour 
{
    private CustomMonoBehavior customMonoBehavior;
    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        moveSpeedPerFrame *= Time.fixedDeltaTime;
        runSpeedPerFrame *= Time.fixedDeltaTime;
    }

    public float moveSpeed = 5f;
    public float moveSpeedPerFrame = 5;
    public float runSpeedPerFrame = 10;
    private void FixedUpdate() 
    {
        Move();
    }
    public float moveHorizontal;
    public float moveVertical;
    public Vector3 movement;
    public Vector3 rotation;
    private void Move()
    {
        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        // {
        //     moveHorizontal = Input.GetAxis("Horizontal");
        //     moveVertical = Input.GetAxis("Vertical");
        //     movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        //     rotation = movement.z < 0 ? -movement : movement;
        //     myGameObject.animator.SetFloat("MoveDirZ", movement.z);
        //     movement = myGameObject.cameraPoint.transform.TransformDirection(movement);
        //     rotation = myGameObject.cameraPoint.transform.TransformDirection(rotation);
        //     movement.y = 0;
        //     tempRotation = Quaternion.LookRotation(rotation);
        //     if (myGameObject.canRotateThisFrame) transform.rotation = tempRotation;
        //     if (Input.GetKey(KeyCode.LeftShift))
        //     {
        //         myGameObject.humanLikeAnimatorBrain.ChangeState(State.Run);
        //         transform.position += movement * runSpeedPerFrame;
        //     }
        //     else
        //     {
        //         myGameObject.humanLikeAnimatorBrain.ChangeState(State.Walk);
        //         transform.position += movement * moveSpeedPerFrame;
        //     }
        // }
        // else {myGameObject.humanLikeAnimatorBrain.StopState(State.Walk); myGameObject.humanLikeAnimatorBrain.StopState(State.Run);}
    }

    public void Move(Vector3 movement)
    {
        movement = movement.normalized;
        rotation = movement.z < 0 ? -movement : movement;
        customMonoBehavior.Animator.SetFloat("MoveDirZ", movement.z);
        this.movement = customMonoBehavior.CameraPoint.transform.TransformDirection(movement);
        rotation = customMonoBehavior.CameraPoint.transform.TransformDirection(rotation);
        this.movement.y = 0;
        if (customMonoBehavior.CanRotateThisFrame) transform.rotation = Quaternion.LookRotation(rotation);
        customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Walk);
        transform.position += this.movement * moveSpeedPerFrame;
    }

    public void Run(Vector3 movement)
    {
        movement = movement.normalized;
        rotation = movement.z < 0 ? -movement : movement;
        customMonoBehavior.Animator.SetFloat("MoveDirZ", movement.z);
        this.movement = customMonoBehavior.CameraPoint.transform.TransformDirection(movement);
        rotation = customMonoBehavior.CameraPoint.transform.TransformDirection(rotation);
        this.movement.y = 0;
        if (customMonoBehavior.CanRotateThisFrame) transform.rotation = Quaternion.LookRotation(rotation);
        customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Run);
        transform.position += this.movement * runSpeedPerFrame;
    }

    public void Stop()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Walk);
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Run);
    }
}