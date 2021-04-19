using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_SetActiveTrue : MonoBehaviour
{
    [SerializeField] protected List<GameObject> targetObjects;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            foreach(GameObject obj in targetObjects) {
                obj.SetActive(true);
            }
        }   
    }
}
