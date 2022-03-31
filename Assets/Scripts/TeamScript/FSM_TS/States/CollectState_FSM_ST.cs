using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CollectState_FSM_ST : BaseState_FSM_TS
{

    private SmartTank_FSM_ST AiTank_FSM;
    public CollectState_FSM_ST(SmartTank_FSM_ST AiTank_FSM)

    {

        this.AiTank_FSM = AiTank_FSM;

    }
    public override Type StateEnter()

    {
        Debug.Log("collect" + AiTank_FSM.HealthCheck());
 
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

        AiTank_FSM.consumablePosition = AiTank_FSM.consumablesFound.FirstOrDefault().Key;

        if (AiTank_FSM.consumablesFound.Count > 0)
        {
            AiTank_FSM.MoveTowardsObject(AiTank_FSM.consumablePosition, 0.5f);
            if (AiTank_FSM.HealthCheck() > 50)
            {
                return null;
            }
            if(AiTank_FSM.AmmoCheck() == 0)
            {
                return null;
            }
            /*if (AiTank_FSM.FuelCheck() < 50)
            {
                return typeof (RoamState_FSM_ST);
            }*/

        }
        else
        {
            AiTank_FSM.targetTankPosition = null;
            AiTank_FSM.consumablePosition = null;
            AiTank_FSM.basePosition = null;
            return typeof(RoamState_FSM_ST);
        }
        return null;
    }

}

