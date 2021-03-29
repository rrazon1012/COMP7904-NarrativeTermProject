using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_Footsteps : MonoBehaviour
{
    protected PlayerMotor motor;
    [SerializeField] protected AudioCollection sfx_footsteps;
    [SerializeField] protected float footstepDelay = 0.6f;
    protected float timeToNextFootstep;
    protected IEnumerator footstepCoroutine;

    // Start is called before the first frame update
    void Start() {
        motor = this.GetComponent<PlayerMotor>();
        timeToNextFootstep = footstepDelay;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Count down the timer when the player is moving. Moving is set based on motor's Horizontal Velocity.
        if (motor.IsMoving) {
            timeToNextFootstep -= Time.fixedDeltaTime;

            if (timeToNextFootstep <= 0) {
                AudioDirector.Instance.PlayRandomAudioAtPoint(sfx_footsteps, this.transform.position);
                timeToNextFootstep = footstepDelay;
            }
        } else {
            // Set a short delay for the first footstep.
            timeToNextFootstep = 0.1f;
        }

        if (!motor.isSprinting) {
            footstepDelay = 0.6f;
        } else {
            footstepDelay = 0.2f;
        }
        
    }
}
