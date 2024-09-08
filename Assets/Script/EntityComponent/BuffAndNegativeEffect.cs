using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAndNegativeEffect : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private ObjectPool freezeEffectPool;
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
        GameObject freezeEffectPrefab = Resources.Load("Effect/Freeze") as GameObject;
        freezeEffectPool = new ObjectPool(freezeEffectPrefab, 10, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
    }

    public void ApplyEffect(Effect effect, DurationType durationType, float duration = 0, float delay = 0)
    {
        switch (effect)
        {
            case Effect.Freeze:
                if (CheckHavingEffectBit(Effect.Burn) || CheckHavingEffectBit(Effect.SelfBurn)) return;
                ApplyEffectBit(effect);
                StartCoroutine(ApplyFreezing(duration));
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

        customMonoBehavior.Freeze();
        customMonoBehavior.MainSkinnedMeshRenderer.material = GlobalObject.Instance.freezeMaterial;
        GameEffect freezeEffect = freezeEffectPool.PickOne().GameEffect;
        freezeEffect.transform.position = transform.position;
        freezeEffect.VisualEffect.SetSkinnedMeshRenderer("SkinnedMeshRenderer", customMonoBehavior.MainSkinnedMeshRenderer);
        freezeEffect.VisualEffect.Play();

        while (isFreezing)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            time += Time.fixedDeltaTime;
            if (time >= duration)
            {
                isFreezing = false;
            }
        }

        customMonoBehavior.UnFreeze();
        customMonoBehavior.MainSkinnedMeshRenderer.material = customMonoBehavior.MainMaterial;
        freezeEffect.gameObject.SetActive(false);
        RemoveEffectBit(Effect.Freeze);
        freezeEffect.VisualEffect.Stop();
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
