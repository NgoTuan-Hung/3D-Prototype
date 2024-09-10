using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetChecker : ExtensibleMonobehavior
{
    private CustomMonoBehavior nearestTarget;
    private float shortestDistance = int.MaxValue;
    private float tempDistance;

    public CustomMonoBehavior NearestTarget { get => nearestTarget; set => nearestTarget = value; }
    public float ShortestDistance { get => shortestDistance; set => shortestDistance = value; }
    public float TempDistance { get => tempDistance; set => tempDistance = value; }
    private void Awake() {
        ExcludeTags = new List<string>();
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
        /* get current collider */
        CustomMonoBehavior tempCustomMonoBehavior;
        if ((tempCustomMonoBehavior = GlobalObject.Instance.GetCustomMonoBehavior(other.gameObject)) != null && !ExcludeTags.Contains(other.tag))
        {
            /* Compare it's distance with the current nearest target
            - If it's closer, set it as the nearest target
            - Else if it's the same target, update the shortest distance */
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
