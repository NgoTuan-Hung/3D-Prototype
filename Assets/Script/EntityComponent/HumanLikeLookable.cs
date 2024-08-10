using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLikeLookable : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;
    private GameObject eyeAt;
    public GameObject EyeAt { get => eyeAt; set => eyeAt = value; }

    private void Awake() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void Start() 
    {
        eyeAt = CustomMonoBehavior.freeObjectPool.PickOne().GameObject;
    }

    public void EyeLook()
    {
        customMonoBehavior.CanRotateThisFrame = false;
        customMonoBehavior.CameraPointToCameraVector = eyeAt.transform.position - customMonoBehavior.CameraPoint.transform.position;
        customMonoBehavior.CameraPoint.transform.rotation = Quaternion.LookRotation(new Vector3(customMonoBehavior.CameraPointToCameraVector.x, 0, customMonoBehavior.CameraPointToCameraVector.z));
        if (Mathf.Abs(Quaternion.Angle(customMonoBehavior.CameraPoint.transform.rotation, transform.rotation)) > 100) {transform.rotation = customMonoBehavior.CameraPoint.transform.rotation;}
        else customMonoBehavior.CanRotateThisFrame = true;
        customMonoBehavior.LookAtConstraintObjectParent.transform.rotation = Quaternion.LookRotation(customMonoBehavior.CameraPointToCameraVector);
        customMonoBehavior.LookAtConstraintObject.transform.position = customMonoBehavior.LookAtConstraintObjectParent.transform.TransformPoint(Vector3.forward);
    }
}
