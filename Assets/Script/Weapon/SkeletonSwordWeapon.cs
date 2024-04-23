using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordWeapon : Weapon
{
    private GameObject stabParticleSystemObject;
    private GameObject stabParticleSystem;
    [SerializeField] private static ObjectPool<ParticleSystem> stabParticleSystemPool {get; set;}
    ObjectPoolClass<ParticleSystem> stabParticleSystemObjectPoolClass;
    // Start is called before the first frame update
    void Start()
    {
        stabParticleSystemObject = Resources.Load("Effect/StabEffect") as GameObject;
        stabParticleSystemPool = new ObjectPool<ParticleSystem>(stabParticleSystemObject, 20, ObjectPool<ParticleSystem>.WhereComponent.Self);
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
        stabParticleSystemObjectPoolClass = stabParticleSystemPool.PickOne();
        stabParticleSystemObjectPoolClass.GameObject.transform.position = transform.position;
        stabParticleSystemObjectPoolClass.GameObject.transform.rotation = transform.parent.rotation;
        stabParticleSystemObjectPoolClass.Component.Play();
    }

    public override void OnCollisionEnter(Collision other) 
    {
        base.OnCollisionEnter(other);
    }
}
