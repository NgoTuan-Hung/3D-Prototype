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
    private List<BoxCollider> boxColliders = new List<BoxCollider>();

    void Awake()
    {
        boxColliders = GetComponents<BoxCollider>().ToList();
        switch (itemType)
        {
            case ItemType.RandomSkillBook:
                bookSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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

                OnTriggerEnterDelegateInstance = () =>
                {
                    bookSkinnedMeshRenderer.SetMaterials(new List<Material> {bookDissolveMaterial, bookDissolveMaterial});
                    bookDissolveMaterial.SetFloat("Fade", 1);
                    StartCoroutine(BookDissolve(1.5f));
                };

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
    public Material bookMainMaterial;
    public Material bookDissolveMaterial;
    [SerializeField] private SkinnedMeshRenderer bookSkinnedMeshRenderer;
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

    public IEnumerator BookDissolve(float totalTime)
    {
        float timePassed = 0;
        float fade = 1, fadePerFrame = Time.fixedDeltaTime / totalTime;
        while (true)
        {
            bookSkinnedMeshRenderer.materials[0].SetFloat("Fade", fade -= fadePerFrame);
            bookSkinnedMeshRenderer.materials[1].SetFloat("Fade", fade);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            timePassed += Time.fixedDeltaTime;
            if (timePassed > totalTime)
            {
                break;
            }
        }
    }

    public delegate void OnCollisionEnterDelegate();
    public event OnCollisionEnterDelegate OnCollisionEnterDelegateInstance;
    void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterDelegateInstance?.Invoke();
    }

    public delegate void OnTriggerEnterDelegate();
    public event OnTriggerEnterDelegate OnTriggerEnterDelegateInstance;
    private void OnTriggerEnter(Collider other) 
    {
        OnTriggerEnterDelegateInstance?.Invoke();
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

    public IEnumerator Coroutine2(float time, Action action)
    {
        float timePassed = 0;
        while (true)
        {
            action();

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            timePassed += Time.fixedDeltaTime;
            if (timePassed >= time)
            {
                break;
            }
        }
    }
}
