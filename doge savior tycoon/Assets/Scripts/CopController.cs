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
    bool colorBlue = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detector = GetComponent<ThiefDetector>();
        detector.OnStealingDetection += NoticePlayer;
        detector.OnPlayerEscaped += () => {state = State.Patrolling; wait = 2; counter = 0; };
        CreateNewTarget();
    }


    private void OnTriggerEnter2D(Collider2D collision) //not working yet
    {
        Debug.Log(collision.name);
        if (collision.name == "Player")//noooo
        {
            GameManager.Inventory.remove_all_doges();
        }
    }


    void CopLogic()
    {
        switch (state)
        {
            case State.Patrolling:
                if (!colorBlue)
                {
                    SetFovColor(fovBlue);
                    colorBlue = true;
                }
                if(distanceToTarget < 0.4f || counter > wait)
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
                if (colorBlue)
                {
                    SetFovColor(fovRed);
                    colorBlue = false;
                }
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

    private void OnBecameInvisible()
    {
        int delta = GameManager.cop_count - GameManager.cop_cap;
        if (delta > 0)
        {
            float rnd = UnityEngine.Random.Range(0f, 1f);
            if (rnd < (delta / (GameManager.cop_count + GameManager.cop_cap)))
            {
                Debug.Log("copdelete engaged");
                Destroy(gameObject);
                GameManager.cop_count -= 1;
            }
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
        state = State.Chasing;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
