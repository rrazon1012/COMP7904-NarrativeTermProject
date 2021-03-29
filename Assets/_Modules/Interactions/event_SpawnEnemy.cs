using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_SpawnEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {

            Debug.Log("Firing event");

            EventSystem.current.SpawnEnemy();
        }
    }
}
