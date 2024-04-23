using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TestObject : MonoBehaviour
{
    public GameObject target;
    public GameObject originalPoint;

    void FixedUpdate()
    {
        var vector3 = target.transform.position - transform.position;
        if (vector3 != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(vector3, Vector3.Cross(originalPoint.transform.TransformPoint(Vector3.forward), vector3));
            transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        }

        // Debug.DrawLine(transform.position, target.transform.position, Color.red);
        // Debug.DrawLine(transform.position, originalPoint.transform.TransformPoint(Vector3.forward * 10), Color.green);
        
    }
}
