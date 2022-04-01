using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AttackState_ST_BTFSM : BaseState_FSM_TS
{

    private SmartTank_ST_BTFSM AiTank_FSM;
    public AttackState_ST_BTFSM(SmartTank_ST_BTFSM AiTank_FSM)

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


            if (AiTank_FSM.targetTanksFound.Count != 0) //check if still in range.
            {
                AiTank_FSM.EnemeyTankCheck();
                if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) < 25f && AiTank_FSM.AmmoCheck() != 0) //if the distance is 25 or less fire and we have ammo.
                {
                    AiTank_FSM.FireTank(AiTank_FSM.targetTankPosition);
                    AiTank_FSM.EnemeyTankCheck(); //once fired check possion if out of range exist state.
                    if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) > 50f) // if the range gets higeher then 50 enter roam.
                    {
                        return typeof(RoamState_ST_BTFSM);

                    }
                }
                else//if it falls anywhere in btween chase.
                {
                    return typeof(ChaseState_ST_BTFSM);
                }

            }
            else //if the target moces out of range chase after target.
            {
                return typeof(RoamState_ST_BTFSM);
            }
        return null;

    }

}

