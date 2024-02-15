using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 defaultPosition = new Vector3(0, 2.3f, -3.27f);
    public Vector3 rotation = new Vector3(10.95f, 0, 0);
    public PlayerInputSystem playerInputSystem;
    [SerializeField] private Vector2 zoom;
    [SerializeField] private float zoomAmount = 0.5f;

    private void Awake() 
    {
        playerInputSystem = new PlayerInputSystem();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + defaultPosition;
        Zoom();
    }

    void UpdatePosition()
    {

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
        defaultPosition = transform.TransformPoint(0, 0, z) - player.transform.position;
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
    }

    void OnDisable()
    {
        playerInputSystem.Control.Disable();
    }
}
