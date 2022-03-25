using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DumbTank : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    float t;

    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
    }

    /*******************************************************************************************************       
    WARNING, do not include void Update(), use AITankUpdate() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankUpdate()
    {
        //Get the targets found from the sensor view
        targetTanksFound = GetAllTargetTanksFound;
        consumablesFound = GetAllConsumablesFound;
        basesFound = GetAllBasesFound;


        //if low health or ammo, go searching
        if (GetHealthLevel < 50 || GetAmmoLevel < 5)
        {
            if (consumablesFound.Count > 0)
            {
                consumablePosition = consumablesFound.First().Key;
                FollowPathToPoint(consumablePosition, 1f);
                t += Time.deltaTime;
                if (t > 10)
                {
                    GenerateRandomPoint();
                    t = 0;
                }
            }
            else
            {
                targetTankPosition = null;
                consumablePosition = null;
                basePosition = null;
                FollowPathToRandomPoint(1f);
            }
        }
        else
        {
            //if there is a target found
            if (targetTanksFound.Count > 0 && targetTanksFound.First().Key != null)
            {
                targetTankPosition = targetTanksFound.First().Key;
                if(targetTankPosition != null)
                {
                    //get closer to target, and fire
                    if (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 25f)
                    {
                        FireAtPoint(targetTankPosition);
                    }
                    else
                    {
                        FollowPathToPoint(targetTankPosition, 1f);
                    }
                }
            }
            else if (consumablesFound.Count > 0)
            {
                //if consumables are found, go to it.
                consumablePosition = consumablesFound.First().Key;
                FollowPathToPoint(consumablePosition, 1f);
            }
            else if (basesFound.Count > 0 )
            {
                //if base if found
                basePosition = basesFound.First().Key;
                if (basePosition != null)
                {
                    //go close to it and fire
                    if (Vector3.Distance(transform.position, basePosition.transform.position) < 25f)
                    {
                        FireAtPoint(basePosition);
                    }
                    else
                    {
                        FollowPathToPoint(basePosition, 1f);
                    }
                }
            }
            else
            {
                //searching
                targetTankPosition = null;
                consumablePosition = null;
                basePosition = null;
                FollowPathToRandomPoint(1f);
                t += Time.deltaTime;
                if (t > 10)
                {
                    GenerateRandomPoint();
                    t = 0;
                }
            }
        }
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
    }
}
