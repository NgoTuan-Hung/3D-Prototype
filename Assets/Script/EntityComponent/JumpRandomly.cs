using System.Collections;
using UnityEngine;

public class JumpRandomly : MonoBehaviour 
{
    private CustomMonoBehavior customMonoBehavior;
    private float jumpChancePerFrame = 0.05f;

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();    
    }

    private void FixedUpdate() 
    {
        if (Random.value < jumpChancePerFrame)
        {
            if (customMonoBehavior.JumpableObject.Jumpable)
            {
                customMonoBehavior.Animator.SetBool("Jump", true);
            }
        }
    }
}