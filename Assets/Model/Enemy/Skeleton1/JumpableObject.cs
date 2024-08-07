using System.Collections;
using UnityEngine;

public class JumpableObject : MonoBehaviour
{
    public MyGameObject1 myGameObject;
    public float jumpForce = 5f;
    public float jumpCount = 2;
    public float currentJumpCount = 0;
    public float jumpDelay = 0.2f;
    public bool canJump = true;

    void Awake()
    {
        myGameObject = GetComponent<MyGameObject1>();
    }

    void FixedUpdate()
    {
        
        if (canJump)
        {
            if (!myGameObject.humanLikeAnimatorBrain.onAir) currentJumpCount = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                if (currentJumpCount < jumpCount)
                {
                    myGameObject.rigidbody.velocity = new Vector3(myGameObject.rigidbody.velocity.x, 0, myGameObject.rigidbody.velocity.z);
                    myGameObject.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    myGameObject.humanLikeAnimatorBrain.ChangeState(State.Jump);
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
        myGameObject.humanLikeAnimatorBrain.StopState(State.Jump);
    }
}