using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank_ST_BTFSM : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    //BT Nodes
    public BTActionNode lowHealthCheck;
    public BTActionNode lowAmmoCheck;
    public BTActionNode lowFuelCheck;
    public BTActionNode consumableSpottedCheck;
    public BTActionNode consumableReachedCheck;
    public BTSequence restockSequence;


    public bool ammoCollected;
    public bool healthCollected;
    public bool fuelCollected;

    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
        Application.targetFrameRate = 60;
        //This method runs once at the beginning when pressing play in Unity.
        InitializeStateMachine();

       // consumableSpottedCheck = new BTActionNode(ConsumableSpottedCheck);
       // consumableReachedCheck = new BTActionNode(ConsumableReachedCheck);
        lowFuelCheck = new BTActionNode(LowFuelCheck);
        lowHealthCheck = new BTActionNode(LowHealthCheck);
        lowAmmoCheck = new BTActionNode(LowAmmoCheck);
        restockSequence = new BTSequence(new List<BTBaseNode> { lowFuelCheck, lowHealthCheck, lowAmmoCheck });

    }
    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState_FSM_TS> states = new Dictionary<Type, BaseState_FSM_TS>();

        states.Add(typeof(RoamState_ST_BTFSM), new RoamState_ST_BTFSM(this));
        states.Add(typeof(ChaseState_ST_BTFSM), new ChaseState_ST_BTFSM(this));
        states.Add(typeof(AttackState_ST_BTFSM), new AttackState_ST_BTFSM(this));
        states.Add(typeof(CollectState_ST_BTFSM), new CollectState_ST_BTFSM(this));

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

        if (collision.gameObject.tag == "Ammo")
        {
            ammoCollected = true;
        }
        if (collision.gameObject.tag == "Health")
        {
            healthCollected = true;
        }
        if (collision.gameObject.tag == "Fuel")
        {
            fuelCollected = true;
        }

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

    public void FindConsumable()
    {
        if (consumablesFound.Count > 0)
        {
            Debug.Log("HeadingToConsumable");
            //found consumable
            FollowPathToPoint(consumablePosition, 0.5f);
        }
        else 
        {
            //back to random path
            Debug.Log("FindingConsumable");
            RandomPath(0.25f);
        }
    }

    public BTNodeStates LowHealthCheck()
    {
        if (GetHealthLevel <= 25)
        {
            Debug.Log("LOWHEALTH");
            FindConsumable();
            return BTNodeStates.FAILURE;
        }
        else
        {
            return BTNodeStates.SUCCESS;
        }

    }

    public BTNodeStates LowAmmoCheck()
    {
        if (GetAmmoLevel == 0)
        {
            Debug.Log("LOWAMMO");
            FindConsumable();
            return BTNodeStates.FAILURE;
        }
        else
        {
            return BTNodeStates.SUCCESS;
        }

    }

    public BTNodeStates LowFuelCheck()
    {
        if (GetFuelLevel <= 20)
        {
            Debug.Log("LOWFUEL");
            FindConsumable();
            return BTNodeStates.FAILURE;
        }
        else
        {
            return BTNodeStates.SUCCESS;
        }

    }
   /* public BTNodeStates ConsumableSpottedCheck()
    {
        if (consumablesFound.Count > 0)
        {
            return BTNodeStates.SUCCESS;
        }
        else
        {
            FindConsumable();
            return BTNodeStates.FAILURE;
        }
    }

    public BTNodeStates ConsumableReachedCheck()
    {
        if (fuelCollected == true)
        {
            return BTNodeStates.SUCCESS;
        }
        if (healthCollected == true)
        {
            return BTNodeStates.SUCCESS;
        }
        if (ammoCollected == true)
        {
            return BTNodeStates.SUCCESS;
        }
        else
        {
            return BTNodeStates.FAILURE;
        }
    }
 */
}

