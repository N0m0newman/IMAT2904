//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ST_RBSFSM_AttackState : BaseState_FSM_TS
//{
//    private ST_SmartTank_RBSFSM smartTank;
//    public override Type StateEnter()
//    {
//        return null;
//    }

//    public override Type StateExit()
//    {

//        return null;
//    }

//    public override Type StateUpdate()
//    {
//        AiTank_FSM.EnemeyTankCheck();
//        AiTank_FSM.HealthCheck();
//        AiTank_FSM.AmmoCheck();
//        AiTank_FSM.FuelCheck();
//        AiTank_FSM.CollectableCheck();



//        if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.targetTankPosition.transform.position) < 25f) //check if still in range.
//        {
//            AiTank_FSM.FireTank(AiTank_FSM.targetTankPosition);
//            ai
//        }
//        if (AiTank_FSM.basePosition != null)
//        {
//            go close to it and fire
//            if (Vector3.Distance(AiTank_FSM.transform.position, AiTank_FSM.basePosition.transform.position) < 25f)
//            {
//                AiTank_FSM.FireTank(AiTank_FSM.basePosition);
//            }
//            else
//            {
//                AiTank_FSM.MoveTowardsObject(AiTank_FSM.basePosition, 1f);
//            }
//        }
//        else //if the target moces out of range chase after target.
//        {
//            return typeof(RoamState_FSM_ST);
//        }
//        return null;
//    }
//}
