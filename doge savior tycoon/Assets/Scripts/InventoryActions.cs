using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActions : MonoBehaviour
{
    
    public GameObject prefab;
    public int current_balance;
    public int cost_per_sec;
    public List<DogData> doges = new List<DogData>();
    public int total_risk = 0;
    public bool has_whistle = false;
    public bool has_boots = false;
    public bool has_well = false;
    const int WHISTLE_COST = 50;
    const int BOOTS_COST = 300;
    const int WELL_COST = 10000;
    string[] NAMES = {"Mr. Doge","Doge, the Esteemed", "Good Ol' Doge", "Yet Another Doge", "Doge, the First", "Just a Doge", "Bob", "Arch-Dog Extraordinaire", "Cat"};
    public void generate_new_doge()
    {
        string name = NAMES[Random.Range(0, NAMES.Length)];
        int basec = Random.Range(50, 250);
        int maintenance = Random.Range(1, basec / 10);
        float mod = Random.Range(0.4f, 1.6f);
        create_doge(new DogData(name, basec, maintenance, mod, doges.Count));
    }
    // Start is called before the first frame update
    void Start()
    {
        current_balance = 10;
        cost_per_sec = 0;
        InvokeRepeating("TimePass", 1, 1);
        generate_new_doge();
        
    }

    void TimePass()
    {
        current_balance -= cost_per_sec;
        total_risk = 0;
        foreach (DogData dog in doges)
        {
            dog.updog();
            total_risk += dog.current_risk;
        }

        Debug.Log("current balance: " + current_balance + " current_risk: " + total_risk);
        //need ui indication
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void create_doge(DogData data)
    {

        GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
        obj.GetComponent<DogeUI>().data = data;
        
        obj.transform.SetParent(transform.Find("DogsPanel").transform);
    }
    void sell_doge(int idx)
    {
        int res = doges[idx].sell();
        if (res > 0)
        {
            current_balance += res;
            cost_per_sec -= doges[idx].maintenance_cost;
            doges.RemoveAt(idx);
            

        }

    }

    public void upgrade_whistle()
    {
        if (current_balance > WHISTLE_COST)
        {
            current_balance -= WHISTLE_COST;
            has_whistle = true;
        }
    }

    public void upgrade_boots()
    {
        current_balance -= BOOTS_COST;
        has_boots = true;
    }

    public void upgrade_well()
    {
        current_balance -= WELL_COST;
        has_well = true;
    }

}
