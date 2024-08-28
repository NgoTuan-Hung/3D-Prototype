using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {RandomSkillBook}
    public ItemType itemType;
    private bool interactable = true;
    private string skillName;
    private Animator animator;
    private ObjectPool objectPool;

    void Awake()
    {
        switch (itemType)
        {
            case ItemType.RandomSkillBook:
                skillName = "SkillBook";
                OnInteract = SkillBookInteract;
                animator = GetComponent<Animator>();
                objectPool = new ObjectPool
                (
                    Resources.Load("Effect/Book/BookTrail") as GameObject,
                    20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self)
                );
                AnimationClip animationClip = animator.runtimeAnimatorController.animationClips.First
                (
                    clip => clip.name.Equals("BookAnim")
                );
                animationClip.AddEvent(new AnimationEvent()
                {
                    functionName = "RandomSkillBookSpawnVFX",
                    time = 0.915f
                });
                break;
            default:
                break;
        }
    }

    private delegate void FixedUpdateDelegate();
    private FixedUpdateDelegate FixedUpdateDelegateInstance;
    void FixedUpdate()
    {
        FixedUpdateDelegateInstance?.Invoke();
    }

    public delegate void InteractableDelegate(Collision collision);
    public event InteractableDelegate OnInteract;

    private CustomMonoBehavior interactedCustomMonoBehavior;
    public void SkillBookInteract(Collision collision)
    {
        interactedCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(collision.gameObject);
    }

    public void RandomSkillBookSpawnVFX()
    {
        GameEffect gameEffect;
        gameEffect = objectPool.PickOne().GameEffect;
        gameEffect.transform.position = transform.position;
        gameEffect.VisualEffect.Play();
        
        StartCoroutine(Coroutine1(5f, () => gameEffect.gameObject.SetActive(false)));
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

    private delegate void AnimationEventDelegate();
    private AnimationEventDelegate AnimationEventDelegateInstance;
    public void AnimationEvent1()
    {
        AnimationEventDelegateInstance?.Invoke();
    }

    public IEnumerator Coroutine1(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        
        action();
    }
}
