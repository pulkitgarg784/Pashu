using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalController : MonoBehaviour
{
    public Material[] skins;
    public Renderer meshRenderer;
    public enum State
    {
        Wander,
        goToFood,
        goToWater,
        Dead
    }
    public State currentState;

    public NavMeshAgent agent;
    Vector3 target;
    public Animator animator;
    public bool rotateOnDeath;
    public float runSpeed;
    public float walkSpeed;

    //Wander
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    //Stats
    [Header("Stats:")]
    public float health = 100;
    public float water = 100;
    private float healthReductionSpeed = 2;
    private void OnEnable()
    {
        if (health == 0) { health = 100; }
        if (water == 0) { water = 100; }
        healthReductionSpeed = Random.Range(1.0f, 2.0f);
    }
    void Start()
    {
        meshRenderer.material = skins[Random.Range(0, skins.Length)];

        currentState = State.Wander;

    }

    // Update is called once per frame
    void Update()
    {
        health -= Time.deltaTime * healthReductionSpeed;
        water -= Time.deltaTime * (healthReductionSpeed / 2);
        if (health <= 0 || water <= 0)
        {
            currentState = State.Dead;
            health = 0;
            water = 0;
            Debug.Log(transform.name + " died");
            animator.SetBool("isSleeping", true);
            agent.enabled = false;
            if (rotateOnDeath)
            {
                if (transform.rotation.eulerAngles.z <= 90)
                {
                    transform.rotation *= Quaternion.Euler(0, 0, 90f * 2 * Time.deltaTime);
                }
            }
            Destroy(gameObject, 5);
            return;
        }
        if (health < 50)
        {
            currentState = State.goToFood;
        }
        if (water < 50)
        {
            currentState = State.goToWater;
        }
        if (currentState == State.Wander)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                target = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(target);
                timer = 0;
                wanderTimer += Random.Range(-3, 3);
                wanderTimer = Mathf.Clamp(wanderTimer, 5, 15);
            }
        }

        if (currentState == State.goToFood)
        {
            Debug.Log(transform.name + " is hungry");

            if (findClosestResourceWithTag("Food") != null)
            {

                target = findClosestResourceWithTag("Food").transform.position;
                if (target != null)
                {
                    agent.SetDestination(target);
                    if (Vector3.Distance(transform.position, target) <= 2f)
                    {
                        Destroy(findClosestResourceWithTag("Food"));
                        findClosestResourceWithTag("Food").GetComponent<Resource>().isOccupied = true;
                        health = 100;
                        currentState = State.Wander;
                    }
                }
            }
            else
            {
                currentState = State.Wander;
            }
        }

        if (currentState == State.goToWater)
        {
            Debug.Log(transform.name + " is thirsty");

            if (findClosestResourceWithTag("Water") != null)
            {

                target = findClosestResourceWithTag("Water").transform.position;
                if (target != null)
                {
                    agent.SetDestination(target);
                    if (Vector3.Distance(transform.position, target) <= 2f)
                    {
                        Destroy(findClosestResourceWithTag("Water"));
                        findClosestResourceWithTag("Water").GetComponent<Resource>().isOccupied = true;
                        water = 100;
                        currentState = State.Wander;
                    }
                }
            }
            else
            {
                currentState = State.Wander;
            }
        }

        if (agent.remainingDistance >= 3f)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            agent.speed = Mathf.Lerp(agent.speed, runSpeed, 2 * Time.deltaTime);
        }
        else if (agent.remainingDistance <= 0.2f)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if (agent.remainingDistance < 3f)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = Mathf.Lerp(agent.speed, walkSpeed, 2 * Time.deltaTime);
        }
    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    private GameObject findClosestResourceWithTag(string tagtoCheck)
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tagtoCheck);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (!go.GetComponent<Resource>().isOccupied)
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

    public void OnMouseDown()
    {
        cameraController.instance.followTransform = transform;
    }
}


