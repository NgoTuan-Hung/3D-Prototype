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

    public bool IsTarget { get => isTarget; set => isTarget = value; }
    public GameObject CurrentTarget { get => currentTarget; set => currentTarget = value; }
    public GameObject BodyAimSourceObjectOriginalBodyPoint { get => bodyAimSourceObjectOriginalBodyPoint; set => bodyAimSourceObjectOriginalBodyPoint = value; }
    public GameObject BodyAimSourceObjectFirstRotate { get => bodyAimSourceObjectFirstRotate; set => bodyAimSourceObjectFirstRotate = value; }
    public GameObject BodyRotationSourceObject { get => bodyRotationSourceObject; set => bodyRotationSourceObject = value; }
    public TargetChecker TargetChecker { get => targetChecker; set => targetChecker = value; }
    private void Start() 
    {
        GameObject bodyAimSourceObjectRoot = transform.Find("BodyAimSourceObjectRoot").gameObject;
        bodyRotationSourceObject = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObject").gameObject;
        bodyAimSourceObjectOriginalBodyPoint = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObjectOriginalBodyPoint").gameObject;
        bodyAimSourceObjectFirstRotate = bodyAimSourceObjectRoot.transform.Find("BodyAimSourceObjectFirstRotate").gameObject;
        targetChecker = Instantiate(Resources.Load("TargetChecker"), transform).GetComponent<TargetChecker>();
        targetChecker.transform.position = transform.position;
    }

    public void Target()
    {
        isTarget = true;
        StartCoroutine(TargetHandler());
    }

    [SerializeField] private Vector3 targetHandler_direction;
    [SerializeField] private Vector3 targetHandler_angle;
    [SerializeField] private Vector3 targetHandler_tempRotation;

    public IEnumerator TargetHandler()
    {
        while (isTarget)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            targetHandler_direction = targetChecker.NearestTarget.transform.position - bodyRotationSourceObject.transform.position;
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

    public void Reset()
    {
        isTarget = false;
        bodyRotationSourceObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
