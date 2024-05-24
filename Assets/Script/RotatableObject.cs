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
    [SerializeField] private float toAngleY;
    public enum directionEnum {Clockwise = 1, CounterClockwise = -1}
    [SerializeField] private float moveAngleY;
    private UtilObject utilObject = new UtilObject();
    private GetOptimalRotateDirectionAndMoveAngleClass returnValue;

    public float RotateAmountAbsY { get => rotateAmountAbsY; set => rotateAmountAbsY = value; }
    public float RotateAmountY { get => rotateAmountY; set => rotateAmountY = value; }
    public float ToAngleY { get => toAngleY; set => toAngleY = value; }
    public float MoveAngleY { get => moveAngleY; set => moveAngleY = value; }
    public GetOptimalRotateDirectionAndMoveAngleClass GetOptimalRotateDirectionAndMoveAngleY(float toAngle)
    {
        GetOptimalRotateDirectionAndMoveAngleClass gORDAMAC = new GetOptimalRotateDirectionAndMoveAngleClass
        {
            RotateDirection = toAngle >= transform.rotation.eulerAngles.y ? directionEnum.Clockwise : directionEnum.CounterClockwise,
            MoveAngle = Math.Abs(toAngle - transform.rotation.eulerAngles.y)
        };

        if (gORDAMAC.MoveAngle > 360 - gORDAMAC.MoveAngle)
        {
            gORDAMAC.RotateDirection = gORDAMAC.RotateDirection == directionEnum.Clockwise ? directionEnum.CounterClockwise : directionEnum.Clockwise;
        }

        return gORDAMAC;
    }

    public void Awake()
    {
        epsilon = rotateAmountAbsY * 2/3;
    }

    public void RotateY(Vector2 directionVector)
    {
        RotateY(new Vector3(directionVector.x, 0, directionVector.y));
    }

    float epsilon;
    public void RotateY(Vector3 directionVector)
    {
        #region Handle Rotaion
        toAngleY = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);

        returnValue = GetOptimalRotateDirectionAndMoveAngleY(toAngleY);
        moveAngleY = returnValue.MoveAngle;
        rotateAmountY = (int)returnValue.RotateDirection * rotateAmountAbsY;

        if (moveAngleY > epsilon) transform.Rotate(new Vector3(0, rotateAmountY, 0));
        #endregion

        #region Or Simplier Approach
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionVector, Vector3.up), rotateAmountAbs * 100);
        #endregion
    }

    public void RotateYImediatly(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        toAngleY = angle;
    }

    public void RotateYImediatly(Vector3 directionVector)
    {
        toAngleY = utilObject.CalculateAngleBase360(Vector3.forward, directionVector, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public class GetOptimalRotateDirectionAndMoveAngleClass
    {
        private float moveAngle;
        private directionEnum rotateDirection;

        public float MoveAngle { get => moveAngle; set => moveAngle = value; }
        public directionEnum RotateDirection { get => rotateDirection; set => rotateDirection = value; }
    }
}
