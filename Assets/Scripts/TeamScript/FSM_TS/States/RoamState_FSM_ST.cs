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
        Debug.Log("eh");
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
        
       // AiTank_FSM.RandomPath(0.5f);

        /*if (AiTank_FSM.FuelCheck() < 75) // if the fuel is less then 75 move at half speed.
        {
            AiTank_FSM.RandomPath(0.25f);
            return null;
        } else
        {
            AiTank_FSM.RandomPath(0.5f);
        }
        if (AiTank_FSM.HealthCheck() < 50 && AiTank_FSM.consumablesFound.Count !=0) //if you have less and there is collecatbles near by go to it.
        {
            AiTank_FSM.consumablePosition = AiTank_FSM.consumablesFound.FirstOrDefault().Key;
            return typeof(CollectState_FSM_ST);
        }
        if (AiTank_FSM.FuelCheck() < 50 && AiTank_FSM.consumablesFound.Count != 0) // fuel less then 50 find fuel.
        {
            AiTank_FSM.consumablePosition = AiTank_FSM.consumablesFound.FirstOrDefault().Key;
            return typeof(CollectState_FSM_ST);
        }
        if (AiTank_FSM.AmmoCheck() < 4 && AiTank_FSM.consumablesFound.Count != 0)
        {
            AiTank_FSM.consumablePosition = AiTank_FSM.consumablesFound.FirstOrDefault().Key;
            return typeof(CollectState_FSM_ST);
        }
        if (AiTank_FSM.targetTanksFound.Count > 0 && AiTank_FSM.targetTanksFound.FirstOrDefault().Key != null && AiTank_FSM.HealthCheck()>50 && AiTank_FSM.AmmoCheck()>0) //If the enemy is in range and the health if above 50 and the ammount isnt 0 purse enemy/ enter chase.
        {
            AiTank_FSM.targetTankPosition = AiTank_FSM.targetTanksFound.FirstOrDefault().Key;
            return typeof(ChaseState_FSM_ST);
        }
        if(AiTank_FSM.basesFound.Count >0 && AiTank_FSM.basesFound.FirstOrDefault().Key != null)
        {
            AiTank_FSM.basePosition = AiTank_FSM.basesFound.FirstOrDefault().Key;
            return typeof(AttackState_FSM_ST);
        }
        else //when no other conditons are met move at a 0.5 speed.
        {
            return null;
        }*/

        if(AiTank_FSM.FuelCheck()> 75)
        {
            AiTank_FSM.RandomPath(0.5f);
        }
        if(AiTank_FSM.targetTanksFound.Count!=0 && AiTank_FSM.targetTanksFound.FirstOrDefault().Key != null)
        {
            Debug.Log("A");
            return typeof(ChaseState_FSM_ST);
        }
        if(AiTank_FSM.consumablesFound.Count!=0 && AiTank_FSM.consumablesFound.FirstOrDefault().Key != null)
        {
            Debug.Log("B");
            return typeof(CollectState_FSM_ST);
        }
        return null;


    }

}

