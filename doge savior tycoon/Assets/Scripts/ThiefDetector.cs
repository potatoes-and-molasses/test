using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefDetector : MonoBehaviour
{
    public float radius = 5;
    public float povAngle = 25;
    public Action OnStealingDetection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(transform.position, GameManager.Player.transform.position);
        if(dist < radius)
        {
            if(CanSeePlayer())
            {
                if(GameManager.Player.IsStealing)
                {
                    OnStealingDetection.Invoke();
                }
            }
        }
    }

    bool CanSeePlayer()
    {
        var currentAngle = transform.eulerAngles.z;
        var rightAngle = currentAngle + povAngle;
        var leftAngle = currentAngle - povAngle;
        var dir = (GameManager.Player.transform.position - transform.position).normalized;
        var angle = (dir.y >= 0) ? Mathf.Acos(dir.x) * Mathf.Rad2Deg : -Mathf.Acos(dir.x) * Mathf.Rad2Deg;

        return leftAngle <= angle && rightAngle >= angle;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var currentAngle = transform.eulerAngles.z;
        var angleA = (currentAngle + povAngle)*Mathf.Deg2Rad;
        var angleB = (currentAngle - povAngle)*Mathf.Deg2Rad;
        var pointA = transform.position + radius * (new Vector3(Mathf.Cos(angleA), Mathf.Sin(angleA), 0));
        var pointB = transform.position + radius * (new Vector3(Mathf.Cos(angleB), Mathf.Sin(angleB), 0));
        var forward = transform.position + radius * (new Vector3(Mathf.Cos(currentAngle*Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0));
        Gizmos.DrawLine(transform.position, pointA);
        Gizmos.DrawLine(transform.position, pointB);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, forward);
    }
}
