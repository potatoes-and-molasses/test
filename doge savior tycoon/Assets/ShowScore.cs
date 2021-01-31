using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowFinaleScore()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        text.text = $"Final Score : {GameManager.Inventory.current_balance} Doges Collected: Too many...\n to Exit the game: press ESC\tto Play Again: press R";

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
