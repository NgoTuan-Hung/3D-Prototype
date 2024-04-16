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

    new private void OnCollisionEnter(Collision other) 
    {
        base.OnCollisionEnter(other);
        Debug.Log("collide " + other.gameObject.name);
    }
}