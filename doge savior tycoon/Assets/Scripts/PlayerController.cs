using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    
    Inputs inputs;
    Vector2 velocity;

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
        inputs.KeysandMouse.Movement.performed += ctx => velocity = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(velocity.x, velocity.x,0) * speed * Time.deltaTime;
    }
}
