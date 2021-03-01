using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entering Item Trigger");

        if (other.tag == "ItemTrigger") {

            Debug.Log("Firing event");

            EventSystem.current.KeyEnterTrigger();
        }
    }
}
