using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_key : MonoBehaviour
{
    [SerializeField] protected intr_Door door;
    [SerializeField] protected GameObject key;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering Item Trigger");

        if (other.name.Equals(key.name)) {

            door.LockOpen();
            door.OpenDoor();
            EventSystem.current.KeyEnterTrigger();
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            //Debug.Log("Firing event");
        }
    }
}
