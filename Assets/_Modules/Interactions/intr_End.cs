using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_End : InteractableObject
{
    void FixedUpdate()
    {
        UpdateInteraction();
    }

    public override void OnInteraction(InteractionManager interactor)
    {

        if (active)
        {
            base.OnInteraction(interactor);
        }

    }
}
