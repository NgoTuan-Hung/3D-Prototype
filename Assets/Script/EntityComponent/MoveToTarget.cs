using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : EntityAction
{
    [SerializeField] private Transform target;
    private CustomMonoBehavior customMonoBehavior;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float distanceToStopMove;
    public bool CanMove { get => canMove; set => canMove = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DistanceToTarget { get => distanceToTarget; set => distanceToTarget = value; }
    public float DistanceToStopMove { get => distanceToStopMove; set => distanceToStopMove = value; }
    public Transform Target { get => target; set => target = value; }

    // Start is called before the first frame update
    void Awake()
    {
        MoveSpeed = 1f;
        distanceToStopMove = 1f;
        target = GameObject.Find("Player").transform;
    }

    void Start()
    {
        IfDo(customMonoBehavior.RotatableObjectBool, RotateToTarget());
        IfDo(customMonoBehavior.AnimatorBool && customMonoBehavior.RigidbodyBool, Move());
        // one thing to note about this is that the order of execution is hard to control
        // so if we want to have an order of execution in some case, we can always use update or fixedupdate
    }

    private void FixedUpdate() 
    {
        CalculateDistanceVector();
    }

    private Vector3 funcMove_DistanceVector = Vector3.zero;

    public void CalculateDistanceVector()
    {
        funcMove_DistanceVector = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
    }

    public IEnumerator RotateToTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            customMonoBehavior.RotatableObject.RotateToDirectionAxisXZ(funcMove_DistanceVector);
            Debug.DrawRay(transform.position, funcMove_DistanceVector, Color.red);
        }
    }

    public IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);

            distanceToTarget = funcMove_DistanceVector.magnitude;
            if (canMove)
            {
                if (distanceToTarget > distanceToStopMove)
                {
                    customMonoBehavior.Rigidbody.velocity = funcMove_DistanceVector.normalized * moveSpeed;

                    customMonoBehavior.Animator.SetBool("Move", true);
                }
            }
            else
            {
                customMonoBehavior.Animator.SetBool("Move", false);
            }
        }
    }
}
