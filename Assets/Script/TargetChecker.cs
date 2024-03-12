using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChecker : MonoBehaviour
{
    public GameObject nearestTarget;
    public float shortestDistance = int.MaxValue;
    public float tempDistance;
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
        tempDistance = Vector3.Distance(transform.position, other.transform.position);
        if (tempDistance < shortestDistance)
        {
            nearestTarget = other.gameObject;
            shortestDistance = tempDistance;
        }
        else if (other.gameObject == nearestTarget) shortestDistance = tempDistance;
    }
}
