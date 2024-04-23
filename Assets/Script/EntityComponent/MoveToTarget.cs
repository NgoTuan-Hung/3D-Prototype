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
    new private Rigidbody rigidbody;
    private RotatableObject rotatableObject;
    public bool CanMove { get => canMove; set => canMove = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float DistanceToStopMove { get => distanceToStopMove; set => distanceToStopMove = value; }
    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 1f;
        distanceToStopMove = 1f;
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        rotatableObject = GetComponent<RotatableObject>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        CalculateDistanceVector();
        RotateToTarget();
        Move();
    }

    private Vector3 funcMove_DistanceVector;

    public void CalculateDistanceVector()
    {
        funcMove_DistanceVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
    }

    public void RotateToTarget()
    {
        rotatableObject.RotateToDirectionAxisXZ(funcMove_DistanceVector);
        Debug.DrawRay(transform.position, funcMove_DistanceVector, Color.red);
    }

    public void Move()
    {
        
        distanceToTarget = funcMove_DistanceVector.magnitude;
        if (canMove)
        {
            if (distanceToTarget > distanceToStopMove)
            {
                rigidbody.velocity = funcMove_DistanceVector.normalized * moveSpeed;

                animator.SetBool("Move", true);
            }
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
