using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "States/Player/InitiateInspect")]
public class playerState_InitiateInspect : Substate
{
    public override void OnStateEnter(StateFrame frame)
    {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;
        Debug.Log("in state initiateinspect");

        //spawn object in inspect container
        playerFrame.inspectController.InitiateInspect(playerFrame.interactionManager.currentInteraction.gameObject);
        //remove interacted object with
        playerFrame.interactionManager.currentInteraction = null;
    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame)
    {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        string input = playerFrame.inputBuffer.PeekQueuedInput();

        if (input.Equals(InputName.Exit) && playerFrame.inspectController.IsInspecting) {
            playerFrame.ReturnToIdle();
        }

    }
    public override void OnStateExit(StateFrame frame)
    {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.inspectController.ExitInspect();
    }
}
