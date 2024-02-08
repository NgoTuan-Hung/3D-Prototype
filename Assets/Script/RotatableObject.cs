using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RotatableObject
{
    [Header("Rotation")]
    [SerializeField] protected float rotateAmountAbs = 3f;
    [SerializeField] protected float rotateAmount;
    [SerializeField] protected float curentAngle = 0;
    [SerializeField] protected float toAngle;
    [SerializeField] protected float prevToAngle = 0;
    [SerializeField] protected enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    [SerializeField] protected directionEnum rotateDirection;
    [SerializeField] protected float moveAngle;
    [SerializeField] protected float optimalMoveAngle;
    [SerializeField] protected float movedAngle = 0;
    [SerializeField] protected Transform objectTransform;

    public RotatableObject(Transform objectTransform)
    {
        this.objectTransform = objectTransform;
    }

    public void RotateToDirection(UtilObject utilObject, Vector3 directionVector)
    {
        #region Handle Rotaion
        toAngle = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        if (toAngle != prevToAngle)
        {
            moveAngle = Math.Abs(toAngle - curentAngle);
            rotateDirection = toAngle >= curentAngle ? directionEnum.Clockwise : directionEnum.CounterClockwise;
            optimalMoveAngle = 360 - moveAngle;
            if (optimalMoveAngle < moveAngle)
            {
                rotateDirection = rotateDirection == directionEnum.Clockwise ? directionEnum.CounterClockwise : directionEnum.Clockwise;
                moveAngle = optimalMoveAngle;
            }
            rotateAmount = (int)rotateDirection * rotateAmountAbs;
            movedAngle = 0;
        }
        prevToAngle = toAngle;

        if (movedAngle < moveAngle)
        {
            objectTransform.Rotate(new Vector3(0, rotateAmount, 0));
            curentAngle += rotateAmount;
            if (curentAngle < 0) curentAngle = 360 + curentAngle;
            else if (curentAngle > 360) curentAngle %= 360;
            movedAngle += rotateAmountAbs;
        }
        #endregion

        #region Or Simplier Approach
        // objectTransform.rotation = Quaternion.RotateTowards(objectTransform.rotation, Quaternion.LookRotation(directionVector, Vector3.up), rotateAmountAbs * 100);
        #endregion
    }
}
