using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<AITank> aiTanks = new List<AITank>();

    private float gameStartTime = 3f;

    [HideInInspector]
    public bool gameStarted = false;


    private AStar aStar;

    private GameObject healthGameObject;
    private GameObject ammoGameObject;
    private GameObject fuelGameObject;

    List<GameObject> consumable = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        healthGameObject = transform.Find("Health").gameObject;
        ammoGameObject = transform.Find("Ammo").gameObject;
        fuelGameObject = transform.Find("Fuel").gameObject;
        aStar = GameObject.Find("AStarPlane").GetComponent<AStar>();

        consumable.Add(healthGameObject);
        consumable.Add(ammoGameObject);
        consumable.Add(ammoGameObject);
        consumable.Add(fuelGameObject);
        consumable.Add(fuelGameObject);
        consumable.Add(fuelGameObject);

        foreach (GameObject cons in consumable)
        {
            cons.SetActive(false);
        }

        StartCoroutine(GameStart(gameStartTime));
        StartCoroutine("GenerateRandomConsumable");

        GameObject[] aiTanksTemp = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < aiTanksTemp.Length; i++)
        {
            aiTanks.Add(aiTanksTemp[i].GetComponent<AITank>());
        }

    }
    IEnumerator GameStart(float gameStartTime)
    {
        yield return new WaitForSeconds(gameStartTime);
        gameStarted = true;
    }

    IEnumerator GenerateRandomConsumable()
    {
        foreach (GameObject cons in consumable)
        {
            cons.SetActive(false);
        }

        yield return new WaitForSeconds(Random.Range(2f, 10f));
        Node randomNode = aStar.NodePositionInGrid(new Vector3(Random.Range(-92, 92), 0, Random.Range(-92, 92)));
        Vector3 consPos = Vector3.zero;
        while (!randomNode.traversable || (randomNode.x > -35 && randomNode.x < 35) 
            || (randomNode.y > -37 && randomNode.y < 37))
        {
            randomNode = aStar.NodePositionInGrid(new Vector3(Random.Range(-95, 95), 0, Random.Range(-95, 95)));

            yield return new WaitForEndOfFrame();
        }
        consPos = randomNode.nodePos;

        int randCons = Random.Range(0, consumable.Count);

        consumable[randCons].transform.position = new Vector3(consPos.x, 5, consPos.z) ;

        yield return new WaitForSeconds(1f);

        consumable[randCons].SetActive(true);

        yield return new WaitForSeconds(Random.Range(45f, 60f));

        StartCoroutine("GenerateRandomConsumable");
    }

    public void ConsumableCollection()
    {
        StopCoroutine("GenerateRandomConsumable");
        StartCoroutine("GenerateRandomConsumable");
    }

    private void Update()
    {
        List<AITank> aiTanksTemp = aiTanks;

        for (int i = 0; i < aiTanksTemp.Count; i++)
        {
            if(aiTanksTemp[i] == null)
            {
                aiTanksTemp.RemoveAt(i);
            }
            if(aiTanksTemp.Count == 1)
            {
                break;
            }
        }

        if(aiTanksTemp.Count == 1)
        {
            print(aiTanksTemp[0].transform.parent.name + " Wins!");
            gameStarted = false;
        }
    }

}
