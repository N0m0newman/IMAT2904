using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AttackState_FSM_ST : BaseState_FSM_TS
{

    private SmartTank_FSM_ST AiTank_FSM;
    public AttackState_FSM_ST(SmartTank_FSM_ST AiTank_FSM)

    {

        this.AiTank_FSM = AiTank_FSM;

    }
    public override Type StateEnter()

    {
        Debug.Log("Attack" + AiTank_FSM.HealthCheck());
        return null;

    }
    public override Type StateExit()
    {
        AiTank_FSM.targetTankPosition = null;
        AiTank_FSM.consumablePosition = null;
        AiTank_FSM.basePosition = null;
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

        AiTank_FSM.targetTankPosition = AiTank_FSM.targetTanksFound.FirstOrDefault().Key;

        AiTank_FSM.basePosition = AiTank_FSM.basesFound.FirstOrDefault().Key;


        if (AiTank_FSM.targetTanksFound.Count !=0) //check if still in range.
        {
            if(Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) < 25f)
            {
                AiTank_FSM.FireTank(AiTank_FSM.targetTankPosition);
            }
            else
            {
                return typeof(ChaseState_FSM_ST);
            }

        }
        if (AiTank_FSM.basesFound.Count() !=0) 
        {
            if(Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.basePosition.transform.position) < 25f && AiTank_FSM.AmmoCheck() != 0)
            {
                AiTank_FSM.FireTank(AiTank_FSM.basePosition);
                return typeof(RoamState_FSM_ST);
            }
            else
            {
                return typeof(RoamState_FSM_ST);
            }
        }
        if(AiTank_FSM.basesFound.Count == 0)
        {
            return typeof(RoamState_FSM_ST);
        }
        if (AiTank_FSM.HealthCheck() > 50) //if tanks health is greater then 50 fire again.
        {
            return null;
        }
        if (AiTank_FSM.HealthCheck() < 50) // if health becomes less then 50 enter collecatble state to try and find health.
        {
            return typeof(CollectState_FSM_ST);
        }
        if (AiTank_FSM.AmmoCheck() == 0) //if ammo count becomes 0 enter collecabtle state to try and find ammo
        {
            return typeof(CollectState_FSM_ST);
        }
        else //if the target moces out of range chase after target.
        {
            return typeof(RoamState_FSM_ST);
        }
    }

}

