using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"DogeCoins: {GameManager.Inventory.current_balance}\t\tRemaining Time: {GameManager.ShowTime()}";
    }
}
