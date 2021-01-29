using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DogeUI : MonoBehaviour
{
    [SerializeField]
    int doge_id;
    [SerializeField]
    InventoryActions inventory_ref;
    TMP_Text name;
    TMP_Text reward;
    TMP_Text maintenance;
    TMP_Text risk;
    

    public DogData data;
    // Start is called before the first frame update
    void Start()
    {

        name = GetComponentsInChildren<TMP_Text>()[1];
        reward = GetComponentsInChildren<TMP_Text>()[2];
        maintenance = GetComponentsInChildren<TMP_Text>()[3];
        risk = GetComponentsInChildren<TMP_Text>()[4];
        inventory_ref = GameObject.Find("InventoryCanvas").GetComponent<InventoryActions>();
        name.text = data.name;
        reward.text = "Base Reward: "+data.base_cost+"$";
        maintenance.text = $"Food Cost: {data.maintenance_cost}$";
        risk.text = "Risk Level: " + data.current_risk;
        doge_id = data.doge_id;

    }

    // Update is called once per frame
    void Update()
    {
        risk.text = "Risk Level: " + data.current_risk;
        risk.color = new Color(data.current_risk*10f/255f, 0, 0); // temp
        doge_id = data.doge_id;
    }

    public void selldoge()
    {
        inventory_ref.sell_doge(doge_id);
        Object.Destroy(this.gameObject);//need fancy sell animation and effects
    }

}
