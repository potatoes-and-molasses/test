using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float followSpeed = 10;
    Vector3 velocity = Vector3.zero;
    Vector3 lastPos;
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
         //velocity = transform.position - lastPos;
         //lastPos = transform.position;
         Vector3 newPos = GameManager.Player.transform.position;
         newPos.z = transform.position.z;
         velocity = newPos - transform.position;

         transform.position += velocity * followSpeed * Time.fixedDeltaTime;
        
    }
}
