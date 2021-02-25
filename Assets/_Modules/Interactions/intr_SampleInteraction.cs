using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_SampleInteraction : InteractableObject {

    protected bool colorSwapped = false;

    void FixedUpdate() {
        UpdateInteraction();
    }

    public override void OnInteraction(InteractionManager interactor) {

        if (active) {
            base.OnInteraction(interactor);
        
            Debug.Log(interactor.name + " is interacting with " + this.name);

            if (!colorSwapped) {
                GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            } else {
                GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }

            colorSwapped = !colorSwapped;
        }
        
    }
    
}
