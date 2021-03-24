using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    //player reference for position
    [SerializeField] public GameObject player;

    //boolean that determines whether the enemy is currently revealed
    [SerializeField] protected bool isActive;

    //boolean that determines if the player is chasing or not
    [SerializeField] protected bool isChasing;

    //reference to navmesh agent which handles pathfinding as well as movement
    [SerializeField] protected NavMeshAgent agent;

    //set of points in the play area where the enemy will be patrolling
    [SerializeField] List<Transform> patrollPoints;
    [SerializeField] int destPoint;

    //FOV for line of sight detection of the player
    [SerializeField] protected FieldOfView fov;

    //private Renderer object;

    // Start is called before the first frame update
    void Start()
    {
        destPoint = 0;
        //the enemy begins unrevealed
        isActive = false;
        //the enemy begins in the patrolling state
        isChasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //will only chase player if they have been seen and if it has been revealed
        if (isChasing && isActive)
        {
            //saw player, currently chasing

            //if enemy breaks line of sight, do broke line of sight chase
            if (fov.VisibleTargets.Count == 0) {
                //the last seen position of the player
                Vector3 lastSeenPosition = player.transform.position;
                agent.SetDestination(lastSeenPosition);
            }
        }
        else
        {
            //check whats currently in the enemy's line of sight
            fov.FindVisibleTargets();

            //if the fov's visible targets is more than 1, player has been sighted.
            //change mode to chase
            if (fov.VisibleTargets.Count != 0) {

                //change path to player to begin the chase
                agent.destination = player.transform.position;
                isChasing = true;

            }
            //player is nowhere to be seen, go looking for player, unless player has just been seen
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !isChasing)
            {
                GotoNextPoint();
            }
        }
    }

    //command for going to the next point in a linear matter// will be randomized later on
    private void GotoNextPoint()
    {
        if (patrollPoints.Count == 0)
        {
            return;
        }

        agent.destination = patrollPoints[destPoint].position;
        destPoint = (destPoint + 1) % patrollPoints.Count;
    }

}
