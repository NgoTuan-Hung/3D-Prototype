using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;

public class UtilObject
{
    private float calculateAngleBase360_angle;
    public void RotateByAmount(Transform transform, float x, float y, float z = 0f)
    {
        transform.Rotate(x, y, z);
    }

    public void BackWardByAmount(Transform transform, Vector3 amount)
    {
        transform.position = transform.TransformPoint(amount);
    }

    public float CalculateAngleBase360(Vector3 from, Vector3 to, Vector3 axis)
    {
        calculateAngleBase360_angle = Vector3.SignedAngle(from, to, axis);
        calculateAngleBase360_angle = GetPositiveAngle(calculateAngleBase360_angle);
        return calculateAngleBase360_angle;
    }

    public float CalculateAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        calculateAngleBase360_angle = Vector3.SignedAngle(from, to, axis);
        return calculateAngleBase360_angle;
    }

    public float GetPositiveAngle(float angle)
    {
        return angle < 0 ? 360 + angle : angle;
    }

    public EntityData LoadEntityData(string name)
    {
        return GlobalObject.Instance.entityDatas.First((entityData) => entityData.entityName.Equals(name));
    }

    public CustomMonoBehavior CustomMonoBehaviorBinarySearch(List<CustomMonoBehavior> arr, int target)
    {
        int low = 0;
        int high = arr.Count - 1;
 
        while (low <= high)
        {
            int mid = (low + high) / 2;
            CustomMonoBehavior guess = arr[mid];
 
            if (guess.gameObject.GetInstanceID() == target)
            {
                // Return age if the name matches the target
                return guess;
            }
            else if (guess.gameObject.GetInstanceID() > target)
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }
        }
 
        return null; // Not found
    }

    public void BindKey(PlayerInputSystem playerInputSystem, String key, String method, Type classType, object functionOwner)
    {
        #region Binding key at playtime
        InputAction inputAction = playerInputSystem.Control.Get().asset.FindAction(key);
        MethodInfo methodInfo = classType.GetMethod(method);
        inputAction.performed += (Action<InputAction.CallbackContext>)
        Delegate.CreateDelegate(typeof(Action<InputAction.CallbackContext>), functionOwner, methodInfo);
        #endregion
    }
}

public class Node<T>
{
    public T Data;
    public Node<T> Left;
    public Node<T> Right;

    public Node(T data)
    {
        Data = data;
        Left = null;
        Right = null;
    }
}

public class BinarySearchTree<T> where T : IComparable<T>
{
    private Node<T> root;
    internal Node<T> Root { get => root; set => root = value; }

    public BinarySearchTree()
    {
        root = null;
    }

    public void Insert(T data)
    {
        InsertRecursive(ref root, data);
    }


    private void InsertRecursive(ref Node<T> current, T data)
    {
        current ??= new Node<T>(data);

        if (data.CompareTo(current.Data) < 0)
        {
            InsertRecursive(ref current.Left, data);
        }
        else if (data.CompareTo(current.Data) > 0)
        {
            InsertRecursive(ref current.Right, data);
        }
    }

    public T Search(Func<T, int> getKey, int key)
    {
        return SearchRecursive(root, getKey, key);
    }

    // Recursive method to search for a node based on specific attribute
    private T SearchRecursive(Node<T> current, Func<T, int> getKey, int key)
    {
        if (current == null) return default;

        if (getKey(current.Data) == key)
        {
            return current.Data;
        }
        else if (key < getKey(current.Data))
        {
            return SearchRecursive(current.Left, getKey, key);
        }
        else
        {
            return SearchRecursive(current.Right, getKey, key);
        }
    }

    public void Travel(Action<T> action)
    {
        TravelRecursive(root, action);
    }
    private void TravelRecursive(Node<T> current, Action<T> action)
    {
        if (current != null)
        {
            action(current.Data);
            TravelRecursive(current.Left, action);
            TravelRecursive(current.Right, action);
        }
    }
}


