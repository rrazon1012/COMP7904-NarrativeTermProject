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

                // If there's only one interactable, choose it.
                if (nearbyInteractables.Count >= 1) {

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
                }

                // At this point, we know there are more than one interactable, and they must be sorted by distance.
            } else {
                Debug.Log("No nearby interactions");
            }
        }
        
    }

    protected IEnumerator Cooldown() {
        onCooldown = true;

        yield return new WaitForSeconds(interactionCooldown);

        onCooldown = false;
    }

}
