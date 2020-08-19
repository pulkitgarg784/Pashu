using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class RandomAnimalPlacer : MonoBehaviour
{
    [SerializeField] float spawnSize;
    [SerializeField] int spawnAmmount;

    [SerializeField] GameObject[] animals;

    [ContextMenu("Spawn Animals")]
    void SpawnAnimals()
    {
		var parent = new GameObject("SpawnedAnimals");

        for (int i = 0; i < spawnAmmount; i++)
        {
            var value = Random.Range(0, animals.Length);

            Instantiate(animals[value], RandomNavmeshLocation(spawnSize), Quaternion.identity, parent.transform);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnSize);
    }
}
