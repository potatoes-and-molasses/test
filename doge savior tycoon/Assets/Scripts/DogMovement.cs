using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class DogMovement : MonoBehaviour
{
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
    private bool isMoving;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lish = GetComponent<LineRenderer>();
        lish.startWidth = 0.1f;
        lish.endWidth = 0.1f;
        lish.startColor = lishColor;
        lish.endColor = lishColor;
        lish.sortingOrder = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, owner.position);

        if(dist <= lishLength)
            isMoving = false;
        else if(!isMoving && dist >= lishLength)
            isMoving = true;
        UpdateSpeed(dist);
        LookAtOwner();
        CalculateVelocity();
        Move(dist);
        lish.SetPositions(new Vector3[] { transform.position, owner.position });
    }

    void UpdateSpeed(float dist)
    {
        if (isMoving)
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0.3f, speed);
        else
        {
            if(dist < minDistanceFromOwner)
                currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.fixedDeltaTime, 0, currentSpeed);
        }
        
    }

    void CalculateVelocity()
    {
        velocity = (owner.position - transform.position).normalized;
    }
    void LookAtOwner()
    {

        var lookVector = (owner.position - transform.position).normalized;
        
        var angle = (lookVector.y >= 0) ? Mathf.Acos(lookVector.x) * Mathf.Rad2Deg : -Mathf.Acos(lookVector.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime);
    }
    void Move(float dist)
    {
        var newPos = new Vector3(velocity.x, velocity.y, 0);
        if (isMoving)
          rb.MovePosition(transform.position + newPos * currentSpeed * Time.fixedDeltaTime);
        else
        {
            var distDiff = Mathf.Clamp(dist - minDistanceFromOwner, 0.1f, 1);
            rb.MovePosition(transform.position + newPos * distDiff * currentSpeed * Time.fixedDeltaTime);
        }
    }

}
