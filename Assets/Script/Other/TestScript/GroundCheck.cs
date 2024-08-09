using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    int layerMask = Physics.AllLayers;
    public RaycastHit hit;
    public bool isGround = false;
    public float hitDistance;
    public float rayCastDistance = Mathf.Infinity;
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayCastDistance, layerMask))
        {
            isGround = true;
            hitDistance = hit.distance;
        }
        else
        {
            isGround = false;
        }
    }
}