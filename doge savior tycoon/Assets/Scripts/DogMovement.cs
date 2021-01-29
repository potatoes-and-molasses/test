using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class DogMovement : MonoBehaviour
{
    enum State {WalksToOwner, WalksToRandomPoint, RichedRandomPoint , RichedOwner };
    public Movable owner;
    public float leashLength = 5;
    public float minDistanceFromOwner = 1;
    public float acceleration = 5;
    public float deceleration = 8;
    public float speed = 6;
    public float turnSpeed = 10;
    public LineRenderer leash;
    public Color32 leashColor;
    public LayerMask layerMask;
    public Transform hand;

    private Vector2 velocity;
    private float currentSpeed;
    private bool goToOwner;
    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private State state = State.RichedRandomPoint;
    private float timeNearTarget = 0;
    private float wait = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leash = GetComponent<LineRenderer>();
        leash.startWidth = 0.1f;
        leash.endWidth = 0.1f;
        leash.startColor = leashColor;
        leash.endColor = leashColor;
        leash.sortingOrder = -1;
        GenerateNewTarget();
        speed = Random.Range(4, 6);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var distFromOwner = Vector3.Distance(transform.position, owner.transform.position);
        var distFromTarget = Vector3.Distance(transform.position, targetPosition);
        DogsLogic(distFromOwner, distFromTarget);
        leash.SetPositions(new Vector3[] {transform.position, hand.position});
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
                if (distanceToOwner > leashLength)
                {
                    state = State.WalksToOwner;
                    return;
                }
                if(distanceToTargrt < 0.5f)
                {
                    state = State.RichedRandomPoint;
                    wait = Random.Range(0.6f, 1.4f);
                    timeNearTarget = 0;
                    return;
                }
                UpdateMovement(distanceToTargrt);
                return;
            case State.RichedRandomPoint:
                if (distanceToOwner > leashLength)
                {
                    state = State.WalksToOwner;
                    return;
                }
                if(timeNearTarget >= wait)
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
        var length = Random.Range(minDistanceFromOwner, leashLength);
        var angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 newTarget = owner.transform.position + new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, 0);
        
        
        while(Vector3.Distance(newTarget, transform.position) < minDistanceFromOwner/2)
        {
            length = Random.Range(minDistanceFromOwner, leashLength);
            angle = Random.Range(0, 2 * Mathf.PI);
            newTarget = owner.transform.position + new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, 0);
        }
        //if(Physics.Raycast(transform.position, dir, out hit, leashLength, layerMask))
        //{
         //   newTarget = hit.point;
        //}
        targetPosition = newTarget;
        var dir = (newTarget - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1000, layerMask);
        if (hit.collider != null)
        {
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, 0);
            float dist = Vector3.Distance(targetPosition, hitPoint);
            targetPosition -= dir * (dist + 0.3f);
            
        }
    }

    void OnTargetCollision(Vector3 direction, RaycastHit2D hit)
    {

    }

    void CalculateVelocity()
    {
        if(state == State.WalksToOwner)
            velocity = (owner.transform.position - transform.position).normalized;
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
        /*var newPos = new Vector3(velocity.x, velocity.y, 0);
        if(state == State.WalksToOwner)
            rb.MovePosition(transform.position + newPos * speed * Time.fixedDeltaTime);
        else if(dist > 0.3)
            rb.MovePosition(transform.position + newPos * (speed/2) * Time.fixedDeltaTime);*/

        var newPos = transform.right;
        if (state == State.WalksToOwner)
            rb.MovePosition(transform.position + newPos * owner.speed * Time.fixedDeltaTime);
        else if (dist > 0.3)
            rb.MovePosition(transform.position + newPos * speed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (owner == null)
            return;
        Gizmos.DrawSphere(targetPosition, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(owner.transform.position, minDistanceFromOwner);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(owner.transform.position, leashLength);
        Gizmos.color = Color.red;
    }
}
