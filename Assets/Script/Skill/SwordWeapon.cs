using System.Collections;
using System.Linq;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private AnimationClip bigSwordClip;
    private TrailRenderer flyingTrail;
    public AnimationClip BigSwordClip { get => bigSwordClip; set => bigSwordClip = value; }
    public TrailRenderer FlyingTrail { get => flyingTrail; set => flyingTrail = value; }

    public override void Awake() 
    {
        base.Awake();
        bigSwordClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault((animatorClip) => animatorClip.name.Equals("BigSword"));
        flyingTrail = GetComponentInChildren<TrailRenderer>();
        flyingTrail.enabled = false;
    }

    public override void Attack()
    {
        base.Attack();
        AttackTrail.enabled = true;
    }

    public override void StopAttack()
    {
        AttackTrail.enabled = false;
        base.StopAttack();
    }

    // public void OnEnable() 
    // {
    //     onEnableDelegate?.Invoke();
    //     onEnableDelegate = null;
    //     afterEnableDelegate?.Invoke();
    //     afterEnableDelegate = null;
    // }

    // public void OnDisable() 
    // {
    //     onDisableDelegate?.Invoke();
    //     onDisableDelegate = null;
    //     afterDisableDelegate?.Invoke();
    //     afterDisableDelegate = null;
    // }

    // public void ClearData()
    // {
    //     AttackTrail.Clear();
    //     flyingTrail.Clear();
    // }
}