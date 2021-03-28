using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_Footsteps : MonoBehaviour
{
    protected PlayerMotor motor;
    [SerializeField] protected AudioCollection sfx_footsteps;
    [SerializeField] protected float footstepDelay = 1.5f;
    protected float timeToNextFootstep;
    protected IEnumerator footstepCoroutine;

    // Start is called before the first frame update
    void Start() {
        motor = this.GetComponent<PlayerMotor>();
        timeToNextFootstep = footstepDelay;
    }

    // Update is called once per frame
    void FixedUpdate() {
        
    }
}
