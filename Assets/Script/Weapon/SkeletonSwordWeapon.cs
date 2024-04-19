using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordWeapon : Weapon
{
    private GameObject stabParticleSystemObject;
    private GameObject stabParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        stabParticleSystemObject = Resources.Load("Effect/StabEffect") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
        stabParticleSystem = Instantiate(stabParticleSystemObject, transform.position, transform.parent.rotation);
    }

    public override void OnCollisionEnter(Collision other) 
    {
        base.OnCollisionEnter(other);
            
    }
}
