using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RoamState_ST_BTFSM : BaseState_FSM_TS
{

    private SmartTank_ST_BTFSM AiTank_FSM;
    public RoamState_ST_BTFSM(SmartTank_ST_BTFSM AiTank_FSM)

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
            AiTank_FSM.LowHealthCheck();
            AiTank_FSM.LowAmmoCheck();
            AiTank_FSM.LowFuelCheck();
            AiTank_FSM.CollectableCheck();
            AiTank_FSM.BaseFound();
            AiTank_FSM.basePosition = AiTank_FSM.basesFound.FirstOrDefault().Key;

        if (AiTank_FSM.restockSequence.Evaluate() == BTNodeStates.SUCCESS)
        { 

            if (AiTank_FSM.FuelCheck() >= 60) //if the fuel is greater then or eqaul to 60 move at 0.5 speed
            {
                AiTank_FSM.RandomPath(0.5f);
            }
            if (AiTank_FSM.FuelCheck() < 60) //if the fuel is less then 60 move at 0.25 speed.
            {
                AiTank_FSM.RandomPath(0.25f);
            }
            if (AiTank_FSM.targetTanksFound.Count != 0 && AiTank_FSM.targetTanksFound.FirstOrDefault().Key != null) //If there is a target tank in range enter the chase.
            {
                Debug.Log("A");
                return typeof(ChaseState_ST_BTFSM);
            }
            if (AiTank_FSM.consumablesFound.Count != 0 && AiTank_FSM.consumablesFound.FirstOrDefault().Key != null) //if there is consumables in range enter the collecatble.
            {
                Debug.Log("B");
                return typeof(CollectState_ST_BTFSM);
            }
            if (AiTank_FSM.basesFound.Count != 0 && AiTank_FSM.basesFound.FirstOrDefault().Key != null) // if there is a enemy base fire at it.
            {
                if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.basePosition.transform.position) < 25f)
                {
                    Debug.Log("C");
                    AiTank_FSM.FireTank(AiTank_FSM.basePosition);
                }
                else
                {
                    AiTank_FSM.MoveTowardsObject(AiTank_FSM.basePosition, 0.5f);
                }
            }
            }
        return null;

        }



}

