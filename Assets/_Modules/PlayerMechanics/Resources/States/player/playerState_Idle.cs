using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class playerState_Idle : Substate {

    public override void OnStateEnter(StateFrame frame) {

    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame) {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // We peek the latest queued input from the player's input buffer.
        string latestInput = playerFrame.inputBuffer.PeekQueuedInput();


        // If the player presses Action 1, reveal the void.
        if (latestInput.Equals(InputName.Action1) && !playerFrame.voidController.VoidActive && playerFrame.interactionManager.currentInteraction == null)
        {

            // 1. Manage the input.
            Debug.Log("Action 1 Performed. ");
            playerFrame.inputBuffer.PopQueuedInput();

            // 2. Manage the state transition.
            frame.StateTransition(playerFrame.voidController.AimVoidState);
        }

        // If the player presses Action 2, repress the void.
        else if (latestInput.Equals(InputName.Action2) && playerFrame.voidController.VoidActive && playerFrame.interactionManager.currentInteraction == null)
        {
            Debug.Log("Action 2 Performed. ");

            // 1. Manage the input.
            playerFrame.inputBuffer.PopQueuedInput();

            // 2. Manage the state transition.
            frame.StateTransition(playerFrame.voidController.FocusRepressState);
        }
        else if (playerFrame.interactionManager.currentInteraction is InspectableObject && !playerFrame.inspectController.IsInspecting || playerFrame.interactionManager.currentInteraction is intr_Book && !playerFrame.inspectController.IsInspecting || playerFrame.interactionManager.currentInteraction is intr_RetrainOrder && !playerFrame.inspectController.IsInspecting)
        {
            if (playerFrame.interactionManager.currentInteraction is intr_Book)
            {
                intr_Book interaction = playerFrame.interactionManager.currentInteraction as intr_Book;
                if (interaction.isPulled)
                {
                    // 1. Manage the input.
                    playerFrame.inputBuffer.PopQueuedInput();

                    // 2. Manage the state transition.
                    frame.StateTransition(playerFrame.inspectController.InitiateInspectState);
                }
            }
            else
            {
                // 1. Manage the input.
                playerFrame.inputBuffer.PopQueuedInput();

                // 2. Manage the state transition.
                frame.StateTransition(playerFrame.inspectController.InitiateInspectState);
            }
        }
        else if (playerFrame.interactionManager.currentInteraction is intr_Lock && !playerFrame.lockController.IsLock) {

            // 1. Manage the input.
            playerFrame.inputBuffer.PopQueuedInput();

            // 2. Manage the state transition.
            frame.StateTransition(playerFrame.lockController.LockState);
        }

        // If the player presses Action 4, sprint.
        if (playerFrame.inputBuffer.ActionHeld(InputName.Action4) && playerFrame.interactionManager.currentInteraction == null)
        {
            Debug.Log("Action 4 Performed. ");
            if (!playerFrame.motor.noStamina)
            {
                playerFrame.motor.isSprinting = true;
            }
            else
            {
                playerFrame.motor.isSprinting = false;
            }
        }
        else
        {
            playerFrame.motor.isSprinting = false;
        }
    }
    
    public override void OnStateExit(StateFrame frame) {
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;
        playerFrame.motor.isSprinting = false;
    }

}
