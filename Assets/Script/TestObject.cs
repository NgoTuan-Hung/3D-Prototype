using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public GameObject testBehindObject;
    public Vector3 behindPosition = new Vector3(0, 0, -10);
    private void FixedUpdate() 
    {
        testBehindObject.transform.position = transform.TransformPoint(behindPosition);
    }
}
