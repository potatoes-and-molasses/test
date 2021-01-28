using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DogeUI : MonoBehaviour
{
    [SerializeField]
    int doge_id;

    
    public DogData data;
    // Start is called before the first frame update
    void Start()
    {

        TMP_Text name = GetComponentsInChildren<TMP_Text>()[1];
        TMP_Text reward = GetComponentsInChildren<TMP_Text>()[2];
        TMP_Text maintenance = GetComponentsInChildren<TMP_Text>()[3];
        TMP_Text risk = GetComponentsInChildren<TMP_Text>()[4];
        name.text = data.name;
        reward.text = "Base Reward: "+data.base_cost+"$";
        maintenance.text = $"Food Cost: {data.maintenance_cost}$";
        risk.text = "Risk Level: " + data.current_risk;
        doge_id = data.doge_id;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
