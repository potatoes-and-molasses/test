using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogData 
{
    public string name;
    public int base_cost;
    public int current_risk;
    public int time_kept;
    public int maintenance_cost;
    public float increase_modifier;
    public int doge_id;
    public bool can_return;
    public DogData(string aname, int basec, int maintenance, float modifier, int id)
    {
        name = aname;
        base_cost = basec;
        current_risk = 0;
        time_kept = 0;
        maintenance_cost = maintenance;
        increase_modifier = modifier;
        doge_id = id;
        can_return = false;
    }
    
    public int sell()
    {
        return (int)(base_cost + base_cost*Mathf.Pow((float)(increase_modifier * time_kept/10), 1.5f));
    }

    public void updog()
    {
        this.time_kept += 1;
        this.current_risk += 1;

    }

    public void dbgprint()
    {
        Debug.Log(string.Format("dogstats:\nname: {0}\nbasecost: {1}\ncurrent_risk: {2}\ntime_kept: {3}\nmaintenance_cost: {4}\nincrease_mod: {5}", name, base_cost, current_risk, time_kept, maintenance_cost, increase_modifier));
    }
}
