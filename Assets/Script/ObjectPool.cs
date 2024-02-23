using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    public List<T> pool;
    public int size;
    
    void Start()
    {
        pool = new List<T>(size);
        for (int i=0;i<pool.Capacity;i++)
        {
            // pool[i] = Instantiate<T>()
        }
    }

    public T PickOne()
    {
        for (int i=0;i<pool.Count;i++)
        {
            if (!pool[i].enabled)
            {
                pool[i].enabled = true;
                return pool[i];
            }
        }

        return null;
    }
}
