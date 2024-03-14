using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator animator;
    new ParticleSystem particleSystem;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        particleSystem = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    public void PlayAttackParticleSystem()
    {
        if (!particleSystem.isPlaying) particleSystem.Play();
    }

    public void Attack()
    {
        animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.gameObject.SetActive(false);
    }
}
