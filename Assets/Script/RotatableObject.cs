using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RotatableObject
{
    [Header("Rotation")]
    [SerializeField] private float rotateAmountAbs = 10f;
    [SerializeField] private float rotateAmount;
    [SerializeField] private float curentAngle = 0;
    [SerializeField] private float toAngle;
    [SerializeField] private float prevToAngle = 0;
    [SerializeField] private enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    [SerializeField] private directionEnum rotateDirection;
    [SerializeField] private float moveAngle;
    [SerializeField] private float optimalMoveAngle;
    [SerializeField] private float movedAngle = 0;
    [SerializeField] private Transform objectTransform;
    [SerializeField] private bool finish = false;

    public float RotateAmountAbs { get => rotateAmountAbs; set => rotateAmountAbs = value; }
    public float RotateAmount { get => rotateAmount; set => rotateAmount = value; }
    public float CurentAngle { get => curentAngle; set => curentAngle = value; }
    public float ToAngle { get => toAngle; set => toAngle = value; }
    public float PrevToAngle { get => prevToAngle; set => prevToAngle = value; }
    private directionEnum RotateDirection { get => rotateDirection; set => rotateDirection = value; }
    public float MoveAngle { get => moveAngle; set => moveAngle = value; }
    public float OptimalMoveAngle { get => optimalMoveAngle; set => optimalMoveAngle = value; }
    public float MovedAngle { get => movedAngle; set => movedAngle = value; }
    public Transform ObjectTransform { get => objectTransform; set => objectTransform = value; }
    public bool Finish { get => finish; set => finish = value; }

    public RotatableObject(Transform objectTransform)
    {
        this.ObjectTransform = objectTransform;
    }

    public void RotateToDirectionAxisXZ(Vector3 directionVector)
    {
        #region Handle Rotaion
        ToAngle = UtilObject.Instance.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        if (ToAngle != PrevToAngle)
        {
            MoveAngle = Math.Abs(ToAngle - CurentAngle);
            RotateDirection = ToAngle >= CurentAngle ? directionEnum.Clockwise : directionEnum.CounterClockwise;
            OptimalMoveAngle = 360 - MoveAngle;
            if (OptimalMoveAngle < MoveAngle)
            {
                RotateDirection = RotateDirection == directionEnum.Clockwise ? directionEnum.CounterClockwise : directionEnum.Clockwise;
                MoveAngle = OptimalMoveAngle;
            }
            RotateAmount = (int)RotateDirection * RotateAmountAbs;
            MovedAngle = 0;
        }
        PrevToAngle = ToAngle;

        if (MovedAngle < MoveAngle)
        {
            Finish = false;
            ObjectTransform.Rotate(new Vector3(0, RotateAmount, 0));
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
        // objectTransform.rotation = Quaternion.RotateTowards(objectTransform.rotation, Quaternion.LookRotation(directionVector, Vector3.up), rotateAmountAbs * 100);
        #endregion
    }

    public void RotateToAngleAxisXZImediatly(float angle)
    {
        objectTransform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        toAngle = angle;
        curentAngle = angle;
    }

    public void RotateToAngleAxisXZImediatly(Vector3 directionVector)
    {
        toAngle = UtilObject.Instance.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        curentAngle = UtilObject.Instance.GetPositiveAngle(toAngle);
        objectTransform.rotation = Quaternion.Euler(0, curentAngle, 0);
    }
}
