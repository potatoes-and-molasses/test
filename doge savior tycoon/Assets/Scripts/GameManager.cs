using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static PlayerController Player;
    public static InventoryActions Inventory;
    public static Spawner Spawner;
    public static int total_risk;
    public static int timepassage = 1;
    public static float maxRiskSeconds = 120;
    public static int max_humans = 15;
    public static int max_cops = 20;
    public static int cop_count = 0;
    public static int human_count = 0;



    static float CurrentRisk => Mathf.Clamp(total_risk / (maxRiskSeconds * maxRiskSeconds), 0, 1);
    public static int cop_cap => (int)(CurrentRisk*max_cops);

    void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        Inventory = FindObjectOfType<InventoryActions>();
        Spawner = FindObjectOfType<Spawner>();    }

    private void Start()
    {
        Inventory.hide();
        //Spawner.spawn_obj("cop");
        //Spawner.spawn_obj("humandog");
    }

    public static float ChaseTime()
    {
        var t = CurrentRisk;
        return (1 - t) * 5 + t * 30;
    }

    public static float VisionRadius()
    {
        var t = CurrentRisk;
        return (1 - t) * 5 + t * 10;
    }

    public static int NumberOfCops()
    {
        var t = CurrentRisk;
        return (int)((1 - t) * 1 + t * 10);
    }

    public static float CopCreateChance()
    {
        return CurrentRisk / 3;
    }

    public static void ToggleInventory()
    {
        if(Inventory.isActive())
        {
            Inventory.hide();
            return;
        }
        Inventory.show();
    }

    bool IsGameOver()
    {
        if (Inventory.current_balance < 0)
        {
            return true;
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsGameOver())
        {
            Debug.Log("gameover placeholder");
        }
    }

   
}
