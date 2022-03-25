using System;
using UnityEngine;

public class ChaseState : BaseState
{
    private SmartTank_ST_FSM smartTank_ST_FSM;

    public ChaseState(SmartTank_ST_FSM smartTank_ST_FSM)
    {
        this.smartTank_ST_FSM = smartTank_ST_FSM;
    }

    public override Type StateEnter()
    {
        return null;
    }

    public override Type StateExit()
    {
        return null;
    }

    public override Type StateUpdate()
    {
        
        return null;
    }
}
