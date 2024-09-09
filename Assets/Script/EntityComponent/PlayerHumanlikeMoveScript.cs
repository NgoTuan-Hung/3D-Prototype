using System.ComponentModel.Design;
using UnityEngine;

public class PlayerHumanlikeMoveScript : ExtensibleMonobehavior
{
    private CustomMonoBehavior customMonoBehavior;

    private void Start() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    } 

    private void FixedUpdate() 
    {
        if (Freeze1) return;
        Move();
    }

    private Vector2 moveVector;
    public void Move()
    {
        moveVector = customMonoBehavior.PlayerInputSystem.Control.Move.ReadValue<Vector2>();

        /* Check if we pressed any move button this frame:
        - If any move key is pressed we also check if we pressed the run button, if we did we call the Run method
        (which handle animation/logic for run), otherwise we call the Move method.
        - If we didn't press any move key this frame, we stop the walk and run state */
        if (moveVector != Vector2.zero)
        {
            if (customMonoBehavior.PlayerInputSystem.Control.Run.IsPressed()) customMonoBehavior.HumanLikeMovable.Run(new Vector3(moveVector.x, 0.0f, moveVector.y));
            else customMonoBehavior.HumanLikeMovable.Move(new Vector3(moveVector.x, 0.0f, moveVector.y));
        }
        else {customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Walk); customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Run);}
    }
}