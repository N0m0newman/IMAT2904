using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_RBSFSM_RoamState : BaseState_FSM_TS
{

    ST_SmartTank_RBSFSM smartTank;
    public ST_RBSFSM_RoamState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        Debug.Log("roaming");
        smartTank.stats["patroling"] = true;
        smartTank.targetTankPosition = null;
        smartTank.consumablePosition = null;
        smartTank.basePosition = null;
        return null;
    }

    public override Type StateExit()
    {
        Debug.Log("oawurioahrtf");
        smartTank.stats["patroling"] = false;
        return null;
    }
    bool t = false;

    public override Type StateUpdate()
    {
        if(t == false)
        {
            Debug.Log("racist words");
            t = true;
        }
        //Update stored data Dicton
        smartTank.GetEnemysFound();
        smartTank.GetCollectablesFound();
        smartTank.GetBasesFound();

        #region Moving
        if (smartTank.GetFuel() >= 40) //if the fuel is greater then or eqaul to 60 move at 0.5 speed
        {
            smartTank.GotoRandomLocation(0.5f);
            smartTank.stats["lowFuel"] = false;
        }
        else
        {
            smartTank.GotoRandomLocation(0.25f);
            smartTank.stats["lowFuel"] = true;
        }
        #endregion

        //reevaluate rules
        foreach (var item in smartTank.rules.Rules)
        {
            if(item.CheckRule(smartTank.stats) != null)
            {
                return item.CheckRule(smartTank.stats);
            }
        }
        return null;
    }

}
