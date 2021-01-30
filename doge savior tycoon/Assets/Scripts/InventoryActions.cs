using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    bool active = true;
    TMP_Text balance_value;
    TMP_Text maint_value;

    public void hide()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        active = false;

    }

    public void show()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        active = true;

    }

    public bool isActive()
    {
        return active;
    }

    public void enable_doges()
    {
        foreach(DogData doge in doges)
        {
            doge.can_return = true;
        }
    }
    string[] NAMES = {"Mr. Doge","Doge, the Esteemed", "Good Ol' Doge", "Yet Another Doge", "Doge, the First", "Just a Doge", "Bob", "Arch-Dog Extraordinaire", "Cat"};
    public void generate_new_doge()
    {
        string name = NAMES[Random.Range(0, NAMES.Length)];
        int basec = Random.Range(50, 150);
        int maintenance = Random.Range(basec / 10, basec / 5);
        float mod = Random.Range(0.6f, 1.4f);
        DogData newdog = new DogData(name, basec, maintenance, mod, doges.Count);
        doges.Add(newdog);
        create_doge(newdog);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        current_balance = 100;
        cost_per_sec = 0;
        balance_value = GameObject.Find("CurrentBalanceValue").GetComponent<TMP_Text>();
        maint_value = GameObject.Find("CurrentMaintValue").GetComponent<TMP_Text>();
        InvokeRepeating(nameof(TimePass), GameManager.timepassage, GameManager.timepassage);
        //generate_new_doge();
        //generate_new_doge();
        //generate_new_doge();
        
        
    }

    void TimePass()
    {
        current_balance -= cost_per_sec;
        total_risk = 0;
        cost_per_sec = 0;
        foreach (DogData dog in doges)
        {
            dog.updog();
            total_risk += (int)Mathf.Pow(dog.current_risk,2);
            cost_per_sec += dog.maintenance_cost;

        }
        GameManager.total_risk = total_risk;

        //Debug.Log("current balance: " + current_balance + " current_risk: " + total_risk);
        //need ui indication
    }
    // Update is called once per frame
    void Update()
    {
        balance_value.text = current_balance + "$";
        maint_value.text = cost_per_sec + "$";

        
    }

    void create_doge(DogData data)
    {

        GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
        obj.GetComponent<DogeUI>().data = data;
        
        obj.transform.SetParent(transform.Find("DogsPanel").transform);
    }
    

    public void remove_all_doges()
    {
        foreach (DogMovement onedoge in GameObject.FindObjectsOfType<DogMovement>())
        {
            if (onedoge.owner.name == "Player")
            {
                Destroy(onedoge.gameObject);
                
            }
        }
        foreach (DogeUI dogeui in GameObject.FindObjectsOfType<DogeUI>())
        {
            Destroy(dogeui.gameObject);
        }

        doges.Clear();



    }
    public void sell_doge(int idx)
    {
        int res = doges[idx].sell();
        if (res > 0)
        {
            current_balance += res;
            cost_per_sec -= doges[idx].maintenance_cost;
            doges.RemoveAt(idx);
            for (int i=0; i < doges.Count; i++)
            {
                doges[i].doge_id = i;
            }

        }
        
        foreach (DogMovement onedoge in GameObject.FindObjectsOfType<DogMovement>())
        {
            if (onedoge.owner.name == "Player")
            {
                Destroy(onedoge.gameObject);
                break;
            }
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
