using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_RBSFSM_AttackBaseState : BaseState_FSM_TS
{
    ST_SmartTank_RBSFSM smartTank;

    public ST_RBSFSM_AttackBaseState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        Debug.Log("Attacking enemy Base");
        smartTank.stats["attackingEnemyBase"] = true;
        return null;
    }

    public override Type StateExit()
    {
        smartTank.stats["attackingEnemyBase"] = false;
        return null;
    }

    public override Type StateUpdate()
    {
        smartTank.GetBasesFound();
        smartTank.GetCollectablesFound();

        smartTank.AttackBase();

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
