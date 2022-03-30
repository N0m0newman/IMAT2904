using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChaseState_FSM_ST : BaseState_FSM_TS
{

    private SmartTank_FSM_ST AiTank_FSM;
    public ChaseState_FSM_ST(SmartTank_FSM_ST AiTank_FSM)

    {
        Debug.Log("Chase" + AiTank_FSM.HealthCheck());
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
        AiTank_FSM.MoveTowardsObject(AiTank_FSM.targetTankPosition, 0.25f); // move towards target and fire at the target.
        if (AiTank_FSM.targetTankPosition != null)
        {
            //get closer to target, and fire
            if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) < 25f) //if the distance btween our tank and enmy
            {
                return typeof(AttackState_FSM_ST);
            }
            else if(Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) > 35f)
                {
                return typeof(RoamState_FSM_ST);
            }
        }

        return null;

    }

}

