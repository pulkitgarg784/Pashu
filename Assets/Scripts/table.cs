using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class table : MonoBehaviour
{
    public Transform[] foodSpawnPoints;
    public Transform[] waterSpawnPoints;
    public GameObject food;
    public GameObject water;
    private ManController man;
    public bool isOccupied;
    Stats stats;
    public int itemCost;
    void Start()
    {
        stats = FindObjectOfType<Stats>();
        for (int i = 0; i < foodSpawnPoints.Length; i++)
        {
            if (foodSpawnPoints[i].childCount == 0)
            {
                GameObject foodObj = Instantiate(food, foodSpawnPoints[i].position, Quaternion.identity, foodSpawnPoints[i]);
                foodObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
        }
        for (int i = 0; i < waterSpawnPoints.Length; i++)
        {
            if (waterSpawnPoints[i].childCount == 0)
            {
                GameObject waterObj = Instantiate(water, waterSpawnPoints[i].position, Quaternion.identity, waterSpawnPoints[i]);
                waterObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < foodSpawnPoints.Length; i++)
        {
            if (foodSpawnPoints[i].childCount == 0 || waterSpawnPoints[i].childCount == 0)
            {
                if (findClosestMan() != null)
                {
                    man = findClosestMan().GetComponent<ManController>();
                    man.agent.SetDestination(transform.position);
                    Debug.Log("going to table");

                    if (Vector3.Distance(man.transform.position, transform.position) <= 2)
                    {
                        Debug.Log("Refilling");
                        isOccupied = true;
                        SpawnObjects();
                        StartCoroutine(SpawnObjects());
                    }
                }
            }

        }


    }

    public IEnumerator SpawnObjects()
    {
        man.busy = true;
        yield return new WaitForSeconds(10);
        for (int i = 0; i < foodSpawnPoints.Length; i++)
        {
            if (foodSpawnPoints[i].childCount == 0)
            {
                GameObject foodObj = Instantiate(food, foodSpawnPoints[i].position, Quaternion.identity, foodSpawnPoints[i]);
                stats.Money -= itemCost;
                foodObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
        }
        for (int i = 0; i < waterSpawnPoints.Length; i++)
        {
            if (waterSpawnPoints[i].childCount == 0)
            {
                GameObject waterObj = Instantiate(water, waterSpawnPoints[i].position, Quaternion.identity, waterSpawnPoints[i]);
                stats.Money -= itemCost;
                waterObj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
        }
        man.busy = false;

    }

    GameObject findClosestMan()
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Man");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<ManController>() != null)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }

        return closest;
    }


}
