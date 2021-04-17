using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_RetrainOrder : InteractableObject
{
    //temporary access to main camer, might delegate to statemanager for an inspecting state
    [SerializeField]
    public GameObject main_Cam;
    protected bool colorSwapped = false;
    protected bool checkedRO = false;

    void FixedUpdate()
    {
        UpdateInteraction();
    }

    public override void OnInteraction(InteractionManager interactor)
    {
        if (active)
        {
            Debug.Log("Inspect");
            base.OnInteraction(interactor);
            interactor.currentInteraction = this;
            if (!checkedRO)
            {
                EventSystem.current.RestrainOrderCheck();
                checkedRO = true;
            }
        }
    }
}
