using Unity.Mathematics;
using UnityEngine;

public class OKScript : MonoBehaviour 
{
    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.LeftAlt)) {Cursor.visible = !Cursor.visible; Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;} 
    }
}