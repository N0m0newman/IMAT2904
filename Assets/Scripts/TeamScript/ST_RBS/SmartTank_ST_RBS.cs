using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartTank_ST_RBS : AITank
{
    public Dictionary<string, bool> stats = new Dictionary<string, bool>();
    public ST_Rules rules = new ST_Rules();

    public override void AIOnCollisionEnter(Collision collision)
    {
        
    }

    public override void AITankStart()
    {
        stats.Add("patroling", false);
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
        stats.Add("fleeAmmo", false);
        stats.Add("fleeFuel", false);
        stats.Add("fleeHealth", false);

        //rules.AddRule(new ST_Rule("patroling", "spottedEnemy"))

    }

    public override void AITankUpdate()
    {
        
    }
}
