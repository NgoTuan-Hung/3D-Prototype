using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordWeapon : Weapon
{
    private GameObject stabParticleSystemObject;
    [SerializeField] private static ObjectPool<ParticleSystem> stabParticleSystemPool {get; set;}
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
        ObjectPoolClass<ParticleSystem> stabParticleSystemObjectPoolClass = stabParticleSystemPool.PickOne();
        stabParticleSystemObjectPoolClass.GameObject.transform.position = transform.position;
        stabParticleSystemObjectPoolClass.GameObject.transform.rotation = transform.parent.rotation;
        stabParticleSystemObjectPoolClass.Component.Play();
    }

    public void ChargeAttack()
    {
        ObjectPoolClass<ParticleSystem> stabParticleSystemObjectPoolClass = stabParticleSystemPool.PickOne();
        var main = stabParticleSystemObjectPoolClass.Component.main;
        main.startSizeXMultiplier = 2f; main.startSizeYMultiplier = 2f; main.startSizeZMultiplier = 3f;
        main.startLifetime = 1f;
        stabParticleSystemObjectPoolClass.Component.transform.parent = transform.parent;
        stabParticleSystemObjectPoolClass.Component.transform.position = new Vector3(0, 0, 1.86f);

        animator.SetBool("ChargeAttack", true);
        stabParticleSystemObjectPoolClass.Component.Play();
        StartCoroutine(ResetStabParticleSystem(stabParticleSystemObjectPoolClass.Component, main));
    }

    IEnumerator ResetStabParticleSystem(ParticleSystem particleSystem, ParticleSystem.MainModule mainModule)
    {
        yield return new WaitForSeconds(mainModule.startLifetime.constantMax);

        mainModule.startLifetime = 0.5f;
        mainModule.startSizeXMultiplier = 1f; mainModule.startSizeYMultiplier = 1f; mainModule.startSizeZMultiplier = 1f;
        particleSystem.transform.parent = null;
        particleSystem.transform.position = new Vector3(0, 0, 0);
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
