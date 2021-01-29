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

    private void Update()
    {
         velocity = transform.position - lastPos;
         lastPos = transform.position;
         Vector3 newPos = GameManager.Player.transform.position;
         newPos.z = transform.position.z;
         transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, 10 * Time.smoothDeltaTime);
    }
}
