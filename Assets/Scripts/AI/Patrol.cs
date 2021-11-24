using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public GameObject[] waypoints;
    public NavMeshAgent agent;
    public bool active = false;
    int patrolWP = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolWP = (patrolWP + 1) % waypoints.Length;
            agent.SetDestination(waypoints[patrolWP].transform.position);
        }
    }
    public void ResetPatrol()
    {
        patrolWP = 0;
        transform.position = new Vector3(-3, 0, 27);
        agent.isStopped = true;
        agent.ResetPath();
    }
}
