using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimalController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public Animator animator;
    public float runSpeed;
    public float walkSpeed;

    //Wander
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (agent.remainingDistance >= 3f)
        {

            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            agent.speed = Mathf.Lerp(agent.speed, runSpeed, 3 * Time.deltaTime);
        }
        else if (agent.remainingDistance <= 0.1f)
        {
            Debug.Log("stop");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

        }
        else if (agent.remainingDistance < 3f)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = Mathf.Lerp(agent.speed, walkSpeed, 3 * Time.deltaTime);
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
}


