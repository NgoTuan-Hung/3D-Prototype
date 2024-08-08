using UnityEngine;

public class BotHumanLikeLookAtTarget : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private GameObject eyeAtTarget;
    
    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
        customMonoBehavior.Target = GameObject.Find("Player");
    }

    private void Start()
    {
        eyeAtTarget = CustomMonoBehavior.freeObjectPool.PickOne().GameObject;
    }

    private void FixedUpdate()
    {
        SetEyeAtTarget();
        LookAtTarget();
    }

    private void SetEyeAtTarget()
    {
        eyeAtTarget.transform.position = customMonoBehavior.Target.transform.position + new Vector3(0, 1.3f, 0);
    }

    private void LookAtTarget()
    {
        customMonoBehavior.CanRotateThisFrame = false;
        customMonoBehavior.CameraPointToCameraVector = eyeAtTarget.transform.position - customMonoBehavior.CameraPoint.transform.position;
        customMonoBehavior.CameraPoint.transform.rotation = Quaternion.LookRotation(new Vector3(customMonoBehavior.CameraPointToCameraVector.x, 0, customMonoBehavior.CameraPointToCameraVector.z));
        if (Mathf.Abs(Quaternion.Angle(customMonoBehavior.CameraPoint.transform.rotation, transform.rotation)) > 100) {transform.rotation = customMonoBehavior.CameraPoint.transform.rotation;}
        else customMonoBehavior.CanRotateThisFrame = true;
        customMonoBehavior.LookAtConstraintObjectParent.transform.rotation = Quaternion.LookRotation(customMonoBehavior.CameraPointToCameraVector);
        customMonoBehavior.LookAtConstraintObject.transform.position = customMonoBehavior.LookAtConstraintObjectParent.transform.TransformPoint(Vector3.forward);
    }
}