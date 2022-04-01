using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_RBSFSM_ChaseState : BaseState_FSM_TS
{
    ST_SmartTank_RBSFSM smartTank;
    public ST_RBSFSM_ChaseState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        Debug.Log("Chasing Enemy Player");
        smartTank.stats["chasingEnemy"] = true;
        smartTank.targetTankPosition = null;
        smartTank.consumablePosition = null;
        smartTank.basePosition = null;
        return null;
    }

    public override Type StateExit()
    {
        Debug.Log("stopped Chasing Enemy");
        smartTank.stats["chasingEnemy"] = false;
        return null;
    }

    public override Type StateUpdate()
    {

        smartTank.ChasePlayer();

        foreach (var item in smartTank.rules.Rules)
        {
            if (item.CheckRule(smartTank.stats) != null)
            {
                return item.CheckRule(smartTank.stats);
            }
        }
        return null;
    }
}
