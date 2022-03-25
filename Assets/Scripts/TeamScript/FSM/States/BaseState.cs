using System;

public abstract class BaseState : SmartTank_ST_FSM
{
    public abstract Type StateUpdate();
    public abstract Type StateEnter();
    public abstract Type StateExit();
}