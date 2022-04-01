using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ST_SmartTank_RBSFSM : AITank
{
    public ST_Rules rules = new ST_Rules();
    [SerializeField]
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();


    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    public override void AITankStart()
    {
        Application.targetFrameRate = 60;

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
        stats.Add("inRangeOfEnemyBase", false);
        stats.Add("tooCloseToEnemy", false);

        //flee rules
        stats.Add("lowAmmo", false);
        stats.Add("lowFuel", false);
        stats.Add("lowHealth", false);

        //Populating Rule Dictionary
        rules.AddRule(new ST_Rule("patroling", "spottedEnemy", typeof(ST_RBSFSM_ChaseState), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("chasingEnemy", "inRangeOfEnemy", typeof(ST_RBSFSM_AttackState), ST_Rule.Predicate.AND));


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
        GetBasesFound();
        GetCollectablesFound();
        GetEnemysFound();


        //reset all the collectables
        stats["spottedFuel"] = false;
        stats["spottedAmmo"] = false;
        stats["spottedAmmo"] = false;

        //Is low ammo
        stats["lowAmmo"] = (GetAmmoLevel < 2) ? true : false;
        //Is low fuel
        stats["lowFuel"] = (GetFuelLevel < 40) ? true : false;
        // tank found
        stats["spottedEnemy"] = (GetAllTargetTanksFound.Count != 0 && GetAllTargetTanksFound.FirstOrDefault().Key != null) ? true : false;
        //enemy base found
        stats["spottedEnemyBase"] = (GetAllBasesFound.Count != 0 && GetAllBasesFound.FirstOrDefault().Key != null) ? true : false;
        //player is low health
        stats["lowHealth"] = (GetHealthLevel < 40) ? true : false;
        if(targetTankPosition != null)
        {
            stats["inRangeOfEnemy"] = (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 30f) ? true : false;
            stats["tooCloseToEnemy"] = (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 25f) ? true : false;
        } else
        {
            stats["inRangeOfEnemy"] = false;
            stats["tooCloseToEnemy"] = false;
        }
        
        if(basePosition != null)
        {
            stats["inRangeOfEnemyBase"] = (Vector3.Distance(transform.position, basePosition.transform.position) < 20f) ? true : false;
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

        Debug.Log("is Close: " + stats["inRangeOfEnemy"]);
        Debug.Log("is Chasing: " + stats["chasingEnemy"]);

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

    public void AttackPlayer()
    {
        targetTankPosition = targetTanksFound.FirstOrDefault().Key;
        if (targetTanksFound.Count != 0)
        {
            if (stats["inRangeOfEnemy"] == true && stats["lowAmmo"] != true)
            {
                FireTank(targetTankPosition);
            }
        }
    }

    public void ChasePlayer()
    { 
        targetTankPosition = targetTanksFound.FirstOrDefault().Key;
        if (targetTanksFound.FirstOrDefault().Key != null)
        {
            if (stats["inRangeOfEnemy"] == false)
            {
                Debug.Log("l");
                FollowTarget(targetTankPosition, 1f);
                stats["inRangeOfEnemy"] = (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 30f) ? true : false;
                Debug.Log(stats["inRangeOfEnemy"]);
            }
            else
            {
                //AttackPlayer();
            }
        }
        else
        {
            Debug.Log("o");
            stats["chasingEnemy"] = false;
            stats["patrolling"] = true;
        }
    }

    public void AttackBase()
    {
        basePosition = basesFound.FirstOrDefault().Key;
        if (basesFound.Count != 0)
        {
            if (stats["lowAmmo"] != true)
            {
                if (stats["inRangeOfEnemyBase"])
                {
                    FireTank(basePosition);
                }
                else
                {
                    FollowTarget(basePosition.gameObject, 1f);
                }
            }
            else
            {
                stats["attackingEnemyBase"] = false;
                stats["patrolling"] = true;
            }
        }
        else
        {
            stats["patrolling"] = true;
            stats["attackingEnemyBase"] = false;
        }
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
