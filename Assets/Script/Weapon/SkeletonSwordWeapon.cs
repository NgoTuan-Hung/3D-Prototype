using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSwordWeapon : Weapon
{
    private GameObject stabParticleSystemObject;
    [SerializeField] private static ObjectPool stabParticleSystemPool {get; set;}

    public override void Awake()
    {
        base.Awake();
        stabParticleSystemObject = Resources.Load("Effect/StabEffect") as GameObject;
        stabParticleSystemPool ??= new ObjectPool(stabParticleSystemObject, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
    }
    // Start is called before the first frame update
    void Start()
    {

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
        PoolObject stabParticleSystemPoolObject = stabParticleSystemPool.PickOne();
        stabParticleSystemPoolObject.GameObject.transform.position = transform.position;
        stabParticleSystemPoolObject.GameObject.transform.rotation = transform.parent.rotation;
        stabParticleSystemPoolObject.GameEffect.ParticleSystem.Play();
        stabParticleSystemPoolObject.GameEffect.ParticleSystemEvent.particleSystemEventDelegate += () => stabParticleSystemPoolObject.GameObject.SetActive(false);
    }

    public void ChargeAttack()
    {
        PoolObject stabParticleSystemPoolObject = stabParticleSystemPool.PickOneWithoutActive();
        var main = stabParticleSystemPoolObject.GameEffect.ParticleSystem.main;
        main.startSizeXMultiplier = 120f; main.startSizeYMultiplier = 120f; main.startSizeZMultiplier = 180f;
        main.startLifetime = 1f;
        stabParticleSystemPoolObject.GameEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
        stabParticleSystemPoolObject.GameEffect.gameObject.SetActive(true);
        stabParticleSystemPoolObject.GameEffect.ParticleSystemEvent.particleSystemEventDelegate += () => stabParticleSystemPoolObject.GameObject.SetActive(false);
        stabParticleSystemPoolObject.GameEffect.Follow(transform.parent, new Vector3(0, 0, 1.86f), true, true);

        animator.SetBool("ChargeAttack", true);
        //stabParticleSystemPoolObject.Component.ParticleSystem.Play();
        stabParticleSystemPoolObject.GameEffect.onDisableDelegate += () => stabParticleSystemPoolObject.GameEffect.SetParticleSystemOriginalValue();
    }

    public void StopChargeAttack()
    {
        animator.SetBool("ChargeAttack", false);
        transform.parent.gameObject.SetActive(false);
    }
}
