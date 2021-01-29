using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noticeboardBehavior : MonoBehaviour
{

    
    
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
        Debug.Log(collision.gameObject.name);
        //if (collision.gameObject.name == "Player")
        {
            int dogecount = GameManager.Player.GetDogeAmount();
            for (int i = 0; i < dogecount; i++)
            {
                GameManager.Inventory.generate_new_doge();
            }
            GameManager.ToggleInventory();
        }
    }

}
