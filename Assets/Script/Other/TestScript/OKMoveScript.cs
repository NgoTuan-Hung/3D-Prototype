using UnityEngine;

public class OKMoveScript : MonoBehaviour 
{
    private CustomMonoBehavior customMonoBehavior;

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }    

    public float moveHorizontal;
    public float moveVertical;
    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.LeftShift)) customMonoBehavior.HumanLikeMovable.Run(new Vector3(moveHorizontal, 0.0f, moveVertical));
            else customMonoBehavior.HumanLikeMovable.Move(new Vector3(moveHorizontal, 0.0f, moveVertical));
        }
        else {customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Walk); customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Run);}
    }
}