using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(HumanLikeAnimatorBrain), typeof(HumanLikeJumpableObject))]
public class BotHumanLikeJumpRandomly : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;

    void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    [SerializeField] private bool canExecuteBotJump = true;
    [SerializeField] private float jumpChancePerInterval = 0.5f;
    [SerializeField] private float jumpInterval = 2f;
    [SerializeField] private float doubleJumpChance = 0.7f;

    public bool CanExecuteBotJump { get => canExecuteBotJump; set => canExecuteBotJump = value; }
    public float JumpChancePerInterval { get => jumpChancePerInterval; set => jumpChancePerInterval = value; }
    public float JumpInterval1 { get => jumpInterval; set => jumpInterval = value; }
    public float DoubleJumpChance { get => doubleJumpChance; set => doubleJumpChance = value; }

    void FixedUpdate()
    {
        if (freeze) return;
        if (canExecuteBotJump && customMonoBehavior.HumanLikeJumpableObject.CurrentJumpCount < customMonoBehavior.HumanLikeJumpableObject.JumpCount)
        {
            if (Random.value < jumpChancePerInterval)
            {
                customMonoBehavior.HumanLikeJumpableObject.Jump();
                StartCoroutine(JumpInterval());
                StartCoroutine(DoubleJump());
            }
            else StartCoroutine(JumpInterval());
        }
    }

    IEnumerator DoubleJump()
    {
        while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return new WaitForSeconds(customMonoBehavior.HumanLikeJumpableObject.JumpDelay + Random.Range(0f, jumpInterval - customMonoBehavior.HumanLikeJumpableObject.JumpDelay));

        if (Random.value < doubleJumpChance)
        {
            customMonoBehavior.HumanLikeJumpableObject.Jump();
        }
    }

    IEnumerator JumpInterval()
    {
        while (freeze) yield return new WaitForSeconds(Time.fixedDeltaTime);
        canExecuteBotJump = false;
        yield return new WaitForSeconds(jumpInterval);
        canExecuteBotJump = true;
    }

    private bool freeze = false;
    public void Freeze() => freeze = true;
    public void UnFreeze() => freeze = false;
}
