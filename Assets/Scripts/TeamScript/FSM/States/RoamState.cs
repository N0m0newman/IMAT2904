using System;
using UnityEngine;

public class RoamState : BaseState
{
    private SmartTank_ST_FSM smartTank_ST_FSM;

    public RoamState(SmartTank_ST_FSM smartTank_ST_FSM)
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
        if (GetHealthLevel < 50)
        {
            return null;
        }
        if(GetFuelLevel < 50)
        {
            smartTank_ST_FSM.RandomPath(0.25f);
            return (null);//S
        }
        smartTank_ST_FSM.RandomPath(0.5f);
        return null;
    }
}
