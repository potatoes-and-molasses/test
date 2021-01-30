using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float width;
    public float height;
    public GameObject cop_prefab;
    public GameObject human_prefab;
    public GameObject doge_prefab;
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

    public void spawn_obj(string objtype)
    {
        switch (objtype){
            case "cop":
                Vector3 pos = RandomPoint();
                Debug.Log("spawn cop at " + pos);
                GameObject da_police = Instantiate(cop_prefab, pos, transform.rotation);
                break;
            case "humandog":
                Vector3 pos2 = RandomPoint();
                Debug.Log("spawn humandog at " + pos2);
                GameObject human = Instantiate(human_prefab, pos2, transform.rotation);
                GameObject doge = Instantiate(doge_prefab, pos2, transform.rotation);
                break;
            default:
                break;

        }
    }


}
