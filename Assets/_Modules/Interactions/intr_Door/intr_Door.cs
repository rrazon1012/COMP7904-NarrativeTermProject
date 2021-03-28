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

    public override bool ValidInteractionState { get { return active && !locked; } }

    public override void OnInteraction(InteractionManager interactor) {

        if (ValidInteractionState) {
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
                PlayInteractionAudio();
            }
        }
        
    }

    public void LockOpen()
    {
        locked = false;
    }
    protected override void PlayInteractionAudio() {
        if (interactionAudio != null && interactionAudio.Length > 0) {
            AudioDirector.Instance.PlayRandomAudioAtPoint(interactionAudio[0], this.transform.position);
        }
    }

}
