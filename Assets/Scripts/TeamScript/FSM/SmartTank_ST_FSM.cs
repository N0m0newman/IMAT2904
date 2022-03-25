using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank_ST_FSM : AITank
{
    
    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
        //This method runs once at the beginning when pressing play in Unity.
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>();

        states.Add(typeof(RoamState), new RoamState(this));
        states.Add(typeof(ChaseState), new ChaseState(this));
        states.Add(typeof(AttackState), new AttackState(this));

        GetComponent<StateMechine>().SetStates(states);
    }

    /*******************************************************************************************************       
    WARNING, do not include void Update(), use AITankUpdate() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankUpdate()
    {
        //This method runs once per frame.
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
        //This method is used for detecting collisions (unlikley you will need this).
    }
    public void RandomPath(float normalizedSpeed)
    {
        FollowPathToRandomPoint(normalizedSpeed);
    }
    public void StatusCheck(float Health, float Ammo, float Fuel)
    {
    }
}
