using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(HumanLikeLookable))]
public class BotHumanLikeLookAtTarget : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private bool isLookingAtTarget = false;
    public bool IsLookingAtTarget { get => isLookingAtTarget; set => isLookingAtTarget = value; }
    private float targetingChancePerInterval = 0.8f;
    private float targetingInterval = 1.5f;

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        customMonoBehavior.Target = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        if (Random.value < targetingChancePerInterval) { IsLookingAtTarget = true; LookAtTarget(); }
        else IsLookingAtTarget = false;
    }

    private void LookAtTarget()
    {
        SetEyeAtTarget();
        customMonoBehavior.HumanLikeLookable.EyeLook();
    }

    private void SetEyeAtTarget()
    {
        customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = customMonoBehavior.Target.transform.position + new Vector3(0, 1.3f, 0);
    }
}