using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Pickup")]
public class playerState_Pickup : Substate
{

    public override void OnStateEnter(StateFrame frame)
    {

    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame)
    {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // We peek the latest queued input from the player's input buffer.
        string latestInput = playerFrame.inputBuffer.PeekQueuedInput();

        if (latestInput.Equals(InputName.Interact) && playerFrame.interactionManager.currentInteraction is PickupableObject) {
            playerFrame.ReturnToIdle();
            playerFrame.interactionManager.currentInteraction = null;
        }
    }

    public override void OnStateExit(StateFrame frame)
    {

    }

}
