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

        smartTank.basePosition = smartTank.basesFound.FirstOrDefault().Key;
        if (smartTank.basesFound.Count != 0)
        {
            if (smartTank.stats["lowAmmo"] != true)
            {
                smartTank.FireTank(smartTank.basePosition);
                smartTank.GetBasesFound();
                if (Vector3.Distance(smartTank.transform.position, smartTank.targetTankPosition.transform.position) > 50f)
                {
                    smartTank.stats["patroling"] = true;
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
            else
            {
                smartTank.stats["attackingEnemyBase"] = false;
                smartTank.stats["patrolling"] = true;
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
        else
        {
            smartTank.stats["patroling"] = true;
            foreach (var item in smartTank.rules.Rules)
            {
                if (item.CheckRule(smartTank.stats) != null)
                {
                    return item.CheckRule(smartTank.stats);
                }
            }
            return null;
        }
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
