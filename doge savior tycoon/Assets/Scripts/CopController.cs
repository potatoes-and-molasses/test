using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThiefDetector))]
[RequireComponent(typeof(Rigidbody2D))]
public class CopController : Movable
{
    enum State { Patrolling, Chasing}
    ThiefDetector detector;
    bool LookingForPlayer;
    public float patrolSpeed = 3;
    public LayerMask layerMask;
    public LayerMask environmentLayerMask;
    public float alertTime = 10;
    public Sprite fovBlue;
    public Sprite fovRed;
    public Transform FOV;

    State state = State.Patrolling;
    Vector3 velocity;
    private float currentSpeed;
    Vector3 target;
    float distanceToTarget = 0;
    Rigidbody2D rb;
    float counter = 0;
    float wait = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = GetComponent<ThiefDetector>();
        detector.OnStealingDetection += NoticePlayer;
        detector.OnPlayerEscaped += () => state = State.Patrolling;
        CreateNewTarget();
    }

    public void BeAlert()
    {
        StartCoroutine(LookForPlayer());
    }

    IEnumerator LookForPlayer()
    {
        LookingForPlayer = true;
        SetFovColor(fovRed);
        yield return new WaitForSeconds(alertTime);
        LookingForPlayer = false;
        SetFovColor(fovBlue);
    }

    void CopLogic()
    {
        switch (state)
        {
            case State.Patrolling:
                if(distanceToTarget < 0.2f || counter > wait)
                {
                    CreateNewTarget();
                    wait = 10;
                    counter = 0;
                }
                counter += Time.fixedDeltaTime;
                Updatespeed();
                LookAtTarget();
                velocity = (target - transform.position).normalized;
                Move();
                return;
            case State.Chasing:
                if(counter > wait)
                {
                    CreateNewTarget();
                    LookingForPlayer = false;
                    state = State.Patrolling;
                    SetFovColor(fovBlue);
                    wait = 10;
                    counter = 0;
                    return;
                }
                counter += Time.fixedDeltaTime;
                target = GameManager.Player.transform.position;
                Updatespeed();
                LookAtTarget();
                velocity = (target - transform.position).normalized;
                Move();
                return;
        }
    }

    void SetFovColor(Sprite fov)
    {
        FOV.GetComponent<SpriteRenderer>().sprite = fov;
    }
    private void CreateNewTarget()
    {
        //var length = UnityEngine.Random.Range(5, 10);
        //var angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector3 newTarget = GameManager.Spawner.RandomPoint();
        var dir = (newTarget - transform.position).normalized;
        var dist = Vector3.Distance(transform.position, target);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, environmentLayerMask);
        if (hit.collider != null)
            target = new Vector3(hit.point.x, hit.point.y, 0) - dir;
        else
            target = newTarget;
    }

    private void Move()
    {
        var newPos = transform.position + transform.right * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    private void LookAtTarget()
    {
        var angle = (velocity.y >= 0) ? Mathf.Acos(velocity.x) * Mathf.Rad2Deg : -Mathf.Acos(velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime);
    }

    private void Updatespeed()
    {
        if(state == State.Chasing)
        {
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.fixedDeltaTime, 0, speed);
        }
        else
        {
            var multiplayer = Mathf.Clamp(distanceToTarget, 0, 1);
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * multiplayer * Time.fixedDeltaTime, 0, patrolSpeed);
        }
    }

    void FixedUpdate()
    {
        distanceToTarget = Vector3.Distance(target, transform.position);
        CopLogic();
        FOV.localScale = new Vector3(detector.radius, detector.radius, 1);
    }

    void NoticePlayer()
    {
        if(LookingForPlayer || GameManager.Player.IsStealing)
        {
            state = State.Chasing;
            SetFovColor(fovRed);
            wait = 10;
            counter = 0;
        }        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
