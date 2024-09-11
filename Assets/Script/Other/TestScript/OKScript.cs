using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class OKScript : MonoBehaviour 
{
    public GameObject target;
    public GameObject source;
    private void FixedUpdate() 
    {
        if (Input.GetKey(KeyCode.LeftAlt)) {Cursor.visible = !Cursor.visible; Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;} 
    }

    private void Update()
    {
        //if (rotateToward) RotateToward();
    }

    public static bool rotateToward = false;
    public float rotateSpeed = 0.015f;
    
    private void RotateToward()
    {
        source.transform.rotation = Quaternion.Lerp(source.transform.rotation, Quaternion.LookRotation(target.transform.position - source.transform.position), rotateSpeed);
    }
}