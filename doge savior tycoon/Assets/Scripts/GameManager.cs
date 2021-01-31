using System;
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
    public static int timepassage = 3;
    public static float maxRiskSeconds = 60;
    public static int max_humans = 15;
    public static int max_cops = 20;
    public static int cop_count = 0;
    public static int human_count = 0;
    public static int GameTime = 60 * 3;
    public static Action ShowScore;
    public static Action GameOver;
    private static float counter = 0;


    public static float CurrentRisk => Mathf.Clamp(total_risk / (maxRiskSeconds * maxRiskSeconds), 0, 1);
    public static int cop_cap => (int)(CurrentRisk*max_cops);

    void Awake()
    {
        counter = GameTime;
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

    public static string ShowTime()
    {
        var seconds = (int)counter;
        string s_t = "";
        string m_t = "";
        int s = seconds % 60;
        s_t = s < 10 ? "0" + s : "" + s;
        int m = seconds / 60;
        m_t = m < 10 ? "0" + m : "" + m;
        return $"{m_t}:{s_t}";
    }

    void Update()
    {
        if (Inventory.current_balance < 0)
        {
            GameOver.Invoke();
        }
            counter -= Time.deltaTime;
        if(counter < 0)
        {
            ShowScore.Invoke();
        }
    }
}
