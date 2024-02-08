using System.Collections;
using System.Collections.Generic;
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
        calculateAngleBase360_angle = calculateAngleBase360_angle < 0 ? 360 + calculateAngleBase360_angle : calculateAngleBase360_angle;
        return calculateAngleBase360_angle;
    }
}
