using System.Collections;
using System.Linq;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private AnimationClip bigSwordClip;
    private TrailRenderer flyingTrail;
    public AnimationClip BigSwordClip { get => bigSwordClip; set => bigSwordClip = value; }
    public TrailRenderer FlyingTrail { get => flyingTrail; set => flyingTrail = value; }

    new private void Awake() 
    {
        base.Awake();
        bigSwordClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault((animatorClip) => animatorClip.name.Equals("BigSword"));
        flyingTrail = GetComponentInChildren<TrailRenderer>();
        flyingTrail.enabled = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void Attack()
    {
        base.Attack();
        attackTrail.enabled = true;
    }

    public override void StopAttack()
    {
        base.StopAttack();
        attackTrail.enabled = false;
    }
}