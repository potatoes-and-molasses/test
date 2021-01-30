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
    bool doge_b_gone = false;
    [SerializeField]
    State state;
    void Awake()
    {
        //belovedDog.owner = this;
        //belovedDog.hand = transform;
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
        if (doge_b_gone)
        {
            return;
        }
        var dist = Vector3.Distance(belovedDog.transform.position, transform.position);
        var dir = (belovedDog.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, layerMask);
        if(hit.collider != null)
        {
            PlayerController player = GameManager.Player;
            if (player.IsTryingToSteal)
            {   
                belovedDog.owner = GameManager.Player;
                belovedDog.hand = player.GetHand();
                belovedDog = null;
                player.AddDog();
                doge_b_gone = true;
            }
        }
    }

    void OnStealingDetection()
    {
        var cops = new List<CopController>(FindObjectsOfType<CopController>());
        cops.Sort((a, b) =>Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        var num = Mathf.Min(GameManager.NumberOfCops(), cops.Count);

        for (int i = 0; i < num; i++)
        {
            cops[i].BeAlert();
        }
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
                UpdateSpeed();
                Move();
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
                UpdateSpeed();
                Move();
                return;

        }
    }

    void ChooseState()
    {
        var res = Random.Range(0, 100);
        state = (res <= 33) ? State.Walking : (res <= 66)? State.UsePhone : State.LookAround;
        if(state == State.Walking)
        {
            ChooseTarget();
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
        rb.MovePosition(newPos);
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
    private void ChooseTarget()
    {
        var newTarget = GameManager.Spawner.RandomPoint();
        var dir = (newTarget - transform.position).normalized;
        var dist = Vector3.Distance(transform.position, target);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, layerMask);
        if (hit.collider != null)
            target = new Vector3(hit.point.x, hit.point.y,0) - dir;
        else
            target = newTarget;
    }

    private void OnBecameInvisible()
    {
        if (belovedDog == null)
        {
            Destroy(gameObject);
            GameManager.human_count -= 1;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSpeed();
        if(belovedDog != null)
            CheckIfPlayerStealDoge();
        VictimLogic();
    }
}