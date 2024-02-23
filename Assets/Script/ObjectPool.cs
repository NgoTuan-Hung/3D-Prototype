using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool
{
    public List<GameObject> pool;
    public GameObject prefab;
    public int size;

    public ObjectPool(GameObject prefab, int size)
    {
        this.prefab = prefab;
        this.size = size;

        pool = new List<GameObject>(size);
        for (int i=0;i<pool.Capacity;i++)
        {
            pool.Add(GameObject.Instantiate(prefab));
            pool[i].SetActive(false);
        }
    }

    public GameObject PickOne()
    {
        for (int i=0;i<pool.Count;i++)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }

        return null;
    }
}
