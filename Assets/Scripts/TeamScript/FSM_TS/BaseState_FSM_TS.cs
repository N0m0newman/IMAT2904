using System;

public abstract class BaseState_FSM_TS

{

    public abstract Type StateUpdate();

    public abstract Type StateEnter();

    public abstract Type StateExit();

}

