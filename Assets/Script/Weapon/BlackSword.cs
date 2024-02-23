using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSword : MonoBehaviour
{
    public Weapon weapon;
    [SerializeField] private GameObject blackSwordPrefab;
    Animator animator;
    // Start is called before the first frame update
    private void Start() 
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void Init()
    {
        weapon = new Weapon(blackSwordPrefab, 20, 3);
    }

    public void Attack(Transform location)
    {
        if (weapon.canAttack)
        {
            GameObject blackSword = weapon.weaponPool.PickOne();
            blackSword.transform.position = location.position;
            blackSword.transform.rotation = location.rotation;
            blackSword.GetComponentInChildren<Animator>().SetBool("Attack", true);
            StartCoroutine(ResetAttack());
        }
    }

    public void StopAttack()
    {
        animator.SetBool("Attack", false);
        gameObject.SetActive(false);
    }

    public IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(weapon.attackCooldown);

        weapon.canAttack = true;
    }
}
