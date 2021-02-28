using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_Door : InteractableObject
{
    [Header("Tunable Door Params")]
    [SerializeField] protected GameObject doorObject;

    [SerializeField] protected Vector3 openRotation;

    protected bool open = false;

    void FixedUpdate() {
        UpdateInteraction();
    }

    public override void OnInteraction(InteractionManager interactor) {

        if (active) {
            base.OnInteraction(interactor);
        
            Debug.Log(interactor.name + " is interacting with " + this.name);

            if (!open) {
                // GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                UpdateInteractionName("Close Door");
                doorObject.transform.rotation = Quaternion.Euler(openRotation);

            } else {
                // GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

                UpdateInteractionName("Open Door");
                doorObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            PlayInteractionAudio();
            open = !open;
        }
        
    }

    protected override void PlayInteractionAudio() {
        if (interactionAudio != null && interactionAudio.Length > 0) {
            AudioDirector.Instance.PlayRandomAudioAtPoint(interactionAudio[0], this.transform.position);
        }
    }

}
