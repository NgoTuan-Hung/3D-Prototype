using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator animator;

    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        transform.parent.gameObject.SetActive(false);
    }
}
