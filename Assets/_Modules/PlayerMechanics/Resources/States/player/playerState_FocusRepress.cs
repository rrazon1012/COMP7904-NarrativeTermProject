using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/FocusRepress")]
public class playerState_FocusRepress : Substate
{
    public override void OnStateEnter(StateFrame frame) {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.animator.Play("EnterFocusRepress");
    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame) {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // While this state is updating, increment the focus time.
        playerFrame.voidController.FocusRepress();

        // If the player releases the Action 2 button and has focused for long enough, repress the void.
        if (!playerFrame.inputBuffer.ActionHeld(InputName.Action2)) {

            // 1. Call the function required.
            if (playerFrame.voidController.RepressFocused) {
                
                // Reveal the void
                playerFrame.voidController.Repress();

                // Play animation
                playerFrame.animator.Play("CastRepress");

                // Manage the state transition.
                frame.StateTransition(playerFrame.voidController.CastingState);

            } else {

                playerFrame.animator.Play("CancelRepress");

                // Manage the state transition.
                frame.StateTransition(playerFrame.voidController.CastingState);

            }
            
        }

    }
    
    public override void OnStateExit(StateFrame frame) {
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.voidController.ResetFocus();
    }
}
