using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank_FSM_ST : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    

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
        Dictionary<Type, BaseState_FSM_TS> states = new Dictionary<Type, BaseState_FSM_TS>();

        states.Add(typeof(RoamState_FSM_ST), new RoamState_FSM_ST(this));
        states.Add(typeof(ChaseState_FSM_ST), new ChaseState_FSM_ST(this));
        states.Add(typeof(AttackState_FSM_ST), new AttackState_FSM_ST(this));
        states.Add(typeof(CollectState_FSM_ST), new CollectState_FSM_ST(this));

        GetComponent<StateMachine_FSM_ST>().SetStates(states);

        StateMachine_FSM_ST temp = GetComponent<StateMachine_FSM_ST>();
        temp.SetStates(states);
        Debug.Log(states.Values);
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
    public float FuelCheck()
    {
        float fuel = GetFuelLevel;
        return fuel;
    }
    public float HealthCheck()
    {
        float health = GetHealthLevel;
        return health;
    }
    public float AmmoCheck()
    {
        float ammo = GetAmmoLevel;
        return ammo;
    }
    public void EnemeyTankCheck()
    {
        targetTanksFound = GetAllTargetTanksFound;
    }
    public void CollectableCheck()
    {
        consumablesFound = GetAllConsumablesFound;
    }
    public void BaseFound()
    {
        basesFound = GetAllBasesFound;
    }
    public void MoveTowardsObject(GameObject followThis, float normalizedSpeed)
    {

        FollowPathToPoint(followThis, normalizedSpeed);
    }
    public void FireTank(GameObject targetTankPosition)
    {
        FireAtPoint(targetTankPosition);
    }
}

