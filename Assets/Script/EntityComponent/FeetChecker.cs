using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetChecker : MonoBehaviour 
{
    private BoxCollider boxCollider;
    private bool isTouching = false;
    public bool IsTouching { get => isTouching; set => isTouching = value; }

    private void Awake() 
    {
        boxCollider = GetComponent<BoxCollider>();    
    }

    private void OnCollisionStay(Collision other) 
    {
        isTouching = true;    
    }

    private void OnCollisionExit(Collision other) 
    {
        isTouching = false;
    }
}