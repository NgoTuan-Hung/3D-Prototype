using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 defaultPosition = new Vector3(0, 2.3f, -3.27f);
    public Vector3 behindPosition;
    public Vector3 rotation = new Vector3(10.95f, 0, 0);
    public PlayerInputSystem playerInputSystem;
    [SerializeField] private Vector2 zoom;
    [SerializeField] private float zoomAmount = 0.5f;
    [SerializeField] private RotatableObject rotatableObject;
    UtilObject utilObject = new UtilObject();

    private void Awake() 
    {
        playerInputSystem = new PlayerInputSystem();
        rotatableObject = new RotatableObject(transform);
        rotatableObject.RotateAmountAbs = 0.5f;
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
        behindPosition = defaultPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Zoom();
        transform.position = player.transform.position + behindPosition;
    }

    [SerializeField] private bool isTarget;

    public void Target(GameObject target)
    {
        StartCoroutine(TargetHandler(target));
        PlayerScript.cameraDelegate -= Target;
        PlayerScript.cameraDelegate += Untarget;
    }

    public void Untarget(GameObject target)
    {
        isTarget = false;
        PlayerScript.cameraDelegate -= Untarget;
        PlayerScript.cameraDelegate += Target;
    }

    public GameObject th_gameObj;
    public Vector3 directionVector;
    public Vector3 testPos;
    public bool lock1  = false;
    public IEnumerator TargetHandler(GameObject target)
    {
        isTarget = true;
        while (isTarget)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            directionVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
            th_gameObj.transform.position = player.transform.position;
            // th_gameObj.transform.Rotate(new Vector3(0, 
            // //utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up)
            // Vector3.SignedAngle(Vector3.forward, directionVector, Vector3.up)
            // , 0));
            th_gameObj.transform.rotation = Quaternion.Euler(new Vector3(0, 
            utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up)
            , 0));

            rotatableObject.RotateToDirection(utilObject, directionVector);
            behindPosition = th_gameObj.transform.TransformPoint(defaultPosition) - player.transform.position;
        }
    }

    void View()
    {
        // utilObject.RotateByAmount(cameraOfPlayer.transform, -mouseMoved.y, mouseMoved.x);
        // prevMouse = mouse;
    }

    public void Zoom()
    {
        zoom = playerInputSystem.Control.Zoom.ReadValue<Vector2>();
        var z = zoom.y > 0 ? zoomAmount : (zoom.y < 0 ? -zoomAmount : 0);
        if (z != 0) defaultPosition = transform.TransformPoint(0, 0, z) - player.transform.position;
        // behindPosition = defaultPosition;

        // var current = cameraOfPlayer.transform;
        // utilObject.BackWardByAmount(cameraOfPlayer.transform, new Vector3(0, 0, zoom.y > 0 ? -zoomAmount : (zoom.y < 0 ? zoomAmount : 0)));
        // if (current != cameraOfPlayer.transform) Debug.Log("OK");
        //var z = zoom.y > 0 ? zoomAmount : (zoom.y < 0 ? -zoomAmount : 0);
        //var temp = cameraOfPlayer.transform.TransformPoint(0, 0, z);
        //cameraOfPlayer.transform.position = cameraOfPlayer.transform.TransformPoint(-1, 0, 0);
    }

    void OnEnable()
    {
        playerInputSystem.Control.Enable();
        PlayerScript.cameraDelegate += Target;
    }

    void OnDisable()
    {
        playerInputSystem.Control.Disable();
    }
}
