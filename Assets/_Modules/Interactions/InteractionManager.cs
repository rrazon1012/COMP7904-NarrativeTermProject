using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] public bool interacting = false;
    [SerializeField] public InteractableObject currentInteraction;
    [SerializeField] protected LayerMask interactionLayer;

    [SerializeField] protected List<Transform> nearbyInteractables;

    [Header("Cooldown")]
    [SerializeField] protected float interactionCooldown = 0.6f;
    protected bool onCooldown = false;

    protected FieldOfView fov;

    // Start is called before the first frame update
    void Start() {
        fov = this.GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10.0f);
    }

    void OnInteract() {
        Debug.Log("Player pressed interact");
        if (!onCooldown) {

            StartCoroutine(Cooldown());

            // If engaged in an ongoing interaction, check to see if current interaction can be resolved.
            if (interacting) {

                if (currentInteraction != null) {
                    currentInteraction.OnInteraction(this);
                    // currentInteraction = null;
                    // interacting = false;
                }

                return;
            }

            // 1. Find nearby Interactable Objects
            nearbyInteractables = fov.FindNearbyTargets(InteractableObject.INTERACTION_RANGE, interactionLayer);

            // 2. Check if any interactables are near.
            if (nearbyInteractables.Count > 0) {
                
                // Remove invalid interactions from the list before resolving the list
                for (int i = nearbyInteractables.Count - 1; i >= 0 ; --i) {

                    InteractableObject interactable = nearbyInteractables[i].GetComponent<InteractableObject>();
                    if (!interactable.ValidInteractionState) {
                        nearbyInteractables.Remove(interactable.transform);
                    }
                }

                // If there's only one interactable, choose it.
                if (nearbyInteractables.Count == 1) {

                    // Loop, finding the first usable interacable in the list.
                    foreach (Transform target in nearbyInteractables) {

                        InteractableObject interactable = target.GetComponent<InteractableObject>();

                        if (interactable != null) {

                            if (interactable.Directional ) { // && fov.WithinAngle(interactable.transform.position, this.transform.forward, InteractableObject.INTERACTION_ANGLE)

                                interactable.OnInteraction(this);
                                Debug.Log("Interacting with " + interactable.name);
                                return;

                            } else if (!interactable.Directional) {

                                interactable.OnInteraction(this);
                                Debug.Log("Interacting with " + interactable.name);
                                return;
                                
                            }
                            
                        }
                    }
                    // InteractableObject interactable = nearbyInteractables[0].GetComponent<InteractableObject>();
                } else {
                    InteractableObject raycastInteractable;

                    RaycastHit HitInfo = new RaycastHit();
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out HitInfo, 15.0f))
                    {
                        raycastInteractable = HitInfo.collider.gameObject.GetComponent<InteractableObject>();
                        if (raycastInteractable != null)
                        {
                            raycastInteractable.OnInteraction(this);
                        }
                    }
                    else
                    {
                        // At this point, we know there are more than one interactable, and they must be sorted by distance.
                        nearbyInteractables.Sort(delegate (Transform a, Transform b)
                        {
                            Vector3 distToA = (this.transform.position - a.position);
                            Vector3 distToB = (this.transform.position - b.position);

                            return distToA.magnitude.CompareTo(distToB.magnitude);
                        }
                        );

                        foreach (Transform target in nearbyInteractables)
                        {

                            InteractableObject interactable = target.GetComponent<InteractableObject>();

                            if (interactable != null)
                            {

                                if (interactable.Directional)
                                { // && fov.WithinAngle(interactable.transform.position, this.transform.forward, InteractableObject.INTERACTION_ANGLE)

                                    interactable.OnInteraction(this);
                                    Debug.Log("Interacting with " + interactable.name);
                                    return;

                                }
                                else if (!interactable.Directional)
                                {

                                    interactable.OnInteraction(this);
                                    Debug.Log("Interacting with " + interactable.name);
                                    return;

                                }

                            }
                        }
                    }

                }
                
            } else {
                Debug.Log("No Nearby Interactions");
            }
        }
        
    }

    public Transform getCloserObject(Transform t1, Transform t2) {
        if (Vector3.Distance(t1.position, this.transform.position) < Vector3.Distance(t1.position, this.transform.position))
        {
            return t1;
        }
        else 
        {
            return t2;
        }
    }

    protected IEnumerator Cooldown() {
        onCooldown = true;

        yield return new WaitForSeconds(interactionCooldown);

        onCooldown = false;
    }

}
