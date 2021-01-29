using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static PlayerController Player;
    public static InventoryActions Inventory;
    
    void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        Inventory = FindObjectOfType<InventoryActions>();
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
