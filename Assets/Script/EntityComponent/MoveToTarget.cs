using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Animator animator;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    public bool CanMove { get => canMove; set => canMove = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value * 0.03f; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value * 0.03f; }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 1f;
        RotationSpeed = 1f;
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate() 
    {
        Move();
    }

    private Vector3 funcMove_DistanceVector;
    public void Move()
    {
        if (canMove)
        {
            funcMove_DistanceVector = target.transform.position - transform.position;
            transform.position += funcMove_DistanceVector.normalized * moveSpeed;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, funcMove_DistanceVector, rotationSpeed, 0.0f);
            Debug.DrawRay(transform.position, newDirection, Color.red);
            transform.rotation = Quaternion.LookRotation(newDirection);

            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
