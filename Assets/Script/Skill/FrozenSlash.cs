using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenSlash : SkillBase
{
    // Start is called before the first frame update
    private static ObjectPool iceReaperEffectPool;
    private static ObjectPool scytheSlashEffectPool;
    public override void Start()
    {
        AnimatorStateForSkill = State.CastSpellShort;
        UpperBodyCheckForAnimationTransition = true;
        ExecutionTimeAfterAnimationFrame = 0.4f;
        base.Start();
        GameObject iceReaperEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/IceReaper") as GameObject;
        iceReaperEffectPool ??= new ObjectPool(iceReaperEffectPrefab, 10, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));
        GameObject scytheSlashEffectPrefab = Resources.Load("Effect/FrozenSlashSkill/ScytheSlash") as GameObject;
        scytheSlashEffectPool ??= new ObjectPool(scytheSlashEffectPrefab, 30, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

    }

    public override void Awake() 
    {
        base.Awake();
    }

    public bool valid = false;
    
    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.K) && valid)
        {
            Trigger();
            valid = false;
            StartCoroutine(Valid());
        }
    }

    public void Trigger()
    {
        GameEffect iceReaperEffect = iceReaperEffectPool.PickOne().GameEffect;
        iceReaperEffect.transform.position = transform.position;
        GameEffect scytheSlashEffect = scytheSlashEffectPool.PickOne().GameEffect;
        scytheSlashEffect.transform.position = new Vector3(999, 999, 999);
        GameEffect scytheSlashEffect2 = scytheSlashEffectPool.PickOne().GameEffect;
        scytheSlashEffect2.transform.position = new Vector3(999, 999, 999);
        GameEffect scytheSlashEffect3 = scytheSlashEffectPool.PickOne().GameEffect;
        scytheSlashEffect3.transform.position = new Vector3(999, 999, 999);
        iceReaperEffect.animationEvent1Delegate = () =>
        {
            scytheSlashEffect.transform.position = transform.position + new Vector3(-0.88f, 1.625f, 1.08f);
            scytheSlashEffect.transform.rotation = Quaternion.Euler(50, 31, -108);
            scytheSlashEffect.VisualEffect.Play();
        };

        iceReaperEffect.animationEvent2Delegate = () =>
        {
            scytheSlashEffect2.transform.position = transform.position + new Vector3(1.59f, 0.09f, 0.35f);
            scytheSlashEffect2.transform.rotation = Quaternion.Euler(352.85f, 349.6f, 57f);
            scytheSlashEffect2.VisualEffect.Play();
        };

        iceReaperEffect.animationEvent3Delegate = () =>
        {
            scytheSlashEffect3.transform.position = transform.position + new Vector3(-0.46f, 0.68f, -0.66f);
            scytheSlashEffect3.transform.rotation = Quaternion.Euler(12.57f, 312.8f, 291.17f);
            scytheSlashEffect3.VisualEffect.Play();
        };

        iceReaperEffect.animationEvent4Delegate = () =>
        {
            iceReaperEffect.gameObject.SetActive(false);
            scytheSlashEffect.gameObject.SetActive(false);
            scytheSlashEffect2.gameObject.SetActive(false);
            scytheSlashEffect3.gameObject.SetActive(false);
        };
    }

    public IEnumerator Valid()
    {
        yield return new WaitForSeconds(0.5f);
        valid = true;
    }
}
