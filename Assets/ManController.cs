using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
public class ManController : MonoBehaviour
{
    public Camera camera;
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public bool busy;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

        if (!busy)
        {
            animator.SetBool("isWorking", false);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {

                agent.SetDestination(RandomNavSphere(transform.position, wanderRadius, -1));
                timer = 0;
                wanderTimer += Random.Range(-3, 3);
                wanderTimer = Mathf.Clamp(wanderTimer, 5, 15);
            }
        }
        if (busy)
        {
            animator.SetBool("isWorking", true);
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
}
