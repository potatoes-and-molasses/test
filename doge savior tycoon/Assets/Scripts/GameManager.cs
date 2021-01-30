using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static PlayerController Player;
    public static InventoryActions Inventory;
    public static int total_risk;
    public static int timepassage = 1;
    
    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        Inventory = FindObjectOfType<InventoryActions>();
        Inventory.hide();
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
