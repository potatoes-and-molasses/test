using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class DogMovement : MonoBehaviour
{
    enum State {WalksToOwner, WalksToRandomPoint, RichedRandomPoint , RichedOwner };
    public Transform owner;
    public float lishLength = 5;
    public float minDistanceFromOwner = 1;
    public float acceleration = 5;
    public float deceleration = 8;
    public float speed = 6;
    public float turnSpeed = 10;
    public LineRenderer lish;
    public Color32 lishColor;

    private Vector2 velocity;
    private float currentSpeed;
    private bool goToOwner;
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private State state = State.RichedRandomPoint;
    private float timeNearTarget = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lish = GetComponent<LineRenderer>();
        lish.startWidth = 0.1f;
        lish.endWidth = 0.1f;
        lish.startColor = lishColor;
        lish.endColor = lishColor;
        lish.sortingOrder = -1;
        GenerateNewTarget();
        speed = Random.Range(4, 10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var distFromOwner = Vector3.Distance(transform.position, owner.position);
        var distFromTarget = Vector3.Distance(transform.position, targetPosition);
        DogsLogic(distFromOwner, distFromTarget);
        lish.SetPositions(new Vector3[] { transform.position, owner.position });
    }


    void DogsLogic(float distanceToOwner, float distanceToTargrt)
    {
        switch (state)
        {
            case State.WalksToOwner:
                if(distanceToOwner <= minDistanceFromOwner)
                {
                    state = State.RichedOwner;
                    return;
                }
                UpdateMovement(distanceToTargrt);
                return;
            case State.WalksToRandomPoint:
                if (distanceToOwner > lishLength)
                {
                    state = State.WalksToOwner;
                    return;
                }
                if(distanceToTargrt < 0.3f)
                {
                    state = State.RichedRandomPoint;
                    timeNearTarget = 0;
                    return;
                }
                UpdateMovement(distanceToTargrt);
                return;
            case State.RichedRandomPoint:
                if(timeNearTarget >= 1)
                {
                    state = State.WalksToRandomPoint;
                    GenerateNewTarget();
                    return;
                }
                timeNearTarget += Time.fixedDeltaTime;
                return;
            case State.RichedOwner:
                GenerateNewTarget();
                state = State.WalksToRandomPoint;
                return;
        }
    }

    void UpdateMovement(float distFromTarget)
    {
        UpdateSpeed(distFromTarget);
        CalculateVelocity();
        LookAtTarget();
        Move(distFromTarget);
        
    }
    void UpdateSpeed(float dist)
    {
        if (dist < 0.5f)
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0.3f, speed);
        else
            currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.fixedDeltaTime, 0, currentSpeed);   
    }

    void GenerateNewTarget()
    {        
        var length = Random.Range(minDistanceFromOwner, lishLength);
        var angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 newTarget = owner.position + new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, 0);
        while(Vector3.Distance(newTarget, targetPosition) < minDistanceFromOwner/2)
        {
            length = Random.Range(minDistanceFromOwner, lishLength);
            angle = Random.Range(0, 2 * Mathf.PI);
            newTarget = owner.position + new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, 0);
        }
        targetPosition = newTarget;
    }

    void CalculateVelocity()
    {
        if(state == State.WalksToOwner)
            velocity = (owner.position - transform.position).normalized;
        else
            velocity = (targetPosition - transform.position).normalized;
    }
    void LookAtTarget()
    {
        var angle = (velocity.y >= 0) ? Mathf.Acos(velocity.x) * Mathf.Rad2Deg : -Mathf.Acos(velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime);
    }
    void Move(float dist)
    {
        var newPos = new Vector3(velocity.x, velocity.y, 0);
        if(state == State.WalksToOwner)
            rb.MovePosition(transform.position + newPos * speed * Time.fixedDeltaTime);
        else if(dist > 0.3)
            rb.MovePosition(transform.position + newPos * (speed/2) * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(targetPosition, 1);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(owner.position, minDistanceFromOwner);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(owner.position, lishLength);
    }
}
