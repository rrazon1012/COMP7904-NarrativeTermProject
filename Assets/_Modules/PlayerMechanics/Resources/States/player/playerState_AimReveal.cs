using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/AimReveal")]
public class playerState_AimReveal : Substate
{
    public override void OnStateEnter(StateFrame frame) {
        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        playerFrame.animator.Play("EnterAimReveal");
    }

    // This function will be called by StateFrame each fixed Timestep.
    public override void Listen(StateFrame frame) {

        // We perform a cast to the required StateFrame type, to reduce "GetComponent" calls at runtime.
        PlayerStateFrame playerFrame = frame as PlayerStateFrame;

        // While this state is updating, call the aim function in the VoidController.
        playerFrame.voidController.AimVoid();

        // This state will transition once the player releases the action 1 button.
        if (!playerFrame.inputBuffer.ActionHeld(InputName.Action1)) {

            // 1. Call the function required.
            if (playerFrame.voidController.ValidTarget) {
                
                // Reveal the void
                playerFrame.voidController.Reveal();

                // Play animation
                playerFrame.animator.Play("CastReveal");

                // Manage the state transition.
                frame.StateTransition(playerFrame.voidController.CastingState);

            } else {

                playerFrame.animator.Play("CancelReveal");

                // Manage the state transition.
                frame.StateTransition(playerFrame.voidController.CastingState);

            }
            
        }

    }
    
    public override void OnStateExit(StateFrame frame) {

    }
}
