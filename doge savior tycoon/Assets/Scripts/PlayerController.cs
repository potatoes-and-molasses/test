using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Movable
{
  
    private Inputs inputs;
    private Vector2 velocity;
    private float currentSpeed;
    private bool isMoving;
    private Rigidbody2D rb;
    private bool isStealing = false;

    public bool IsStealing => isStealing;
    
    private void Awake()
    {
        inputs = new Inputs();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        inputs.KeysandMouse.Movement.performed += ctx => 
        { 
            velocity = ctx.ReadValue<Vector2>();
            isMoving = true;
        };
        inputs.KeysandMouse.Movement.canceled += ctx =>
        {
            //velocity = Vector2.zero;
            isMoving = false;
        };
        inputs.KeysandMouse.StealDog.started += ctx => StartCoroutine(StealDog());
        inputs.KeysandMouse.StealDog.canceled += ctx => isStealing = false;
    }

    IEnumerator StealDog()
    {
        isStealing = true;
        yield return new WaitForFixedUpdate();
        isStealing = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSpeed();
        Move();
    }

    void UpdateSpeed()
    {
        if(isMoving)
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, speed);
        else
            currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.fixedDeltaTime, 0, speed);
    }

    void Move()
    {
        var newPos = transform.position + new Vector3(velocity.x, velocity.y, 0) * currentSpeed * Time.fixedDeltaTime;
        var angle = (velocity.y >=0)? Mathf.Acos(velocity.x) * Mathf.Rad2Deg : -Mathf.Acos(velocity.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime));
        rb.velocity = velocity * currentSpeed;
    }

}
