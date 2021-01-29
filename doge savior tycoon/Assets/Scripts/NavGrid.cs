using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public int width;
    public int height;
    public LayerMask layerMask;
    Vector3[,] Map;

    void Start()
    {
        Map = new Vector3[2 * width - 1, 2 * height - 1];
        GenerateBaseMap();
        Debug.Log(Map);
        CompleteMap();
    }

    bool AnotherOnMap(int w, int h, int i, int j) => (w + i != w || h + j != h) && (w + i >= 0) && (w + i < 2 * width - 1) && (h + j >= 0) && (h + j < 2 * height - 1);
    void CompleteMap()
    {
        for(int w = 0; w < 2*width-1; w++)
        {
            for(int h=0; h < 2*height-1; h++)
            {
                if (Map[w, h].z != -3)
                    continue;
                if (w % 2 == 0)
                {
                    //if (AnotherOnMap(w, h, i, j) && Map[w, h].z != -3)
                }
                for(int i = -1; i<2; i++)
                {
                    for (int j = -1; i < 2; i++)
                    {
                        if(AnotherOnMap(w,h,i,j) && Map[w,h].z != -3)
                        {
                            var dist = Vector3.Distance(Map[w, h], Map[w + i, h + j]);
                            var dir = (Map[w + i, h + j] - Map[w, h]).normalized;
                            RaycastHit2D hit = Physics2D.Raycast(Map[w, h], dir, dist, layerMask);
                            if (hit.collider != null)
                                Map[w + i, h + j].z = -3;
                        }
                    }
                }
            }
        }
    }


    void GenerateBaseMap()
    {
        for(int i = 0; i < 2*width-1; i++)
        {
            for(int j = 0; j < 2*height-1; j++)
            {
                if (i % 2 == 0)
                    Map[i,j] = transform.position + new Vector3(i/2, j, 0);
                else
                    Map[i, j] = transform.position + new Vector3(((float)i)/2, j+0.5f, 0);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        var w = width - 1;
        var h = 2*height - 1.5f;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * w);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * h);
        Gizmos.DrawLine(transform.position + Vector3.right * w + Vector3.up * h, transform.position + Vector3.right * w);
        Gizmos.DrawLine(transform.position + Vector3.right * w + Vector3.up * h, transform.position + Vector3.up * h);
        if (Map == null)
            return;
        
        foreach (Vector3 v in Map)
        {
            if (v.z != -3)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(v, 0.1f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
            
    }
}
