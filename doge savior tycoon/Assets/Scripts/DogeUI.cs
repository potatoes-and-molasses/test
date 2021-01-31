using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DogeUI : MonoBehaviour
{
    [SerializeField]
    int doge_id;
    
    InventoryActions inventory_ref;
    public TMP_Text reward;
    public TMP_Text maintenance;
    public Scrollbar risk;
    public TMP_Text dname;
    public Button return_button;
    public Sprite canReturn;
    public Sprite canNotReturn;

    public DogData data;
    bool Enabled = false;
    // Start is called before the first frame update
    void Start()
    {

       /* dname = GetComponentsInChildren<TMP_Text>()[1];
        reward = GetComponentsInChildren<TMP_Text>()[2];
        maintenance = GetComponentsInChildren<TMP_Text>()[3];
        risk = GetComponentsInChildren<TMP_Text>()[4];
        return_button = GetComponentInChildren<Button>();*/
        inventory_ref = GameObject.Find("InventoryCanvas").GetComponent<InventoryActions>();
        dname.text = data.name;
        reward.text = data.sell()+"$";
        maintenance.text = $"Food Cost: {data.maintenance_cost}$";
        return_button.enabled = data.can_return;
        doge_id = data.doge_id;
        return_button.GetComponent<Image>().sprite = canNotReturn;
    }

    public void CanReturn()
    {
        return_button.GetComponent<Image>().sprite = canReturn;
        Enabled = true;
    }


    void Update()
    {
        risk.value = Mathf.Clamp(((float)data.current_risk) / 100,0,1);
        reward.text = data.sell() + "$";
        //risk.color = new Color(data.current_risk*10f/255f, 0, 0); // temp
        doge_id = data.doge_id;
        if(data.can_return && !Enabled)
        {
            CanReturn();
        }
        return_button.enabled = data.can_return;
    }

    public void selldoge()
    {
        inventory_ref.sell_doge(doge_id);
        Object.Destroy(this.gameObject);//need fancy sell animation and effects
    }

    public void OnDestroy()
    {
        
    }

}
