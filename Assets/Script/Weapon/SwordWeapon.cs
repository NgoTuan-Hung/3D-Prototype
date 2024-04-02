using System.Collections;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private void Awake() 
    {
        StartParent();    
    }

    private void OnCollisionEnter(Collision other) 
    {
        OnCollisionEnterParent(other);
    }

    public void SummonBigSword()
    {
        animator.SetBool("BigSword", true);
    }

    public void StopSummon()
    {
        animator.SetBool("BigSword", false);
        StartCoroutine(StopSummonHandler());
    }

    public IEnumerator StopSummonHandler()
    {
        yield return new WaitForSeconds(1);

        transform.parent.gameObject.SetActive(false);
    }
}