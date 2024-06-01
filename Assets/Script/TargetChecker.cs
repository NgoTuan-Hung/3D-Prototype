using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChecker : MonoBehaviour
{
    private CustomMonoBehavior nearestTarget;
    private float shortestDistance = int.MaxValue;
    private float tempDistance;

    public CustomMonoBehavior NearestTarget { get => nearestTarget; set => nearestTarget = value; }
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
        CustomMonoBehavior tempCustomMonoBehavior;
        if ((tempCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(other.gameObject)) != null)
        {
            tempDistance = Vector3.Distance(transform.position, other.transform.position);
        
            if (tempDistance < shortestDistance)
            {
                nearestTarget = tempCustomMonoBehavior;
                shortestDistance = tempDistance;
            }
            else if (tempCustomMonoBehavior.GetInstanceID() == nearestTarget.GetInstanceID()) shortestDistance = tempDistance;
        }
    }
}
