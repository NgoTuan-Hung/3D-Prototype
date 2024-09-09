using UnityEngine;

public class PlayerHumanlikeThirdPersonViewController : ExtensibleMonobehavior 
{
    private CustomMonoBehavior customMonoBehavior;

    private void Start() 
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();    
    }

    private void FixedUpdate() 
    {
        if (Freeze1) return;
        customMonoBehavior.CanRotateThisFrame = false;
        customMonoBehavior.CameraPointToCameraVector = customMonoBehavior.CameraPoint.transform.position - customMonoBehavior.Camera.transform.position;
        customMonoBehavior.CameraPoint.transform.rotation = Quaternion.LookRotation(new Vector3(customMonoBehavior.CameraPointToCameraVector.x, 0, customMonoBehavior.CameraPointToCameraVector.z));
        if (Mathf.Abs(Quaternion.Angle(customMonoBehavior.CameraPoint.transform.rotation, transform.rotation)) > 100) {transform.rotation = customMonoBehavior.CameraPoint.transform.rotation;}
        else customMonoBehavior.CanRotateThisFrame = true;
        customMonoBehavior.LookAtConstraintObjectParent.transform.rotation = Quaternion.LookRotation(customMonoBehavior.CameraPointToCameraVector);
        customMonoBehavior.LookAtConstraintObject.transform.position = customMonoBehavior.LookAtConstraintObjectParent.transform.TransformPoint(Vector3.forward);
    }
}