using System.Collections;
using UnityEngine;

public class JumpableObject : MonoBehaviour
{
    public CustomMonoBehavior customMonoBehavior;
    public float jumpForce = 5f;
    public float jumpCount = 2;
    public float currentJumpCount = 0;
    public float jumpDelay = 0.2f;
    public bool canJump = true;

    void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    void FixedUpdate()
    {
        
        if (canJump)
        {
            if (!customMonoBehavior.HumanLikeAnimatorBrain.onAir) currentJumpCount = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                if (currentJumpCount < jumpCount)
                {
                    customMonoBehavior.Rigidbody.velocity = new Vector3(customMonoBehavior.Rigidbody.velocity.x, 0, customMonoBehavior.Rigidbody.velocity.z);
                    customMonoBehavior.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    customMonoBehavior.HumanLikeAnimatorBrain.ChangeState(State.Jump);
                    currentJumpCount++;
                    StartCoroutine(JumpDelay());
                    Debug.Log("Jump");
                }
            }
        }
    }

    IEnumerator JumpDelay()
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