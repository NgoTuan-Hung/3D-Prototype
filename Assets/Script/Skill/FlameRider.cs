
using UnityEngine;

public class FlameRider : SkillBase
{
    private static ObjectPool fireShieldEffectPool;
    public override void Awake()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAction = true;
        // ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Awake();
        GameObject fireShieldEffectPrefab = Resources.Load("Effect/FireShield") as GameObject;
        fireShieldEffectPool ??= new ObjectPool(fireShieldEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        
    }

    public override void Start()
    {
        base.Start();
    }
}