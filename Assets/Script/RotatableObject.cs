using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RotatableObject : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotateAmountAbsY = 10f;
    [SerializeField] private float rotateAmountY;
    [SerializeField] private float curentAngleY = 0;
    [SerializeField] private float toAngleY;
    [SerializeField] private float prevToAngleY = 0;
    [SerializeField] private enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    [SerializeField] private float moveAngleY;
    [SerializeField] private float movedAngleY = 0;
    [SerializeField] private bool finishY = false;
    private UtilObject utilObject = new UtilObject();
    private object[][] returnValue;

    public float RotateAmountAbsY { get => rotateAmountAbsY; set => rotateAmountAbsY = value; }
    public float RotateAmountY { get => rotateAmountY; set => rotateAmountY = value; }
    public float CurentAngleY { get => curentAngleY; set => curentAngleY = value; }
    public float ToAngleY { get => toAngleY; set => toAngleY = value; }
    public float PrevToAngleY { get => prevToAngleY; set => prevToAngleY = value; }
    public float MoveAngleY { get => moveAngleY; set => moveAngleY = value; }
    public float MovedAngleY { get => movedAngleY; set => movedAngleY = value; }
    public bool FinishY { get => finishY; set => finishY = value; }

    public float GetMoveAngle(float toAngle)
    {
        return Math.Abs(toAngle - curentAngle);
    }

    public object[][] GetOptimalRotateDirectionAndMoveAngle(float toAngle)
    {
        object[][] gORDAMA_returnValue = new object[1][];
        gORDAMA_returnValue[0] = new object[2];

        float gORDAMA_moveAngle;
        directionEnum gORDAMA_rotateDirection;
        float gORDAMA_optimalMoveAngle;

        gORDAMA_moveAngle = Math.Abs(toAngle - curentAngle);
        gORDAMA_rotateDirection = toAngle >= curentAngle ? directionEnum.Clockwise : directionEnum.CounterClockwise;
        gORDAMA_optimalMoveAngle = 360 - gORDAMA_moveAngle;
        if (gORDAMA_optimalMoveAngle < gORDAMA_moveAngle)
        {
            gORDAMA_rotateDirection = gORDAMA_rotateDirection == directionEnum.Clockwise ? directionEnum.CounterClockwise : directionEnum.Clockwise;
            gORDAMA_moveAngle = gORDAMA_optimalMoveAngle;
        }

        gORDAMA_returnValue[0][0] = gORDAMA_rotateDirection;
        gORDAMA_returnValue[0][1] = gORDAMA_moveAngle;

        return gORDAMA_returnValue;
    }

    public void RotateY(Vector2 directionVector)
    {
        directionVector = new Vector3(directionVector.x, 0, directionVector.y);
        RotateY(directionVector);
    }

    public void RotateY(Vector3 directionVector)
    {
        #region Handle Rotaion
        toAngleY = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        if (toAngleY != prevToAngleY)
        {
            returnValue = GetOptimalRotateDirectionAndMoveAngle(toAngleY);
            moveAngleY = (float)returnValue[0][1];
            rotateAmountY = (int)returnValue[0][0] * rotateAmountAbsY;
            movedAngleY = 0;
        }
        prevToAngleY = toAngleY;

        if (movedAngleY < moveAngleY)
        {
            finishY = false;
            transform.Rotate(new Vector3(0, rotateAmountY, 0));
            curentAngleY += rotateAmountY;
            if (curentAngleY < 0) curentAngleY = 360 + curentAngleY;
            else if (curentAngleY > 360) curentAngleY %= 360;
            movedAngleY += rotateAmountAbsY;
        }
        else
        {
            finishY = true;
        }
        #endregion

        #region Or Simplier Approach
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionVector, Vector3.up), rotateAmountAbs * 100);
        #endregion
    }

    public void RotateToAngleAxisXZImediatly(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        toAngle = angle;
        curentAngle = angle;
    }

    public void RotateToAngleAxisXZImediatly(Vector3 directionVector)
    {
        toAngle = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        curentAngle = utilObject.GetPositiveAngle(toAngle);
        transform.rotation = Quaternion.Euler(0, curentAngle, 0);
    }
}
