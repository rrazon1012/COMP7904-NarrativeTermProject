using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This component defines an object that will be toggled by the Void Sphere
public class VoidToggledObject : MonoBehaviour
{

    public enum LightOrDark {
        Light,
        Dark
    }

    public LightOrDark revealedInWorld;

    [SerializeField] protected bool isRevealed = true;
    [SerializeField] protected List<GameObject> objectList;

    protected Collider collider;

    // Start is called before the first frame update
    void Start() {

        collider = this.GetComponent<Collider>();
        objectList = new List<GameObject>();

        foreach (Transform child in this.transform) {
            objectList.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (revealedInWorld == LightOrDark.Light) {

            // A Light-Object is revealed the majority of the time, so we check the exception: the case where we should hide it.
            if (VoidSphere.Instance.Active && VoidSphere.Instance.ObjectFullyWithinCollision(collider, (collider as SphereCollider).radius)) {
                
                // Only iterate and hide each object if they have not already been hidden.
                if (isRevealed) {
                    isRevealed = false;
                    foreach (GameObject obj in objectList) {
                        obj.SetActive(false);
                    }
                }
                

            } else {
                
                // Only iterate and reveal each object if they have not already been revealed.
                if (!isRevealed) {
                    isRevealed = true;
                    foreach (GameObject obj in objectList) {
                        obj.SetActive(true);
                    }
                }
                
            }

        } else if (revealedInWorld == LightOrDark.Dark) {

            // A Dark-Object is hidden the majority of the time, so we check the exception: the case where the void is active and it may be revealed.
            if (VoidSphere.Instance.Active && VoidSphere.Instance.ObjectJustOnEdgeCollision(collider)) {
                
                
                if (!isRevealed) {
                    isRevealed = true;
                    foreach (GameObject obj in objectList) {
                        obj.SetActive(true);
                    }
                }

            } else {

                if (isRevealed) {
                    isRevealed = false;
                    foreach (GameObject obj in objectList) {
                        obj.SetActive(false);
                    }
                }
            }
        }
        
    }
}
