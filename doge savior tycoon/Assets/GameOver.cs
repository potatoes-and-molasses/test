using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        GameManager.GameOver += () => { gameObject.SetActive(true); Time.timeScale = 0; };
        gameObject.SetActive(false);
    }

}
