
using UnityEngine;

public class PlayerHumanlikeJumpScript : ExtensibleMonobehavior
{
    private CustomMonoBehavior customMonoBehavior;

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void FixedUpdate()
    {
        if (Freeze1) return;
        Jump();
    }

    private void Jump()
    {
        if (customMonoBehavior.PlayerInputSystem.Control.Jump.IsPressed())
        {
            customMonoBehavior.HumanLikeJumpableObject.Jump();
        }
    }
}