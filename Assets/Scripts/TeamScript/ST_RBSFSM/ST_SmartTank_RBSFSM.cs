using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_SmartTank_RBSFSM : AITank
{
    public ST_Rules rules = new ST_Rules();
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();


    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    public override void AITankStart()
    {
        stats.Add("patroling", true);
        stats.Add("chasingEnemy", false);
        stats.Add("collectingState", false);

        //Spotted information
        stats.Add("spottedAmmo", false);
        stats.Add("spottedFuel", false);
        stats.Add("spottedEnemy", false);
        stats.Add("spottedEnemyBase", false);
        stats.Add("spottedHealth", false);

        //Attacking
        stats.Add("attackingEnemyPlayer", false);
        stats.Add("attackingEnemyBase", false);
        stats.Add("inRangeOfEnemy", false);

        //flee rules
        stats.Add("lowAmmo", false);
        stats.Add("lowFuel", false);
        stats.Add("lowHealth", false);

        //Populating Rule Dictionary
        rules.AddRule(new ST_Rule("patroling", "spottedEnemy", typeof(ChaseState_FSM_ST), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("attackingEnemyBase", "spottedEnemy", typeof(ChaseState_FSM_ST), ST_Rule.Predicate.AND));

        rules.AddRule(new ST_Rule("attackingEnemyPlayer", "lowAmmo", typeof(ST_RBSFSM_RoamState), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("chasingEnemy", "lowFuel", typeof(ST_RBSFSM_RoamState), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("lowHealth", "lowAmmo", typeof(ST_RBSFSM_RoamState), ST_Rule.Predicate.OR));
        rules.AddRule(new ST_Rule("attackingEnemyPlayer", "lowHealth", typeof(ST_RBSFSM_RoamState), ST_Rule.Predicate.AND));

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState_FSM_TS> states = new Dictionary<Type, BaseState_FSM_TS>();

        states.Add(typeof(ST_RBSFSM_RoamState), new ST_RBSFSM_RoamState(this));
        states.Add(typeof(ST_RBSFSM_ChaseState), new ST_RBSFSM_ChaseState(this));
        states.Add(typeof(ST_RBSFSM_AttackBaseState), new ST_RBSFSM_AttackBaseState(this));
        states.Add(typeof(ST_RBSFSM_AttackState), new ST_RBSFSM_AttackState(this));
        states.Add(typeof(ST_RBSFSM_CollectState), new ST_RBSFSM_CollectState(this));

        GetComponent<StateMachine_FSM_ST>().SetStates(states);
    }

    public void GotoRandomLocation(float normalizedSpeed)
    {
        FollowPathToRandomPoint(normalizedSpeed);
    }

    public override void AITankUpdate()
    {

        stats["spottedFuel"] = false;
        stats["spottedAmmo"] = false;
        stats["spottedAmmo"] = false;

        // tank found
        stats["spottedEnemy"] = (GetAllTargetTanksFound.Count != 0 && GetAllTargetTanksFound.FirstOrDefault().Key != null) ? true : false;
        //enemy base found
        stats["spottedEnemyBase"] = (GetAllBasesFound.Count != 0 && GetAllBasesFound.FirstOrDefault().Key != null) ? true : false;
        //player is low health
        stats["lowHealth"] = (GetHealthLevel < 40) ? true : false;
        if(targetTankPosition != null)
        {
            stats["inRangeOfEnemy"] = (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 25f) ? true : false;
        } else
        {
            stats["inRangeOfEnemy"] = false;
        }
        
        //consumable found
        if (GetAllConsumablesFound.Count != 0 && GetAllConsumablesFound.FirstOrDefault().Key != null)
        {//gameobject object, float distance
            foreach (KeyValuePair<GameObject, float> obj in GetAllConsumablesFound)
            {
                switch (obj.Key.name)
                {
                    case "Fuel":
                        stats["spottedFuel"] = true;
                        break;
                    case "Ammo":
                        stats["spottedAmmo"] = true;
                        break;
                    case "Health":
                        stats["spottedHealth"] = true;
                        break;
                }
            }
        }
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        foreach (KeyValuePair<GameObject, float> obj in GetAllConsumablesFound)
        {
            switch (obj.Key.name)
            {
                case "Fuel":
                    stats["spottedFuel"] = false;
                    break;
                case "Ammo":
                    stats["spottedAmmo"] = false;
                    break;
                case "Health":
                    stats["spottedHealth"] = false;
                    break;
            }
        }
    }

    #region Wrapper

    public void FollowTarget(GameObject target, float speed)
    {
        FollowPathToPoint(target, speed);
    }

    public float GetAmmo()
    {
        return GetAmmoLevel;
    }

    public float GetFuel()
    {
        return GetFuelLevel;
    }

    public float GetHealth()
    {
        return GetHealthLevel;
    }

    public void FireTank(GameObject targetPosition)
    {
        FireAtPoint(targetPosition);
    }

    public void GetBasesFound()
    {
        basesFound = GetAllBasesFound;
    }

    public void GetCollectablesFound()
    {
        consumablesFound = GetAllConsumablesFound;
    }

    public Dictionary<GameObject, float> GetEnemysFound()
    {
        targetTanksFound = GetAllTargetTanksFound;
        return targetTanksFound;
    }

    #endregion

}
