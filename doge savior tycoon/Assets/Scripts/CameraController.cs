using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float followSpeed = 10;
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 newPos = GameManager.Player.transform.position;
        newPos.z = transform.position.z;
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
        
    }
}
