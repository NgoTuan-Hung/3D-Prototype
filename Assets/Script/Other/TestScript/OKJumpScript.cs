
using UnityEngine;

public class OKJumpScript : MonoBehaviour
{
    private CustomMonoBehavior customMonoBehavior;

    private void Awake()
    {
        customMonoBehavior = GetComponent<CustomMonoBehavior>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            customMonoBehavior.HumanLikeJumpableObject.Jump();
        }
    }
}