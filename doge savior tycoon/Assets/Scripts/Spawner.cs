using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float width;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 RandomPoint()
    {
        var x = Random.Range(transform.position.x - width / 2, transform.position.x + width / 2);
        var y = Random.Range(transform.position.y - height / 2, transform.position.y + height / 2);
        return new Vector3(x, y, 0);
    }
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));

    }


}
