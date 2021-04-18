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
    [SerializeField] public float losTimer;
    [SerializeField] public float breadCrumbTimer = 3.0f;

    [SerializeField] List<Vector3> breadCrumbs;

    private VoidToggledEnemy voidToggledEnemy;

    //determines if there is a chaseCoroutine running
    private bool chaseRoutine;
    //private Renderer object;

    // Start is called before the first frame update
    void Start()
    {
        
        voidToggledEnemy = GetComponent<VoidToggledEnemy>();
        chaseRoutine = false;
        destPoint = 0;
        fov = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        losTimer = 0.0f;
        breadCrumbs = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        isActive = voidToggledEnemy.IsRevealed;
        
        //will only chase player if they have been seen and if it has been revealed
        if (isChasing && isActive)
        {
            if (chaseRoutine == false) {
                //saw player, currently chasing
                StartCoroutine(chasePlayer());
                chaseRoutine = true;
            }
            //if enemy breaks line of sight, do broke line of sight chase
        }
        else
        {
            //check whats currently in the enemy's line of sight
            fov.FindVisibleTargets();

            //if the fov's visible targets is more than 1, player has been sighted.
            //change mode to chase
            if (fov.VisibleTargets.Count != 0) {

                //change path to player to begin the chase, 
                if (isActive)
                {
                    agent.destination = player.transform.position;
                    isChasing = true;
                }
            }

            //player is nowhere to be seen, go looking for player, unless player has just been seen
            if (!agent.pathPending && agent.remainingDistance < 1.0f && !isChasing)
            {
                GotoNextPoint();
            }
        }
    }

    IEnumerator chasePlayer() {
        //while the player is being chased
        while (isChasing) {
            fov.FindVisibleTargets();
            //while player is in line of sight chase
            if (fov.VisibleTargets.Count != 0) {
                agent.SetDestination(player.transform.position);
                breadCrumbs.Clear();
                losTimer = 0.0f;
            }
            //while player has broken line of sight, put breadcrumbs, and after a few seconds go to the closest one
            else  {
                //dont put a breadcrumb right when LOS is broken
                //start timer, every second add a breadcrumb while LOS is broken
                yield return new WaitForSecondsRealtime(1.0f);
                
                Debug.Log("Broke Line of Sight");
                ++losTimer;

                //if its within the timer, add breadcrumbs
                if (losTimer < breadCrumbTimer) {
                    agent.SetDestination(player.transform.position);
                    breadCrumbs.Add(player.transform.position);
                    
                    //check if the first breadcrumb is too close to the ai and remove them
                    if (Vector3.Distance(transform.position, breadCrumbs[0]) < 10.0f) {
                        breadCrumbs.RemoveAt(0);
                    }
                }

                //timer has ended, go to next breadcrumb
                if (losTimer >= breadCrumbTimer) {
                    
                    agent.SetDestination(breadCrumbs[0]);

                    if (agent.remainingDistance < 0.5f) {
                        
                        Reset();

                        //went to last bread crumb, but didn't see player so go back to patrolling
                        yield break;
                    }
                }
            }
            
            //if the enemy at any point becomes inactive during the chase, go back to patrolling
            if (!isActive) {
               
                Reset();

                yield break;
            }

            //pauses the coroutine and goes back to after a frame;
            yield return null;
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

    public void checkBreadCrumbWithinDist() { 
    }


    public void Reset()
    {
        losTimer = 0.0f;
        isChasing = false;
        agent.isStopped = true;
        agent.ResetPath();
        breadCrumbs.Clear();
        agent.SetDestination(patrollPoints[destPoint].position);
        chaseRoutine = false;
    }

}
