using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_RBSFSM_PatrolState : BaseState_FSM_TS
{
    private ST_SmartTank_RBSFSM smartTank;
    public ST_RBSFSM_PatrolState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        smartTank.stats["patrolling"] = true;
        return null;
    }

    public override Type StateExit()
    {
        smartTank.stats["patrolling"] = false;
        return null;
    }

    public override Type StateUpdate()
    {
        smartTank.GotoRandomLocation(0.5f);
        foreach(var item in smartTank.rules.Rules)
        {
            if(item.CheckRule(smartTank.stats) != null)
            {
                return item.CheckRule(smartTank.stats);
            }
        }
        return null;
    }
}
