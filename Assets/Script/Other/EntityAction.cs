using System.Collections;
using UnityEngine;

public class EntityAction : MonoBehaviour
{
    public void IfDo(bool condition, IEnumerator iEnumerator)
    {
        if (condition)
        {
            StartCoroutine(iEnumerator);
        }
    }
}