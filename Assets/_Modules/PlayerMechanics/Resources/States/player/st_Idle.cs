using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class st_Idle : Substate {

    public override void OnStateEnter(StateFrame frame) {

    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame) {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // We peek the latest queued input from the player's input buffer.
        string latestInput = playerFrame.inputBuffer.PeekQueuedInput();


        // If the player presses Action 1, reveal the void.
        if (latestInput.Equals(InputName.Action1) && !playerFrame.voidController.VoidActive) {

            // 1. Manage the input.
            Debug.Log("Action 1 Performed. ");
            playerFrame.inputBuffer.PopQueuedInput();

            // 2. Call the function required.
            playerFrame.voidController.Reveal();

            // 3. Manage the state transition.
            frame.StateTransition(playerFrame.voidController.CastingState);
        }

        // If the player presses Action 2, repress the void.
        else if (latestInput.Equals(InputName.Action2) && playerFrame.voidController.VoidActive) {
            Debug.Log("Action 2 Performed. ");

            // 1. Manage the input.
            playerFrame.inputBuffer.PopQueuedInput();

            // 2. Call the function required.
            playerFrame.voidController.Repress();

            // 3. Manage the state transition.
            frame.StateTransition(playerFrame.voidController.CastingState);
        }
    }
    
    public override void OnStateExit(StateFrame frame) {

    }

}
