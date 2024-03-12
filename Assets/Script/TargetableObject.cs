using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    [SerializeField] private bool isTarget = false;
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private GameObject bodyAimSourceObjectOriginalBodyPoint;
    [SerializeField] private GameObject bodyAimSourceObjectFirstRotate;
    [SerializeField] private GameObject bodyRotationSourceObject;
    [SerializeField] private TargetChecker targetChecker;

    private void Start() 
    {
        GameObject bodyAimSourceObjectRoot = transform.Find("BodyAimSourceObjectRoot").gameObject;
        bodyRotationSourceObject = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObject").gameObject;
        bodyAimSourceObjectOriginalBodyPoint = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObjectOriginalBodyPoint").gameObject;
        bodyAimSourceObjectFirstRotate = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObjectFirstRotate").gameObject;
        targetChecker = Instantiate(Resources.Load("TargetChecker"), transform).GetComponent<TargetChecker>();
        targetChecker.transform.position = Vector3.zero;
    }

    public void Target()
    {
        if (!isTarget)
        {
            isTarget = true;
            //animator.SetBool("Target", true);
            //cameraDelegate?.Invoke(currentTarget);
            StartCoroutine(TargetHandler());
            
            // var weightedTransformArray = multiAimConstraintData.sourceObjects;
            // weightedTransformArray.SetTransform(0, currentTarget.transform);
            // multiAimConstraintData.sourceObjects = weightedTransformArray;
            // multiAimConstraint.data = multiAimConstraintData;
            // rigBuilder.Build();
        }
        else
        {
            isTarget = false;
            //cameraDelegate?.Invoke(null);
            //animator.SetBool("Target", false);
        }
    }

    [SerializeField] private Vector3 targetHandler_direction;
    [SerializeField] private Vector3 targetHandler_angle;
    [SerializeField] private Vector3 targetHandler_tempRotation;

    public IEnumerator TargetHandler()
    {
        while (isTarget)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            targetHandler_direction = targetChecker.nearestTarget.transform.position - bodyRotationSourceObject.transform.position;
            targetHandler_angle =  Quaternion.LookRotation(targetHandler_direction, 
            Vector3.Cross(targetHandler_direction,
             bodyAimSourceObjectOriginalBodyPoint.transform.TransformPoint(Vector3.forward))).eulerAngles;

            bodyAimSourceObjectFirstRotate.transform.rotation = Quaternion.Euler(targetHandler_angle);
            targetHandler_tempRotation = bodyAimSourceObjectFirstRotate.transform.localRotation.eulerAngles;

            targetHandler_tempRotation = new Vector3
            (
                targetHandler_tempRotation.x > 180 ? targetHandler_tempRotation.x - 360 : targetHandler_tempRotation.x,
                targetHandler_tempRotation.y > 180 ? targetHandler_tempRotation.y - 360 : targetHandler_tempRotation.y,
                0
            );

            bodyRotationSourceObject.transform.localRotation = Quaternion.Euler
            (
                Math.Clamp(targetHandler_tempRotation.x, -30f, 30f)
                , Math.Clamp(targetHandler_tempRotation.y, -90f, 90f)
                , 0
            );
        }
    }
}
