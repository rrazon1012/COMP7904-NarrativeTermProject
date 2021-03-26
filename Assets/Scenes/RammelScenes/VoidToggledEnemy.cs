using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VoidToggledEnemy : MonoBehaviour
{
    public enum LightOrDark {
        Light,
        Dark
    };

    public LightOrDark revealedinWorld;

    [SerializeField] protected bool isRevealed = true;
    public bool IsRevealed { get { return isRevealed; } }

    [SerializeField] protected List<GameObject> objectList;

    protected Collider voidCollider;

    [SerializeField] protected Material revealed;
    [SerializeField] protected Material hidden;
    [SerializeField] protected VoidController voidController;
    private IEnumerator closingRoutine;
    // Start is called before the first frame update
    void Start()
    {
        voidCollider = this.GetComponent<Collider>();
        voidCollider.isTrigger = true;
        objectList = new List<GameObject>();
        objectList.Add(this.gameObject);
        foreach (Transform child in this.transform)
        {
            objectList.Add(child.gameObject);
        }
    }
    IEnumerator closing() {
        //while the void is closing
        while (voidController.isClosing == true) {
            //if the enemy isn't in inside the voidsphere collider anymore while closing, hide the enemy.
            if (!VoidSphere.Instance.ObjectJustOnEdgeCollision(voidCollider)) {
                if (isRevealed)
                {
                    isRevealed = false;
                    this.GetComponent<Renderer>().material = hidden;
                    this.GetComponent<Collider>().isTrigger = true;
                    this.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    this.GetComponent<NavMeshAgent>().ResetPath();

                    foreach (GameObject obj in objectList)
                    {
                        voidController.isClosing = false;
                        obj.GetComponent<MeshRenderer>().material = hidden;
                    }
                }
            }
            yield return null;
        }
        StopCoroutine(closingRoutine);
        closingRoutine = null;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(closingRoutine != null);
        //Debug.Log("Is Closing " + voidController.isClosing + " In Collider " + VoidSphere.Instance.ObjectFullyWithinCollision(voidCollider, (voidCollider as CapsuleCollider).radius));
        // A Dark-Object is hidden the majority of the time, so we check the exception: the case where the void is active and it may be revealed.
        if (VoidSphere.Instance.Active && VoidSphere.Instance.ObjectJustOnEdgeCollision(voidCollider))
        {
            if (!isRevealed)
            {
                isRevealed = true;
                this.GetComponent<Renderer>().material = revealed;
                this.GetComponent<Collider>().isTrigger = false;
                this.GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                foreach (GameObject obj in objectList)
                {
                    obj.GetComponent<MeshRenderer>().material = revealed;
                }
            }
        }
        if(voidController.isClosing && VoidSphere.Instance.ObjectJustOnEdgeCollision(voidCollider)){
            if (closingRoutine == null)
            {
                Debug.Log("Checking when the enemy is closed on by the sphere");
                closingRoutine = closing();
                StartCoroutine(closingRoutine);
            }
        }
    }
    
}
