using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_LockedDrawer : MonoBehaviour
{
    [Header("Prefab Components")]
    [SerializeField] intr_Lock lockMechanism;
    [SerializeField] Transform hiddenObjects;
    [SerializeField] Transform objectsToDeactivate;

    [Header("Render Object Slots")]
    [SerializeField] GameObject door;
    [SerializeField] Vector3 doorOpenLocation;
    [SerializeField] Vector3 doorOpenRotation;

    protected bool closed = true;

    // Start is called before the first frame update
    void Start()
    {
        // Hide all object(s) within.
        foreach(Transform child in hiddenObjects) {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Once the outer lock mechanism is unlocked, change door position and reveal items.
        if (closed && !lockMechanism.IsLocked) {
            Debug.Log("We opened the safe!");

            // 1. Open up!
            closed = false;
            if (door != null) {
                door.transform.localPosition = doorOpenLocation;
                door.transform.localRotation = Quaternion.Euler(doorOpenRotation);
            }

            // 2. Reveal All Items!
            foreach(Transform child in hiddenObjects) {
                child.gameObject.SetActive(true);
            }

            if (objectsToDeactivate != null) {
                foreach(Transform child in objectsToDeactivate) {
                    child.gameObject.SetActive(false);
                }
            }

        }
    }
}
