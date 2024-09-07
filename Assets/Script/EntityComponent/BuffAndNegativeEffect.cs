using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAndNegativeEffect : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    public enum Effect 
    {
        Freeze = 0, 
        Burn = 1 << 0, 
        SelfBurn = 1 << 1,
        Wet = 1 << 2,
        Stun = 1 << 3, 
        Heal = 1 << 4,
    }

    public int currentEffect = 0;
    public enum DurationType {Once, OverTime}
    private bool isFreezing = false;
    private bool isBurning = false;
    private bool isStunning = false;

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    public void ApplyEffect(Effect effect, DurationType durationType, float duration = 0, float delay = 0)
    {
        switch (effect)
        {
            case Effect.Freeze:
                if (CheckHavingEffectBit(Effect.Burn) || CheckHavingEffectBit(Effect.SelfBurn)) return;
                break;
            case Effect.Burn:
                if (CheckHavingEffectBit(Effect.Wet)) return;
                break;
            default: break;
        }
    }

    public IEnumerator ApplyFreezing(float duration)
    {
        float time = 0;
        isFreezing = true;

        while (isFreezing)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            time += Time.fixedDeltaTime;
            if (time >= duration)
            {
                isFreezing = false;
            }
        }


    }

    public bool CheckHavingEffectBit(Effect effect)
    {
        return (currentEffect & (int)effect) != 0;
    }

    public void RemoveEffectBit(Effect effect)
    {
        currentEffect &= ~(int)effect;
    }

    public void ApplyEffectBit(Effect effect)
    {
        currentEffect |= (int)effect;
    }
}
