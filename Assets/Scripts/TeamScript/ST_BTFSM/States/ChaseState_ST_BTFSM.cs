using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChaseState_ST_BTFSM : BaseState_FSM_TS
{

    private SmartTank_ST_BTFSM AiTank_FSM;
    public ChaseState_ST_BTFSM(SmartTank_ST_BTFSM AiTank_FSM)
    {
        this.AiTank_FSM = AiTank_FSM;

    }
    public override Type StateEnter()
    {
        Debug.Log("Chase Entered.");
        AiTank_FSM.targetTankPosition = null;
        AiTank_FSM.consumablePosition = null;
        AiTank_FSM.basePosition = null;
        return null;

    }
    public override Type StateExit()
    {

        return null;

    }
    public override Type StateUpdate()
    {
        AiTank_FSM.EnemeyTankCheck();
        AiTank_FSM.HealthCheck();
        AiTank_FSM.AmmoCheck();
        AiTank_FSM.FuelCheck();
        AiTank_FSM.CollectableCheck();

        AiTank_FSM.targetTankPosition = AiTank_FSM.targetTanksFound.FirstOrDefault().Key;
        if (AiTank_FSM.targetTanksFound.FirstOrDefault().Key != null) //as long as target tank doesnt equal NULL chase
        {
            AiTank_FSM.EnemeyTankCheck(); //check and set the vlaues again
            AiTank_FSM.targetTankPosition = AiTank_FSM.targetTanksFound.FirstOrDefault().Key;
            //get closer to target, and fire
            if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) < 25f)//if its in a range of 25 or less fire
            {
                return typeof(AttackState_ST_BTFSM);
            }
            if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) > 45f)//if the range is greater then 45 roam.
            {
                return typeof(RoamState_ST_BTFSM);
            }
            else //Move towards the target.
            {
                AiTank_FSM.MoveTowardsObject(AiTank_FSM.targetTankPosition, 1f);
            }
        }
        else // if the list becomes NULL roam
        {
            return typeof(RoamState_ST_BTFSM);
        }

        return null;
    }
}

