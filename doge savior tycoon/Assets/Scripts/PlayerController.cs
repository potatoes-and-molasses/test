using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Movable
{

    private Inputs inputs;
    private Vector2 velocity;
    private float currentSpeed;
    private bool isMoving;
    private bool isStealingSucceed = false;
    private Rigidbody2D rb;
    private bool isStealing = false;
    private int newDogs;
    public bool IsTryingToSteal => isStealing;
    public bool IsStealing => isStealing && isStealingSucceed;
    public Vector3 Velocity => velocity * currentSpeed;
    public Animator anim;
    bool stealAnimPlaying = false;
    public StudioEventEmitter emitter;
    public StudioEventEmitter footSteps;

    public void AddDog()
    {
        newDogs++;
        GameManager.Inventory.generate_new_doge();
        StartCoroutine(OnRescue());
    }

    IEnumerator OnRescue()
    {
        isStealingSucceed = true;
        yield return new WaitForSeconds(0.3f);
        isStealingSucceed = false;
    }

    public int GetDogeAmount()
    {
        int res = newDogs;
        newDogs = 0;
        return res;
    }
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
        emitter = GetComponent<StudioEventEmitter>();
        anim = GetComponent<Animator>();
        inputs.KeysandMouse.Movement.performed += ctx => 
        { 
            velocity = ctx.ReadValue<Vector2>();
            isMoving = true;
        };
        inputs.KeysandMouse.Movement.canceled += ctx =>
        {   
            isMoving = false;
        };
        inputs.KeysandMouse.StealDog.started += ctx =>
        {
            if (!stealAnimPlaying)
                StartCoroutine(PlayStealingAnimation());
            StartCoroutine(StealDog());
        };
        inputs.KeysandMouse.StealDog.canceled += ctx => { isStealing = false; anim.ResetTrigger("Stealing"); };
        inputs.KeysandMouse.Inventory.started += ctx => GameManager.ToggleInventory();
        inputs.KeysandMouse.Exit.performed += ctx => Application.Quit();
        inputs.KeysandMouse.Restart.performed += ctx => SceneManager.LoadScene(0);
    }

    public Transform GetHand()
    {
        return transform.GetChild(0);
    }
    IEnumerator PlayStealingAnimation()
    {
        anim.SetTrigger("Stealing");
        stealAnimPlaying = true;
        yield return new WaitForSeconds(0.5f);
        stealAnimPlaying = false;
    }
    IEnumerator StealDog()
    {
        isStealing = true;
        emitter.Play();
        yield return new WaitForFixedUpdate();
        stealAnimPlaying = false;
        isStealing = false;

    }

    private void Update()
    {
        footSteps.SetParameter("Walk", 1 - Mathf.Clamp(currentSpeed,0,1));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetBool("Walking", (velocity*currentSpeed).magnitude > 0.1f);
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
