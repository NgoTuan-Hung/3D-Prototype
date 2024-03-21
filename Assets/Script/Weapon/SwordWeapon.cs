using UnityEngine;

public class SwordWeapon : Weapon
{
    private void Awake() 
    {
        StartParent();    
    }

    private void OnCollisionEnter(Collision other) 
    {
        OnCollisionEnterParent(other);
    }
}