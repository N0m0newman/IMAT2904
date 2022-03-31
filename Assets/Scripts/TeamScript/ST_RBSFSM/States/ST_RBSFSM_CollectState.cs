using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_RBSFSM_CollectState : BaseState_FSM_TS
{
    ST_SmartTank_RBSFSM smartTank;
    public ST_RBSFSM_CollectState(ST_SmartTank_RBSFSM smartTank)
    {
        this.smartTank = smartTank;
    }

    public override Type StateEnter()
    {
        smartTank.stats["collectingState"] = true;
        return null;
    }

    public override Type StateExit()
    {
        smartTank.stats["collectingState"] = false;
        return null;
    }

    public override Type StateUpdate()
    {
        smartTank.GetEnemysFound();
        smartTank.GetCollectablesFound();
        smartTank.GetBasesFound();

        smartTank.consumablePosition = smartTank.consumablesFound.FirstOrDefault().Key;
        if(smartTank.consumablesFound.Count > 0)
        {
            smartTank.FollowTarget(smartTank.consumablePosition, 0.5f);
        } else
        {
            smartTank.stats["collectingState"] = false;
            foreach (var item in smartTank.rules.Rules)
            {
                if (item.CheckRule(smartTank.stats) != null)
                {
                    return item.CheckRule(smartTank.stats);
                }
            }
            return null;
        }
        smartTank.stats["collectingState"] = false;

        smartTank.targetTankPosition = null;
        smartTank.stats["spottedEnemy"] = false;

        smartTank.consumablePosition = null;
        smartTank.stats["spottedAmmo"] = false;
        smartTank.stats["spottedFuel"] = false;
        smartTank.stats["spottedHealth"] = false;

        smartTank.basePosition = null;
        smartTank.stats["spottedEnemyBase"] = false;

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
