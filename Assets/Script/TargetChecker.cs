using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChecker : MonoBehaviour
{
    private GameObject nearestTarget;
    private float shortestDistance = int.MaxValue;
    private float tempDistance;

    public GameObject NearestTarget { get => nearestTarget; set => nearestTarget = value; }
    public float ShortestDistance { get => shortestDistance; set => shortestDistance = value; }
    public float TempDistance { get => tempDistance; set => tempDistance = value; }

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
