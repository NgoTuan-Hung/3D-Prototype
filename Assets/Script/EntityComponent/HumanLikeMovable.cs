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
    private float moveSpeedPerFrame = 5 * Time.fixedDeltaTime;
    private float runSpeedPerFrame = 10 * Time.fixedDeltaTime;
    private float defaultMoveSpeedPerFrame = 5 * Time.fixedDeltaTime;
    private float defaultRunSpeedPerFrame = 10 * Time.fixedDeltaTime;
    private Vector3 movement;
    private Vector3 rotation;

    public float MoveSpeedPerFrame { get => moveSpeedPerFrame; set => moveSpeedPerFrame = value; }
    public float RunSpeedPerFrame { get => runSpeedPerFrame; set => runSpeedPerFrame = value; }
    public float DefaultMoveSpeedPerFrame { get => defaultMoveSpeedPerFrame; set => defaultMoveSpeedPerFrame = value; }
    public float DefaultRunSpeedPerFrame { get => defaultRunSpeedPerFrame; set => defaultRunSpeedPerFrame = value; }

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