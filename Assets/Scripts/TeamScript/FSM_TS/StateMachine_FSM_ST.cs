using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_FSM_ST : MonoBehaviour
{
    private Dictionary<Type, BaseState_FSM_TS> states;

    [SerializeField]
    public BaseState_FSM_TS currentState;

    public BaseState_FSM_TS CurrentState
    {
        get
        {

            return currentState;

        }
        private set
        {
            currentState = value;

        }
    }
    public void SetStates(Dictionary<Type, BaseState_FSM_TS> states)
    {
        this.states = states;
    }
    void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = states.Values.First();
        }
        else
        {
            var nextState = CurrentState.StateUpdate();
            if(nextState != null && nextState != CurrentState.GetType())
            {
                SwitchToState(nextState);
            }
        }
    }
    public void SwitchToState(Type nextState)
    {
        CurrentState.StateExit();

        CurrentState = states[nextState];

        CurrentState.StateEnter();
    }

}

