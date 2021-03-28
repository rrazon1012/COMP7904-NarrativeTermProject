using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> points;
    private int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            GotoNextPoint();
        }
        //check fov for if player is in Line of sight

    }

    void GotoNextPoint() {
        if (points.Count == 0) {
            return;
        }

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Count;
    }
}
