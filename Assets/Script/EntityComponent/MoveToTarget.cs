using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotatableObject))]
public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Animator animator;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float distanceToStopMove;
    private RotatableObject rotatableObject;
    public bool CanMove { get => canMove; set => canMove = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value * 0.03f; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float DistanceToStopMove { get => distanceToStopMove; set => distanceToStopMove = value; }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 1f;
        distanceToStopMove = 1f;
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        rotatableObject = GetComponent<RotatableObject>();
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private Vector3 funcMove_DistanceVector;
    public void Move()
    {
        funcMove_DistanceVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        distanceToTarget = funcMove_DistanceVector.magnitude;
        if (canMove)
        {
            if (distanceToTarget > distanceToStopMove)
            {
                transform.position += funcMove_DistanceVector.normalized * moveSpeed;

                rotatableObject.RotateToDirectionAxisXZ(funcMove_DistanceVector);
                Debug.DrawRay(transform.position, funcMove_DistanceVector, Color.red);

                animator.SetBool("Move", true);
            }
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
