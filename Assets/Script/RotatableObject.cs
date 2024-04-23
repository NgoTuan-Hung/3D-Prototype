using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RotatableObject : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotateAmountAbs = 10f;
    [SerializeField] private float rotateAmount;
    [SerializeField] private float curentAngle = 0;
    [SerializeField] private float toAngle;
    [SerializeField] private float prevToAngle = 0;
    [SerializeField] private enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    [SerializeField] private float moveAngle;
    [SerializeField] private float movedAngle = 0;
    [SerializeField] private bool finish = false;
    private UtilObject utilObject = new UtilObject();
    private object[][] returnValue;

    public float RotateAmountAbs { get => rotateAmountAbs; set => rotateAmountAbs = value; }
    public float RotateAmount { get => rotateAmount; set => rotateAmount = value; }
    public float CurentAngle { get => curentAngle; set => curentAngle = value; }
    public float ToAngle { get => toAngle; set => toAngle = value; }
    public float PrevToAngle { get => prevToAngle; set => prevToAngle = value; }
    public float MoveAngle { get => moveAngle; set => moveAngle = value; }
    public float MovedAngle { get => movedAngle; set => movedAngle = value; }
    public bool Finish { get => finish; set => finish = value; }


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

    public void RotateToDirectionAxisXZ(Vector3 directionVector)
    {
        RotateToDirectionAxisXZBase(directionVector);
    }

    public void RotateToDirectionAxisXZ(Vector2 directionVector)
    {
        directionVector = new Vector3(directionVector.x, 0, directionVector.y);
        RotateToDirectionAxisXZBase(directionVector);
    }

    public void RotateToDirectionAxisXZBase(Vector3 directionVector)
    {
        #region Handle Rotaion
        ToAngle = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        if (ToAngle != PrevToAngle)
        {
            returnValue = GetOptimalRotateDirectionAndMoveAngle(ToAngle);
            MoveAngle = (float)returnValue[0][1];
            RotateAmount = (int)returnValue[0][0] * RotateAmountAbs;
            MovedAngle = 0;
        }
        PrevToAngle = ToAngle;

        if (MovedAngle < MoveAngle)
        {
            Finish = false;
            transform.Rotate(new Vector3(0, RotateAmount, 0));
            CurentAngle += RotateAmount;
            if (CurentAngle < 0) CurentAngle = 360 + CurentAngle;
            else if (CurentAngle > 360) CurentAngle %= 360;
            MovedAngle += RotateAmountAbs;
        }
        else
        {
            Finish = true;
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
