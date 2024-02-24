using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    public static ObjectPool weaponPool;
    public GameObject swordPrefab;
    public bool canAttack = true;
    public float attackCooldown = 0;

    private void Start() 
    {
        weaponPool = new ObjectPool(swordPrefab, 20);
    }

    public void Attack(Transform location)
    {
        if (canAttack)
        {
            canAttack = false;
            GameObject blackSword = weaponPool.PickOne();
            ParticleSystem particleSystem = blackSword.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
            if (!particleSystem.isPlaying) particleSystem.Play();
            blackSword.transform.position = location.position;
            blackSword.transform.rotation = location.rotation;
            blackSword.GetComponentInChildren<Animator>().SetBool("Attack", true);
            StartCoroutine(ResetAttack());
        }
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}
