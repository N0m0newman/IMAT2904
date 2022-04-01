using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_RBSFSM_AttackState : BaseState_FSM_TS
{
    ST_SmartTank_RBSFSM smartTank;

    public ST_RBSFSM_AttackState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        Debug.Log("Attacking enemy Player");
        smartTank.stats["attackingEnemyPlayer"] = true;
        return null;
    }

    public override Type StateExit()
    {
        smartTank.stats["attackingEnemyPlayer"] = false;
        return null;
    }
    float time;
    public override Type StateUpdate()
    {

        smartTank.AttackPlayer();

        time += Time.deltaTime;
        if(time > 1f)
        {
            foreach (var item in smartTank.rules.Rules)
            {
                if (item.CheckRule(smartTank.stats) != null)
                {
                    return item.CheckRule(smartTank.stats);
                }
            }
            return null;
        }
        return null;
    }
}
