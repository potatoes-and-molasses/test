using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noticeboardBehavior : MonoBehaviour
{


    bool inflag;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

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
