using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_Door : InteractableObject
{
    [Header("Tunable Door Params")]
    [SerializeField] protected GameObject doorObject;

    [SerializeField] protected Vector3 openRotation;

    protected bool open = false;
    [SerializeField] protected bool locked = false;
    void FixedUpdate() {
        UpdateInteraction();
    }
    private void Start()
    {
        EventSystem.current.onLockOpen += LockOpen;
    }

    public override void OnInteraction(InteractionManager interactor) {

        if (active) {
            base.OnInteraction(interactor);
        
            Debug.Log(interactor.name + " is interacting with " + this.name);
            if (!locked) {
                if (!open) {
                    // GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                    UpdateInteractionName("Close Door");
                    doorObject.transform.rotation = Quaternion.Euler(openRotation);

                } else {
                    // GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

                    UpdateInteractionName("Open Door");
                    doorObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                }

                open = !open;
            }
        }
        
    }

    public void LockOpen() {
        locked = false;
    }

}
