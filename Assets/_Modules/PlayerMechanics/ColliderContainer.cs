using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by Telegraph Objects to track active objects within their range.
public class ColliderContainer : MonoBehaviour
{
    [SerializeField] private List<Collider> colliders = new List<Collider>();
    public List<Collider> GetAllColliders { get { return colliders; } }

    public List<Collider> GetCollidersOnLayers (LayerMask targetLayer) {
        List<Collider> curatedList = new List<Collider>();
        // Only include colliders that are within the Target Layers
        foreach (Collider c in colliders) {
            if (c != null && targetLayer.Contains(c.gameObject.layer)) {
                curatedList.Add(c);
            }
        }

        return curatedList;
    }

    private void OnTriggerEnter (Collider other) {
        if (!colliders.Contains(other) && other.GetComponent<ColliderContainer>() == null ) {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit (Collider other) {
        colliders.Remove(other);
    }
 
}
