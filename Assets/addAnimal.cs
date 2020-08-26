using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class addAnimal : MonoBehaviour
{
    private Stats stats;
    public GameObject[] animalPrefabs;
    public Transform spawnPt;
    public Transform animalParent;
    public Animation animation;

    void Start()
    {
        stats = FindObjectOfType<Stats>();
        animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    private void OnMouseDown()
    {
        Vector3 spawnPosition = RandomNavSphere(spawnPt.position, 2, -1);
        GameObject animal = Instantiate(animalPrefabs[Random.Range(0, animalPrefabs.Length)], spawnPosition, Quaternion.identity, animalParent);
        animation["addAnimal"].wrapMode = WrapMode.Once;
        animation.Play("addAnimal");

    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
