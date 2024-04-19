using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

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
