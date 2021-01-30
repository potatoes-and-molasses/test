using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noticeboardBehavior : MonoBehaviour
{


    bool inflag;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        {
            GameManager.Inventory.enable_doges();
            if (!inflag)
            {
                GameManager.ToggleInventory();
                inflag = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        {
            inflag = false;
        }
    }
}
