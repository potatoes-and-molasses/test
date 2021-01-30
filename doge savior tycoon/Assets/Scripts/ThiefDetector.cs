using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefDetector : MonoBehaviour
{
    public float radius = 5;
    public float povAngle = 25;
    public Action OnStealingDetection;
    public Action OnPlayerEscaped;
    public LayerMask layerMask;
    private bool seenPlayer = false;
    void Start()
    {
        OnPlayerEscaped += () => { };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dist = Vector3.Distance(transform.position, GameManager.Player.transform.position);
        if(dist < radius)
        {
            var result = CanSeePlayer();
            if (result == true && !seenPlayer)
            {
                seenPlayer = true;
                OnStealingDetection.Invoke();
            }
            else if (result == false && seenPlayer)
            {
                seenPlayer = false;
                OnPlayerEscaped.Invoke();
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
        RaycastHit2D rc = Physics2D.Raycast(transform.position, dir, radius, layerMask);

        
        return (leftAngle <= angle && rightAngle >= angle) && (rc.collider == null);

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
