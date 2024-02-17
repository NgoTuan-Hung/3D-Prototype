using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public GameObject nearestTarget;
    public float shortestDistance = int.MaxValue;
    public float tempDistance;
    public Transform myTransform;

    private void Start() 
    {

    }

    private void OnTriggerEnter(Collider other) 
    {
        GetNearestTarget(other);
    }

    private void OnTriggerStay(Collider other) 
    {
        GetNearestTarget(other);
    }

    public void GetNearestTarget(Collider other)
    {
        tempDistance = Vector3.Distance(myTransform.position, other.transform.position);
        if (tempDistance < shortestDistance)
        {
            nearestTarget = other.gameObject;
            shortestDistance = tempDistance;
        }
        else if (other.gameObject == nearestTarget) shortestDistance = tempDistance;
    }
}
