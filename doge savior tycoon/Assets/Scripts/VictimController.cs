using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimController : Movable
{
    enum State { UsePhone, Walking , LookAround};
    public DogMovement belovedDog;
    public LayerMask layerMask;
    public LayerMask enviromentLayerMask;

    private float currentSpeed;
    ThiefDetector detector;
    private Rigidbody2D rb;
    private Vector2 velocity;
    public Vector3 target;
    private float timeCounter = 0;
    private float waitingTime = 1;
    private float rotationDelta = 0;
    float startAngle = 0;
    float endAngle = 0;
    [SerializeField]
    State state;
    void Awake()
    {
        belovedDog.owner = this;
        belovedDog.hand = transform;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = GetComponent<ThiefDetector>();
        detector.OnStealingDetection += OnStealingDetection;
        ChooseState();
    }
    void CheckIfPlayerStealDoge()
    {
        var dist = Vector3.Distance(belovedDog.transform.position, transform.position);
        var dir = (belovedDog.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, layerMask);
        if(hit.collider != null)
        {
            PlayerController player = GameManager.Player;
            if (player.IsStealing)
            {   
                belovedDog.owner = GameManager.Player;
                belovedDog.hand = player.GetHand();
                player.AddDog();
            }
        }
    }

    void OnStealingDetection()
    {
        Debug.Log("Oh no!");
    }

    void VictimLogic()
    {
        switch (state)
        {
            case State.UsePhone:
                if(timeCounter > waitingTime)
                {
                    ChooseState();
                    return;
                }
                timeCounter += Time.fixedDeltaTime;
                return;
            case State.Walking:
                LookAtTarget();
                UpdateSpeed();
                CalculateVelocity();
                Move();
                if (Vector3.Distance(transform.position, target) < 0.05f)
                    ChooseState();
                return;
            case State.LookAround:
                if (timeCounter >= waitingTime)
                {
                    ChooseState();
                    return;
                }
                timeCounter += Time.fixedDeltaTime;
                if(rotationDelta >= 1)
                {
                    rotationDelta = 0;
                    ChooseLookTarget();
                    return;
                }
                transform.rotation = Quaternion.Euler(0, 0, (1 - rotationDelta) * startAngle + rotationDelta * endAngle);
                rotationDelta += Time.fixedDeltaTime * turnSpeed;
                return;

        }
    }

    void ChooseState()
    {
        var res = Random.Range(0, 100);
        state = (res <= 33) ? State.Walking : (res <= 66)? State.UsePhone : State.LookAround;
        if(state == State.Walking)
        {
            ChooseTarget(0);
        }
        else if(state == State.UsePhone)
        {
            timeCounter = 0;
            waitingTime = Random.Range(2, 5);
        }
        else if(state == State.LookAround)
        {
            timeCounter = 0;
            waitingTime = Random.Range(1, 4);
            rotationDelta = 0;
            ChooseLookTarget();
        }
    }

    void LookAtTarget()
    {
        var angle = (velocity.y >= 0) ? Mathf.Acos(velocity.x) * Mathf.Rad2Deg : -Mathf.Acos(velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime);
    }

    void Move()
    {
        var newPos = transform.position + new Vector3(velocity.x, velocity.y, 0) * currentSpeed * Time.fixedDeltaTime;
        var angle = (velocity.y >= 0) ? Mathf.Acos(velocity.x) * Mathf.Rad2Deg : -Mathf.Acos(velocity.x) * Mathf.Rad2Deg;
        rb.velocity = velocity * currentSpeed;
    }
    void CalculateVelocity()
    {
            velocity = (target - transform.position).normalized;
    }

    void UpdateSpeed()
    {
        if (state == State.Walking)
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, speed);
        else
            currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.fixedDeltaTime, 0, speed);
    }

    private void ChooseLookTarget()
    {
        
        startAngle = transform.rotation.eulerAngles.z;
        endAngle = (360 + startAngle + Random.Range(90, 180)) % 360;

    }
    private void ChooseTarget(int attempts)
    {
        if(attempts > 10)
        {
            ChooseState();
            return;
        }
        var length = Random.Range(3, 5);
        var angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 newTarget = transform.position + new Vector3(Mathf.Sin(angle) * length, Mathf.Cos(angle) * length, 0);
        var dir = (newTarget - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, length, enviromentLayerMask);
        if(hit.collider != null)
        {
            ChooseTarget(attempts+1);
            return;
        }
        target = newTarget;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSpeed();
        CheckIfPlayerStealDoge();
        VictimLogic();
    }
}