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
    public enum LookMode {LookAtTarget, FreeLook, LookAroundTarget}
    private LookMode lookMode;
    public bool IsLookingAtTarget { get => isLookingAtTarget; set => isLookingAtTarget = value; }

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        customMonoBehavior.Target = GameObject.Find("Player");
        changeFreeDirectionCoroutine = customMonoBehavior.NullCoroutine();
        freeDirectionEyeLookCoroutine = customMonoBehavior.NullCoroutine();
        lookAroundTargetCoroutine = customMonoBehavior.NullCoroutine();
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
    public void ChangeMode(LookMode mode)
    {
        if (changeFreeDirectionCoroutine != null) StopCoroutine(changeFreeDirectionCoroutine);
        if (freeDirectionEyeLookCoroutine != null) StopCoroutine(freeDirectionEyeLookCoroutine);
        if (lookAroundTargetCoroutine != null) StopCoroutine(lookAroundTargetCoroutine);

        if (mode != lookMode)
        {
            switch (mode)
            {
                case LookMode.LookAtTarget:
                    isLookingAtTarget = true;
                    lookMode = LookMode.LookAtTarget;
                    LookDelegateMethod = LookAtTarget;
                    break;
                case LookMode.FreeLook:
                    isLookingAtTarget = false;
                    lookMode = LookMode.FreeLook;
                    changeFreeDirectionCoroutine = StartCoroutine(ChangeFreeDirection());
                    LookDelegateMethod = FreeLook;
                    break;
                case LookMode.LookAroundTarget:
                    isLookingAtTarget = false;
                    lookMode = LookMode.LookAroundTarget;
                    lookAroundTargetCoroutine = StartCoroutine(LookAroundTarget());
                    LookDelegateMethod = () => customMonoBehavior.HumanLikeLookable.EyeLook();
                    break;
            }
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
            /* Change direction every specified interval */
            if (CoroutineWrapper.CheckCoroutineNotNull(freeDirectionEyeLookCoroutine)) StopCoroutine(freeDirectionEyeLookCoroutine);
            /* Pick random object and set its position and rotation similar to the camera point */
            tempEye.transform.SetPositionAndRotation(customMonoBehavior.CameraPoint.transform.position, customMonoBehavior.CameraPoint.transform.rotation);
            /* Rotate its forward direction randomly horizontally and look up or down within boundary*/
            tempEye.transform.Rotate(Vector3.up, Random.Range(0, 360));
            tempEye.transform.Rotate
            (
                tempEye.transform.TransformDirection(Vector3.right),
                Random.Range(freeLookUpperBound, freeLookLowerBound)
            );

            /* Get the new eye vector and save the current eye vector to prepare for lerping between them.*/
            Vector3 newEyeDirection = tempEye.transform.TransformDirection(Vector3.forward);
            tempEye.transform.rotation = customMonoBehavior.CameraPoint.transform.rotation;
            previousEyeDirection = tempEye.transform.TransformDirection(Vector3.forward);

            /* Start lerping */
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
            /* move the temp eye along us and lerp our real eye along the position.
            The position is calculated based on the vector relative to our position */
            tempEye.transform.position = customMonoBehavior.CameraPoint.transform.position;
            customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = tempEye.transform.TransformPoint
            (
                Vector3.Lerp(previousEyeDirection, newEyeDirection, passedTime / moveEyeTime)
            );

            /* continue until reach specified time */
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            passedTime += Time.fixedDeltaTime;
            if (passedTime >= moveEyeTime) break;
        }

        /* after that just keep looking at final direction */
        while (true)
        {
            tempEye.transform.position = customMonoBehavior.CameraPoint.transform.position;
            customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = tempEye.transform.TransformPoint(newEyeDirection);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public Coroutine lookAroundTargetCoroutine;
    public IEnumerator LookAroundTarget()
    {
        Vector3 newEyePosition;
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            /* Make a temp object look at our target, and the new direction of our eye will be the left or right of
            that temp object */
            tempEye.transform.position = customMonoBehavior.CameraPoint.transform.position;
            tempEye.transform.rotation = Quaternion.LookRotation(customMonoBehavior.Target.transform.position - customMonoBehavior.CameraPoint.transform.position);
            newEyePosition = tempEye.transform.TransformPoint(Vector3.right);
            customMonoBehavior.HumanLikeLookable.EyeAt.transform.position = newEyePosition;
        }
    }
}