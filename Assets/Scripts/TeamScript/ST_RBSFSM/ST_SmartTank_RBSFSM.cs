using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_SmartTank_RBSFSM : AITank
{
    public ST_Rules rules = new ST_Rules();
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();


    public override void AITankStart()
    {
        stats.Add("patroling", true);
        stats.Add("chasingEnemy", false);

        //Spotted information
        stats.Add("spottedAmmo", false);
        stats.Add("spottedFuel", false);
        stats.Add("spottedEnemy", false);
        stats.Add("spottedEnemyBase", false);

        //Attacking
        stats.Add("attackingEnemyPlayer", false);
        stats.Add("attackingEnemyBase", false);

        //flee rules
        stats.Add("lowAmmo", false);
        stats.Add("lowFuel", false);
        stats.Add("lowHealth", false);

        //Populating Rule Dictionary
        //Patrolling and spotted an enemy chase them
        rules.AddRule(new ST_Rule("patroling", "spottedEnemy", typeof(ChaseState_FSM_ST), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("attackingEnemyPlayer", "lowAmmo", typeof(RoamState_FSM_ST), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("chasingEnemy", "lowFuel", typeof(RoamState_FSM_ST), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("lowHealth", "lowAmmo", typeof(RoamState_FSM_ST), ST_Rule.Predicate.OR));
        rules.AddRule(new ST_Rule("attackingEnemyBase", "spottedEnemy", typeof(ChaseState_FSM_ST), ST_Rule.Predicate.AND));
        rules.AddRule(new ST_Rule("attackingEnemyPlayer", "lowHealth", typeof(RoamState_FSM_ST), ST_Rule.Predicate.AND));
    }

    public void GotoRandomLocation(float normalizedSpeed)
    {
        FollowPathToRandomPoint(normalizedSpeed);
    }
    
    public List<float> GetEquipmentUpdate()
    {
        List<float> temp = new List<float>();
        temp.Add(GetFuelLevel);
        temp.Add(GetHealthLevel);
        temp.Add(GetAmmoLevel);
        return temp;
    }

    

    public override void AITankUpdate()
    {

        throw new System.NotImplementedException();
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
        throw new System.NotImplementedException();
    }
}
