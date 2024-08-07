using Unity.Mathematics;
using UnityEngine;

public class MyGameObject1 : MonoBehaviour 
{
    public new Rigidbody rigidbody;
    public HumanLikeAnimatorBrain humanLikeAnimatorBrain;
    public Animator animator;
    public new Camera camera;
    public GameObject cameraPoint;
    public GameObject lookAtConstraintObjectParent;
    public GameObject lookAtConstraintObject;
    public Vector3 cameraPointToCameraVector;

    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody>();    
        humanLikeAnimatorBrain = GetComponent<HumanLikeAnimatorBrain>();
        animator = GetComponent<Animator>();
        cameraPoint = GameObject.Find("CameraPoint");
        lookAtConstraintObjectParent = GameObject.Find("LookAtConstraintObjectParent");
        lookAtConstraintObject = GameObject.Find("LookAtConstraintObject");
    } 


    public bool canRotateThisFrame;
    private void FixedUpdate() 
    {
        canRotateThisFrame = false;
        cameraPointToCameraVector = transform.position - camera.transform.position;
        cameraPoint.transform.rotation = Quaternion.LookRotation(new Vector3(cameraPointToCameraVector.x, 0, cameraPointToCameraVector.z));
        if (math.abs(Quaternion.Angle(cameraPoint.transform.rotation, transform.rotation)) > 100) {transform.rotation = cameraPoint.transform.rotation;}
        else canRotateThisFrame = true;
        lookAtConstraintObjectParent.transform.rotation = Quaternion.LookRotation(cameraPointToCameraVector);
        lookAtConstraintObject.transform.position = lookAtConstraintObjectParent.transform.TransformPoint(Vector3.forward);
        if (Input.GetKey(KeyCode.LeftAlt)) {Cursor.visible = !Cursor.visible; Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;} 
    }
}