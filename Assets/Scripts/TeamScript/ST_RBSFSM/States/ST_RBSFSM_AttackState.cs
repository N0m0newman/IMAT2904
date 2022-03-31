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

    public override Type StateUpdate()
    {
        smartTank.GetEnemysFound();
        smartTank.GetCollectablesFound();
        smartTank.GetEnemysFound();

        smartTank.targetTankPosition = smartTank.targetTanksFound.FirstOrDefault().Key;
        if(smartTank.targetTanksFound.Count != 0)
        {
            smartTank.GetEnemysFound();
            if(smartTank.stats["inRangeOfEnemy"] == true && smartTank.stats["lowAmmo"] != true)
            {
                smartTank.FireTank(smartTank.targetTankPosition);
                smartTank.GetEnemysFound();
                if(Vector3.Distance(smartTank.transform.position, smartTank.targetTankPosition.transform.position) > 50f) {
                    smartTank.stats["inRangeOfEnemy"] = false;
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
            } else
            {
                smartTank.stats["inRangeOfEnemy"] = false;
                smartTank.stats["chasingEnemy"] = true;
                foreach (var item in smartTank.rules.Rules)
                {
                    if (item.CheckRule(smartTank.stats) != null)
                    {
                        return item.CheckRule(smartTank.stats);
                    }
                }
                return null;
            }
        } else
        {
            smartTank.stats["inRangeOfEnemy"] = false;
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
