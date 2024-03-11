using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSkill : MonoBehaviour, WeaponSkill
{
    public static ObjectPool weaponPool;
    public GameObject swordPrefab;
    public bool canAttack = true;
    public float attackCooldown = 0;

    private void Start() 
    {
        swordPrefab = Instantiate(Resources.Load("LongSword")).GameObject();
        weaponPool = new ObjectPool(swordPrefab, 20);
    }

    public void Attack(Transform target, Vector3 rotationDirection)
    {
        if (canAttack)
        {
            canAttack = false;
            GameObject blackSword = weaponPool.PickOne();
            ParticleSystem particleSystem = blackSword.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
            if (!particleSystem.isPlaying) particleSystem.Play();
            blackSword.transform.position = target.position;
            blackSword.transform.rotation = Quaternion.FromToRotation(Vector3.forward, rotationDirection);
            blackSword.transform.position = blackSword.transform.TransformPoint(0, 0, -2);
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
