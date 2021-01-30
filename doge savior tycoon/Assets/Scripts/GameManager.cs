using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static PlayerController Player;
    public static InventoryActions Inventory;
    public static Spawner Spawner;
    public static int total_risk;
    public static int timepassage = 1;
    public static float maxRiskSeconds = 120;



    static float CurrentRisk => Mathf.Clamp(total_risk / (maxRiskSeconds * maxRiskSeconds), 0, 1);
    
    void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        Inventory = FindObjectOfType<InventoryActions>();
        Spawner = FindObjectOfType<Spawner>();    }

    private void Start()
    {
        Inventory.hide();
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
        return CurrentRisk / 2;
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

    // Update is called once per frame
    void Update()
    {

    }
}
