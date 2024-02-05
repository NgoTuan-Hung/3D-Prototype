using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Vector3 defaultPosition = new Vector3(0, 2.3f, -3.27f);
    public Vector3 rotation = new Vector3(10.95f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + defaultPosition;
    }

    void UpdatePosition()
    {

    }
}
