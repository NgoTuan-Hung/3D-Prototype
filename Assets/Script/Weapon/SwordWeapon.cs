using System.Collections;
using System.Linq;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private AnimationClip bigSwordClip;

    public AnimationClip BigSwordClip { get => bigSwordClip; set => bigSwordClip = value; }

    private void Awake() 
    {
        StartParent();
        bigSwordClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault((animatorClip) => animatorClip.name.Equals("BigSword"));
    }

    private void OnCollisionEnter(Collision other) 
    {
        OnCollisionEnterParent(other);
    }
}