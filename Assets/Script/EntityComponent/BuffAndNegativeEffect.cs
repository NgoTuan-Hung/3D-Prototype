using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAndNegativeEffect : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    public enum Effect {Freeze, Burn, Stun, Heal}
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
}
