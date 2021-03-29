using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class event_CatchPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && GameManager.GM.enemy.GetComponent<VoidToggledEnemy>().IsRevealed)
        {

            Debug.Log("Firing event");

            EventSystem.current.PlayerCaughtTrigger();
        }
    }
}
