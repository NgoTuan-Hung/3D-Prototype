using System.Collections;
using UnityEngine;

public class HumanLikeJumpableObject : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private float jumpForce = 5f;
    private float jumpCount = 2;
    private float currentJumpCount = 0;
    private float jumpDelay = 0.2f;
    private bool canJump = true;

    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public float JumpCount { get => jumpCount; set => jumpCount = value; }
    public float CurrentJumpCount { get => currentJumpCount; set => currentJumpCount = value; }
    public float JumpDelay { get => jumpDelay; set => jumpDelay = value; }
    public bool CanJump { get => canJump; set => canJump = value; }

    void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    void FixedUpdate()
    {
        if (!customMonoBehavior.HumanLikeAnimatorBrain.onAir) currentJumpCount = 0;
    }

    public void Jump()
    {
        if (canJump)
        {
            if (currentJumpCount < jumpCount)
            {
                customMonoBehavior.Rigidbody.velocity = new Vector3(customMonoBehavior.Rigidbody.velocity.x, 0, customMonoBehavior.Rigidbody.velocity.z);
                customMonoBehavior.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Jump);
                currentJumpCount++;
                StartCoroutine(JumpDelayIEnumerator());
            }
        }
    }

    IEnumerator JumpDelayIEnumerator()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }

    public void StopJump()
    {
        customMonoBehavior.HumanLikeAnimatorBrain.StopState(State.Jump);
    }
}