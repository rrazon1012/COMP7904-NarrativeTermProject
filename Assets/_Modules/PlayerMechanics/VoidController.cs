using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] protected VoidSphere voidSphere;

    [Header("Tunable Parameters")]
    [SerializeField] protected float castTime = 3f;
    public float CastTime { get { return castTime; } }

    protected bool voidActive = false;
    public bool VoidActive { get { return voidActive; } }

    // States
    [Header("State Slots")]

    [SerializeField] protected Substate castingState;
    public Substate CastingState {
        get {
            Substate newState = Instantiate(castingState);
            newState.TimeToLive = castTime;
            return newState;
        }
    }

    // This coroutine slot is shared by all functions in the class.
    //      This way, any adjustments may be interrupted without fear of overlapping adjustments.
    protected IEnumerator voidAdjustmentCoroutine;

    public void Reveal() {
        if (voidActive) {
            Debug.Log("Only one void may be active at a time.");
            return;
        }

        voidActive = true;

        // If the void is currently changing in size, end that process.
        if (voidAdjustmentCoroutine != null) {
            StopCoroutine(voidAdjustmentCoroutine);
        }

        // Assign the new coroutine to the shared slot.
        voidAdjustmentCoroutine = AdjustVoidSize(11f, 10);

        StartCoroutine(voidAdjustmentCoroutine);
    }

    public void Repress() {
        if (!voidActive) {
            Debug.Log("The void is already closed.");
            return;
        }

        voidActive = false;

        if (voidAdjustmentCoroutine != null) {
            StopCoroutine(voidAdjustmentCoroutine);
        }

        voidAdjustmentCoroutine = AdjustVoidSize(0f, 10);

        StartCoroutine(voidAdjustmentCoroutine);
    }

    protected IEnumerator AdjustVoidSize(float targetRadius, float adjustionSpeed) {
        // Case that the sphere must expand
        if (targetRadius > voidSphere.voidRadius) {

            while (voidSphere.voidRadius <= targetRadius) {
                // The positive adjustment is clamped to never exceed that target radius
                voidSphere.voidRadius = Mathf.Clamp(voidSphere.voidRadius + adjustionSpeed * Time.fixedDeltaTime, 0, targetRadius);
                yield return new WaitForFixedUpdate();
            }

            voidSphere.voidRadius = targetRadius;
        }

        // Case that the sphere must retract
        else if (targetRadius < voidSphere.voidRadius) {
            
            while (voidSphere.voidRadius >= targetRadius) {
                // The negative adjustment is clamped to never exceed 0
                voidSphere.voidRadius = Mathf.Clamp(voidSphere.voidRadius - adjustionSpeed * Time.fixedDeltaTime, 0, Mathf.Infinity);
                yield return new WaitForFixedUpdate();
            }

            voidSphere.voidRadius = targetRadius;
        }
    }
}
