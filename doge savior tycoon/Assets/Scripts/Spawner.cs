using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float width;
    public float height;
    public GameObject cop_prefab;
    public GameObject[] humanPrefabs;
    public GameObject[] dogePrefabs;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TimePass), GameManager.timepassage, GameManager.timepassage);
    }

    public void TimePass()
    {
        
        if ((GameManager.cop_count < GameManager.cop_cap )&& (Random.Range(0, 1f) < GameManager.CopCreateChance()))
        {
            spawn_obj("cop");
        }
    }
    public Vector3 RandomPoint()
    {
        var x = Random.Range(transform.position.x - width / 2, transform.position.x + width / 2);
        var y = Random.Range(transform.position.y - height / 2, transform.position.y + height / 2);
        return new Vector3(x, y, 0);
    }
    void Update()
    {
        if (GameManager.human_count < GameManager.max_humans)
        {
            spawn_obj("humandog");
        }
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
                while(Vector3.Distance(pos,GameManager.Player.transform.position) < 5)
                {
                    pos = RandomPoint();
                }
                Instantiate(cop_prefab, pos, Quaternion.identity);
                GameManager.cop_count += 1;
                break;
            case "humandog":
                Vector3 pos2 = RandomPoint();
                int i = Random.Range(0, humanPrefabs.Length);
                GameObject human = Instantiate(humanPrefabs[i], pos2, Quaternion.identity);
                i = Random.Range(0, dogePrefabs.Length);
                GameObject doge = Instantiate(dogePrefabs[i], pos2+(new Vector3(2f,-2f,1)), Quaternion.identity);
                VictimController ct = human.GetComponent<VictimController>();
                ct.belovedDog = doge.GetComponent<DogMovement>();
                DogMovement dm = doge.GetComponent<DogMovement>();
                dm.owner = ct;
                dm.hand = ct.transform.GetChild(0);
                GameManager.human_count += 1;
                break;
            default:
                break;

        }
    }


}
