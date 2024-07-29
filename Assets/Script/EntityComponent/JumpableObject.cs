using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpableObject : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    [SerializeField] private bool jumpable = false;
    [SerializeField] private bool justJumped = false;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int currentJumpCount = 0;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool checkJump = true;
    [SerializeField] private float checkJumpDelay = 0.1f;

    public CustomMonoBehavior CustomMonoBehavior { get => customMonoBehavior; set => customMonoBehavior = value; }
    public bool Jumpable { get => jumpable; set => jumpable = value; }
    public bool JustJumped { get => justJumped; set => justJumped = value; }
    public int MaxJumpCount { get => maxJumpCount; set => maxJumpCount = value; }
    public int CurrentJumpCount { get => currentJumpCount; set => currentJumpCount = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public bool CheckJump1 { get => checkJump; set => checkJump = value; }
    public float CheckJumpDelay { get => checkJumpDelay; set => checkJumpDelay = value; }

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();    
    }

    private void Jump()
    {
        if (jumpable)
        {
            customMonoBehavior.Rigidbody.velocity = Vector3.zero;
            customMonoBehavior.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            justJumped = true;
            if (++currentJumpCount == maxJumpCount)
            {
                jumpable = false;
            }
        }
    }

    private void CheckJump()
    {
        if (justJumped)
        {
            justJumped = false;
            checkJump = false;
            StopAllCoroutines();
            StartCoroutine(CheckJumpCoroutine());

            return;
        }

        if (checkJump)
        {
            if (CustomMonoBehavior.FeetChecker.IsTouching)
            {
                jumpable = true;
                currentJumpCount = 0;
            }
        }
    }

    public IEnumerator CheckJumpCoroutine()
    {
        yield return new WaitForSeconds(checkJumpDelay);

        checkJump = true;
    }

    void FixedUpdate()
    {
        CheckJump();
        CustomMonoBehavior.Animator.SetFloat("VerticalSpeed", customMonoBehavior.Rigidbody.velocity.y);
    }
}
