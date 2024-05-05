using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordWeapon : Weapon
{
    private GameObject stabParticleSystemObject;
    [SerializeField] private static ObjectPool<GameEffect> stabParticleSystemPool {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        stabParticleSystemObject = Resources.Load("Effect/StabEffect") as GameObject;
        if (stabParticleSystemPool == null) stabParticleSystemPool = new ObjectPool<GameEffect>(stabParticleSystemObject, 20, ObjectPool<GameEffect>.WhereComponent.Self);
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
        ObjectPoolClass<GameEffect> stabParticleSystemObjectPoolClass = stabParticleSystemPool.PickOne();
        stabParticleSystemObjectPoolClass.GameObject.transform.position = transform.position;
        stabParticleSystemObjectPoolClass.GameObject.transform.rotation = transform.parent.rotation;
        stabParticleSystemObjectPoolClass.Component.ParticleSystem.Play();
    }

    public void ChargeAttack()
    {
        ObjectPoolClass<GameEffect> stabParticleSystemObjectPoolClass = stabParticleSystemPool.PickOneWithoutActive();
        var main = stabParticleSystemObjectPoolClass.Component.ParticleSystem.main;
        main.startSizeXMultiplier = 120f; main.startSizeYMultiplier = 120f; main.startSizeZMultiplier = 180f;
        main.startLifetime = 1f;
        stabParticleSystemObjectPoolClass.Component.transform.rotation = Quaternion.Euler(0, 0, 0);
        stabParticleSystemObjectPoolClass.Component.gameObject.SetActive(true);
        stabParticleSystemObjectPoolClass.Component.Follow(transform.parent, new Vector3(0, 0, 1.86f), true, true);

        animator.SetBool("ChargeAttack", true);
        //stabParticleSystemObjectPoolClass.Component.ParticleSystem.Play();
        stabParticleSystemObjectPoolClass.Component.onDisableDelegate += () => stabParticleSystemObjectPoolClass.Component.SetParticleSystemOriginalValue();
    }

    public void StopChargeAttack()
    {
        animator.SetBool("ChargeAttack", false);
        transform.parent.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
