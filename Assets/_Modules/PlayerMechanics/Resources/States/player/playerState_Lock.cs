using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Lock")]
public class playerState_Lock : Substate
{

    public override void OnStateEnter(StateFrame frame)
    {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.lockController.interactLock(playerFrame.interactionManager.currentInteraction as intr_Lock);
        playerFrame.interactionManager.currentInteraction = null;
    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame)
    {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // We peek the latest queued input from the player's input buffer.
        string latestInput = playerFrame.inputBuffer.PeekQueuedInput();

        if (latestInput.Equals(InputName.EnterCombination) && playerFrame.lockController.IsLock) {
            if (playerFrame.lockController.checkPassCode()) {
                playerFrame.ReturnToIdle();
            }
        } 
        else if (latestInput.Equals(InputName.ExitLock) && playerFrame.lockController.IsLock) {
            
            playerFrame.ReturnToIdle();
        }

    }

    public override void OnStateExit(StateFrame frame)
    {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.lockController.exitLock();
    }

}
