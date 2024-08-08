using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using SystemObject = System.Object;

public class ObjectPool
{
    public List<PoolObject> pool;
    public int size;

    public ObjectPool(GameObject prefab, int size, params PoolArgument[] poolArguments)
    {
        this.size = size;

        pool = new List<PoolObject>(size);
        for (int i=0;i<pool.Capacity;i++)
        {
            PoolObject poolObject = new PoolObject
            {
                GameObject = GameObject.Instantiate(prefab)
            };

            pool.Add(poolObject);
        }

        if (poolArguments.Length > 0)
        {   
            for (int i=0;i<pool.Capacity;i++)
            {
                foreach (PoolArgument poolArgument in poolArguments)
                {
                    switch (poolArgument.whereComponent)
                    {
                        case PoolArgument.WhereComponent.Child:
                            if (poolArgument.Type == typeof(GameObject)) break; 
                            else
                            {
                                foreach (FieldInfo fieldInfo in typeof(PoolObject).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                                {   
                                    if (fieldInfo.FieldType == poolArgument.Type)
                                    {
                                        fieldInfo.SetValue(pool[i], pool[i].GameObject.GetComponentInChildren(poolArgument.Type));
                                    }
                                }
                            }

                            break;
                        case PoolArgument.WhereComponent.Self:
                            if (poolArgument.Type == typeof(GameObject)) break;
                            else 
                            {
                                foreach (FieldInfo fieldInfo in typeof(PoolObject).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                                {
                                    if (fieldInfo.FieldType == poolArgument.Type)
                                    {
                                        fieldInfo.SetValue(pool[i], pool[i].GameObject.GetComponent(poolArgument.Type));
                                    }
                                }
                            }
                            
                            break;
                    }
                }

                pool[i].GameObject.SetActive(false);
            }
        }
    }

    public PoolObject PickOne()
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

    public PoolObject PickOneWithoutActive()
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

    public List<PoolObject> Pick(int n)
    {
        int count = 0;
        List<PoolObject> poolObjects = new List<PoolObject>();
        for (int i=0;i<pool.Count;i++)
        {
            if (count >= n) return poolObjects;

            if (!pool[i].GameObject.activeSelf)
            {
                pool[i].GameObject.SetActive(true);
                poolObjects.Add(pool[i]);
                count++;
            }
        }

        return null;
    }

    public List<PoolObject> PickNWithoutActive(int n)
    {
        int count = 0;
        List<PoolObject> poolObjects = new List<PoolObject>();
        for (int i=0;i<pool.Count;i++)
        {
            if (count >= n) return poolObjects;

            if (!pool[i].GameObject.activeSelf)
            {
                poolObjects.Add(pool[i]);
                count++;
            }
        }

        return null;
    }
}

public class PoolArgument
{
    private Type type;
    public enum WhereComponent {Child, Self}
    public WhereComponent whereComponent;

    public Type Type { get => type; set => type = value; }

    public PoolArgument(Type type, WhereComponent whereComponent)
    {
        this.type = type;
        this.whereComponent = whereComponent;
    }
}

public class PoolObject
{
    private GameObject gameObject;
    private Weapon weapon;
    private GameEffect gameEffect;
    private SwordWeapon swordWeapon;
    private SkeletonSwordWeapon skeletonSwordWeapon;
    public GameObject GameObject { get => gameObject; set => gameObject = value; }
    public Weapon Weapon { get => weapon; set => weapon = value; }
    public GameEffect GameEffect { get => gameEffect; set => gameEffect = value; }
    public SkeletonSwordWeapon SkeletonSwordWeapon { get => skeletonSwordWeapon; set => skeletonSwordWeapon = value; }
    public SwordWeapon SwordWeapon { get => swordWeapon; set => swordWeapon = value; }
}