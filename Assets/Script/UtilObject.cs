using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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

    public CombatEntityInfo CombatEntityInfoBinarySearch(List<CombatEntityInfo> arr, int target)
    {
        int low = 0;
        int high = arr.Count - 1;
 
        while (low <= high)
        {
            int mid = (low + high) / 2;
            CombatEntityInfo guess = arr[mid];
 
            if (guess.GameObject.GetInstanceID() == target)
            {
                // Return age if the name matches the target
                return guess;
            }
            else if (guess.GameObject.GetInstanceID() > target)
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
}
