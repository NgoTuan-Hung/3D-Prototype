using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SystemObject = System.Object;

public class ObjectPool<T>
{
    public List<ObjectPoolClass<T>> pool;
    public GameObject prefab;
    public int size;
    public enum WhereComponent {Child, Self}

    public ObjectPool(GameObject prefab, int size, WhereComponent whereComponent)
    {
        this.prefab = prefab;
        this.size = size;

        pool = new List<ObjectPoolClass<T>>(size);
        switch (whereComponent)
        {
            case WhereComponent.Child:
                for (int i=0;i<pool.Capacity;i++)
                {
                    ObjectPoolClass<T> objectPoolClass = new ObjectPoolClass<T>
                    {
                        GameObject = GameObject.Instantiate(prefab)
                    };
                    objectPoolClass.Component = objectPoolClass.GameObject.GetComponentInChildren<T>();
                    
                    pool.Add(objectPoolClass);
                    pool[i].GameObject.SetActive(false);
                }
                
                break;
            case WhereComponent.Self:
                for (int i=0;i<pool.Capacity;i++)
                {
                    ObjectPoolClass<T> objectPoolClass = new ObjectPoolClass<T>
                    {
                        GameObject = GameObject.Instantiate(prefab)
                    };
                    objectPoolClass.Component = objectPoolClass.GameObject.GetComponent<T>();
                    
                    pool.Add(objectPoolClass);
                    pool[i].GameObject.SetActive(false);
                }
                break;
        }
        
    }

    public ObjectPoolClass<T> PickOne()
    {
        for (int i=0;i<pool.Count;i++)
        {
            if (!pool[i].GameObject.activeSelf)
            {
                pool[i].GameObject.SetActive(true);
                return pool[i];
            }
        }

        return null;
    }

    public ObjectPoolClass<T> PickOneWithoutActive()
    {
        for (int i=0;i<pool.Count;i++)
        {
            if (!pool[i].GameObject.activeSelf)
            {
                return pool[i];
            }
        }

        return null;
    }

    public List<ObjectPoolClass<T>> Pick(int n)
    {
        int count = 0;
        List<ObjectPoolClass<T>> objectPoolClasses = new List<ObjectPoolClass<T>>();
        for (int i=0;i<pool.Count;i++)
        {
            if (count >= n) return objectPoolClasses;

            if (!pool[i].GameObject.activeSelf)
            {
                pool[i].GameObject.SetActive(true);
                objectPoolClasses.Add(pool[i]);
                count++;
            }
        }

        return null;
    }
}

public class ObjectPoolClass<T>
{
    private GameObject gameObject;
    private T component;

    public GameObject GameObject { get => gameObject; set => gameObject = value; }
    public T Component { get => component; set => component = value; }
}
