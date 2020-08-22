using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalController : MonoBehaviour
{
    public Camera cam;
    public Material[] skins;
    public Renderer meshRenderer;
    public enum State
    {
        Wander,
        Hungry,
        Eating,
        Thirsty,
        Drinking,
        Sleeping
    }
    public State currentState;

    public NavMeshAgent agent;
    Vector3 target;
    public Animator animator;
    public float runSpeed;
    public float walkSpeed;

    //Wander
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    void Start()
    {
        meshRenderer.material = skins[Random.Range(0, skins.Length)];
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        if (currentState == State.Hungry)
        {
            if (GameObject.FindGameObjectsWithTag("Food").Length > 0)
            {

                target = findClosestObjectWithTag("Food").transform.position;
                agent.SetDestination(target);
                if (Vector3.Distance(transform.position, target) <= 2f)
                {
                    currentState = State.Eating;
                }
            }
        }
        if (currentState == State.Thirsty)
        {
            if (GameObject.FindGameObjectsWithTag("Water").Length > 0)
            {

                target = findClosestObjectWithTag("Water").transform.position;
                agent.SetDestination(target);
                if (Vector3.Distance(transform.position, target) <= 2f)
                {
                    currentState = State.Drinking;
                }
            }
        }
        if (currentState == State.Sleeping)
        {
            animator.SetBool("isSleeping", true);
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
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    private GameObject findClosestObjectWithTag(string tagtoCheck)
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tagtoCheck);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public void OnMouseDown()
    {
        cameraController.instance.followTransform = transform;
    }
}


