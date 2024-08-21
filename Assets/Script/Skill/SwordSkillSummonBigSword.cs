using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

class SwordSkillSummonBigSword : SkillBase
{
    private bool isWaiting = false;
    private Coroutine summonCoroutine;
    private Vector2 skillCastVector;
    private GameObject skillCast;
    private float skillCastAngle;
    [SerializeField] private static ObjectPool bigSwordEffectPool {get; set;}

    public bool IsWaiting { get => isWaiting; set => isWaiting = value; }
    public Coroutine SummonCoroutine { get => summonCoroutine; set => summonCoroutine = value; }
    public Vector2 SkillCastVector { get => skillCastVector; set => skillCastVector = value; }
    public GameObject SkillCast { get => skillCast; set => skillCast = value; }
    public float SkillCastAngle { get => skillCastAngle; set => skillCastAngle = value; }

    public override void Awake()
    {
        base.Awake();
        if (CustomMonoBehavior.EntityType.Equals("Player"))
        {
            skillCast = Instantiate(Resources.Load("BigSwordSkillCast")) as GameObject;
            skillCast.SetActive(false);
        }
        GameObject bigSwordEffectPrefab = Resources.Load("Effect/SummonBigSword") as GameObject;
        bigSwordEffectPool ??= new ObjectPool(bigSwordEffectPrefab, 20, new PoolArgument(typeof(GameEffect), PoolArgument.WhereComponent.Self));

        SubSkillChangableAttributes.AddRange(new SubSkillChangableAttribute[] {coolDown});

        SubSkillRequiredParameter = new SubSkillRequiredParameter
        {
            Target = true
        };

        RecommendedAIBehavior.MaxDistanceToTarget = 15f;
        UseSkillChance = 25f;
    }

    public override void Start()
    {
        base.Start();
    }

    [SerializeField] private SubSkillChangableAttribute coolDown = new SubSkillChangableAttribute(SubSkillChangableAttribute.SubSkillAttributeValueType.Float, 5f, SubSkillChangableAttribute.SubSkillAttributeType.Cooldown);
    public void Trigger(InputAction.CallbackContext callbackContext)
    {
        if (!isWaiting)
        {
            if (CanUse)
            {
                CanUse = false;
                StartCoroutine(HandleSummonSwordPlayer());
            }
        }
        else isWaiting = false;
    }

    public override void Trigger(SubSkillParameter subSkillParameter)
    {
        if (CanUse)
        {
            CanUse = false;
            HandleSummonSword(subSkillParameter.Target.position);
            StartCoroutine(BeginCooldown());
        }
    }

    public IEnumerator BeginCooldown()
    {
        yield return new WaitForSeconds(coolDown.FloatValue);
        CanUse = true;
    }

    Vector3 tempVec;
    public IEnumerator HandleSummonSwordPlayer()
    {
        // isWaiting = true;
        // while (isWaiting)
        // {
        //     yield return new WaitForSeconds(Time.fixedDeltaTime);

        //     skillCast.transform.position = transform.position;
        //     skillCast.SetActive(true);
        //     skillCastAngle = CustomMonoBehavior.PlayerScript.CameraLookPoint.transform.eulerAngles.y;
        //     skillCast.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        // }
        // StartCoroutine(BeginCooldown());

        // skillCast.SetActive(false);
        // PoolObject poolObjectEffect = bigSwordEffectPool.PickOne();
        // GameEffect gameEffect = poolObjectEffect.GameEffect;
        // gameEffect.ParticleSystemEvent.particleSystemEventDelegate += () => poolObjectEffect.GameObject.SetActive(false);

        // gameEffect.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
        // gameEffect.transform.position = transform.position;
        // gameEffect.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        
        // CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", true);
        // CustomMonoBehavior.Animator.Play("UpperBody.CastSkillBlowDown", 1, 0);
        // StartCoroutine(StopSummon());
        //StartCoroutine(StopSword(swordWeapon));
        yield return new WaitForSeconds(0);
    }

    public void HandleSummonSword(Vector3 target)
    {
        skillCastAngle = Quaternion.LookRotation(target - transform.position).eulerAngles.y;
        PoolObject poolObjectEffect = bigSwordEffectPool.PickOne();
        GameEffect gameEffect = poolObjectEffect.GameEffect;
        gameEffect.ParticleSystemEvent.particleSystemEventDelegate += () => poolObjectEffect.GameObject.SetActive(false);

        gameEffect.CollideAndDamage.CollideExcludeTags = CustomMonoBehavior.AllyTags;
        gameEffect.transform.position = transform.position;
        gameEffect.transform.rotation = Quaternion.Euler(new Vector3(0, skillCastAngle, 0));
        
        // CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", true);
        // CustomMonoBehavior.Animator.Play("UpperBody.CastSkillBlowDown", 1, 0);
        //StartCoroutine(StopSummon());
    }

    // public IEnumerator StopSword(SwordWeapon swordWeapon)
    // {
    //     yield return new WaitForSeconds(swordWeapon.BigSwordClip.length);
    //     swordWeapon.Animator.SetBool("BigSword", false);
    //     CustomMonoBehavior.SkillableObject.StopSkillAnimator((int)SkillableObject.SkillID.SummonBigSword);
        
    //     yield return new WaitForSeconds(1);
    //     swordWeapon.transform.parent.gameObject.SetActive(false);
    //     swordWeapon.transform.localScale = Vector3.one;
    // }

    IEnumerator StopSummon()
    {
        // yield return new WaitForSeconds(CustomMonoBehavior.SkillableObject.CastSkillBlownDown.length);
        yield return new WaitForSeconds(0);

        CustomMonoBehavior.Animator.SetBool("CastSkillBlownDown", false);
    }
}