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
        smartTank.GetEnemysFound();
        smartTank.GetCollectablesFound();
        smartTank.GetEnemysFound();

        smartTank.targetTankPosition = smartTank.targetTanksFound.FirstOrDefault().Key;
        if (smartTank.targetTanksFound.FirstOrDefault().Key != null)
        {
            smartTank.GetEnemysFound();
            smartTank.targetTankPosition = smartTank .targetTanksFound.FirstOrDefault().Key;
            smartTank.stats["inRangeOfEnemy"] = (Vector3.Distance(smartTank.transform.position, smartTank.targetTankPosition.transform.position) < 25f) ? true : false;
            if (!smartTank.stats["inRangeOfEnemy"])
            {
                Debug.Log("l");
                smartTank.FollowTarget(smartTank.targetTankPosition, 1f);
            }
        } else
        {
            Debug.Log("o");
            //smartTank.stats["inRangeOfEnemy"] = false;
            smartTank.stats["chasingEnemy"] = false;
            smartTank.stats["patrolling"] = true;
        }

        foreach (var item in smartTank.rules.Rules)
        {
            Debug.Log("billy cool guy");
            if (item.CheckRule(smartTank.stats) != null)
            {
                return item.CheckRule(smartTank.stats);
            }
        }
        return null;
    }
}
