using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RoamState_FSM_ST : BaseState_FSM_TS
{

    private SmartTank_FSM_ST AiTank_FSM;
    public RoamState_FSM_ST(SmartTank_FSM_ST AiTank_FSM)

    {

        this.AiTank_FSM = AiTank_FSM;

    }

    public override Type StateEnter()
    {
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
        AiTank_FSM.BaseFound();

        if(AiTank_FSM.FuelCheck()>=60) //if the fuel is greater then or eqaul to 60 move at 0.5 speed
        {
            AiTank_FSM.RandomPath(0.5f);
        }
        if (AiTank_FSM.FuelCheck() < 60) //if the fuel is less then 60 move at 0.25 speed.
        {
            AiTank_FSM.RandomPath(0.25f);
        }
        if(AiTank_FSM.targetTanksFound.Count!=0 && AiTank_FSM.targetTanksFound.FirstOrDefault().Key != null) //If there is a target tank in range enter the chase.
        {
            Debug.Log("A");
            return typeof(ChaseState_FSM_ST);
        }
        if(AiTank_FSM.consumablesFound.Count!=0 && AiTank_FSM.consumablesFound.FirstOrDefault().Key != null) //if there is consumables in range enter the collecatble.
        {
            Debug.Log("B");
            return typeof(CollectState_FSM_ST);
        }
        if (AiTank_FSM.basesFound.Count != 0 && AiTank_FSM.consumablesFound.FirstOrDefault().Key != null) // if there is a enemy base fire at it.
        {
            Debug.Log("C");
            AiTank_FSM.FireTank(AiTank_FSM.basePosition);
        }
        return null;


    }

}

