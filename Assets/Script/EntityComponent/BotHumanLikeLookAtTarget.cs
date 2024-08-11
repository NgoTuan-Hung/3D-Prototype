using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CustomMonoBehavior), typeof(HumanLikeLookable))]
public class BotHumanLikeLookAtTarget : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    [SerializeField] private bool isLookingAtTarget = true;
    private GameObject tempEye;
    public bool IsLookingAtTarget { get => isLookingAtTarget; set => isLookingAtTarget = value; }

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        customMonoBehavior.Target = GameObject.Find("Player");
        changeFreeDirectionCoroutine = customMonoBehavior.NullCoroutine();
        freeDirectionEyeLookCoroutine = customMonoBehavior.NullCoroutine();
        LookDelegateMethod = LookAtTarget;
    }

    private void Start()
    {
        tempEye = CustomMonoBehavior.freeObjectPool.PickOne().GameObject;
    }
    private void FixedUpdate()
    {
        LookDelegateMethod?.Invoke();
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

    public delegate void LookDelegate();
    public LookDelegate LookDelegateMethod;
    public void ChangeMode()
    {
        if (isLookingAtTarget)
        {
            isLookingAtTarget = false;
            changeFreeDirectionCoroutine = StartCoroutine(ChangeFreeDirection());
            LookDelegateMethod = FreeLook;
        }
        else
        {
            isLookingAtTarget = true;
            StopCoroutine(changeFreeDirectionCoroutine);
            StopCoroutine(freeDirectionEyeLookCoroutine);
            LookDelegateMethod = LookAtTarget;
        }
    }

    public delegate void MoveEyeDelegate();
    public void FreeLook()
    {
        customMonoBehavior.HumanLikeLookable.EyeLook();
    }

    [SerializeField] private float ChangeFreeDirectionInterval = 0.75f;
    [SerializeField] private float freeLookUpperBound = -135;
    [SerializeField] private float freeLookLowerBound = 45;
    private Coroutine changeFreeDirectionCoroutine;
    [SerializeField] private float moveEyeTime = 0.5f;
    private Vector3 previousEyeDirection;
    public IEnumerator ChangeFreeDirection()
    {
        while (true)
        {
            StopCoroutine(freeDirectionEyeLookCoroutine);
            tempEye.transform.SetPositionAndRotation(customMonoBehavior.CameraPoint.transform.position, customMonoBehavior.CameraPoint.transform.rotation);
            tempEye.transform.Rotate(Vector3.up, Random.Range(0, 360));
            tempEye.transform.Rotate
            (
                tempEye.transform.TransformDirection(Vector3.right),
                Random.Range(freeLookUpperBound, freeLookLowerBound)
            );

            Vector3 newEyeDirection = tempEye.transform.TransformDirection(Vector3.forward);
            tempEye.transform.rotation = customMonoBehavior.CameraPoint.transform.rotation;
            previousEyeDirection = customMonoBehavior.CameraPoint.transform.TransformDirection(Vector3.forward);

            freeDirectionEyeLookCoroutine = StartCoroutine(FreeDirectionEyeLook(newEyeDirection));

            yield return new WaitForSeconds(ChangeFreeDirectionInterval);
        }
    }

    private Coroutine freeDirectionEyeLookCoroutine;
    public IEnumerator FreeDirectionEyeLook(Vector3 newEyeDirection)
    {
        float passedTime = 0;
        while (true)
        {
            tempEye.transform.position = customMonoBehavior.CameraPoint.transform.position;
            customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = tempEye.transform.TransformPoint
            (
                Vector3.Lerp(previousEyeDirection, newEyeDirection, passedTime / moveEyeTime)
            );

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime >= moveEyeTime) break;
        }

        while (true)
        {
            tempEye.transform.position = customMonoBehavior.CameraPoint.transform.position;
            customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = tempEye.transform.TransformPoint(newEyeDirection);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}