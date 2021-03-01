using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : InteractableObject
{
    [SerializeField] GameObject item_holder;
    protected bool picked_up = false;
    void FixedUpdate()
    {
        UpdateInteraction();
        if (picked_up) {
            transform.position = item_holder.transform.position;
        }
    }

    public override void OnInteraction(InteractionManager interactor)
    {

        if (active)
        {
            base.OnInteraction(interactor);
            if (!picked_up)
            {
                //transform.SetParent(item_holder.transform);
                transform.position = item_holder.transform.position;
                interactor.currentInteraction = this;
                interactor.interacting = true;
                promptCanvas.enabled = false;
            }
            else {
                interactor.currentInteraction = null;
                interactor.interacting = false;
                promptCanvas.enabled = true;
            }
            picked_up = !picked_up;
        }

    }
}
