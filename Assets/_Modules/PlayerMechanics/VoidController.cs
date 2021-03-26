using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    [Header("Object References")]
    protected Camera mainCamera;

    [SerializeField] protected GameObject targetSphere;

    [Header("Tunable Parameters")]
    [SerializeField] protected LayerMask targetMask;

    [SerializeField] protected float castTime = 0.5f;
    [SerializeField] protected float voidSpeed = 20f;
    public float CastTime { get { return castTime; } }

    protected bool voidActive = false;
    public bool VoidActive { get { return voidActive; } }

    protected bool validTarget = false;
    public bool ValidTarget { get { return validTarget; } }

    [SerializeField] float targetFocusTime = 5f;
    protected float focusTime = 0.0f;
    public void ResetFocus() { focusTime = 0.0f; }
    public bool RepressFocused { get { return focusTime >= targetFocusTime; } }

    // States
    [Header("State Slots")]

    [SerializeField] protected Substate aimVoidState;
    public Substate AimVoidState {
        get { return Instantiate(aimVoidState); }
    }

    [SerializeField] protected Substate focusRepressState;
    public Substate FocusRepressState {
        get { return Instantiate(focusRepressState); }
    }

    [SerializeField] protected Substate castingState;
    public Substate CastingState {
        get {
            Substate newState = Instantiate(castingState);
            newState.TimeToLive = castTime;
            return newState;
        }
    }

    [Header("Audio Collections")]
    [SerializeField] protected AudioCollection sfx_OpenVoid;
    [SerializeField] protected AudioCollection sfx_CloseVoid;

    void Start() {
        
        mainCamera = Camera.main;
    }

    // This coroutine slot is shared by all functions in the class.
    //      This way, any adjustments may be interrupted without fear of overlapping adjustments.
    protected IEnumerator voidAdjustmentCoroutine;

    protected Vector3 targetLocation;

    public void AimVoid() {
        Debug.Log("Aiming Void...");

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 30f, targetMask)) {
            // Set the target to valid.
            validTarget = true;
            
            // Set the VoidSphere's position to the intersection.
            Debug.DrawRay(transform.position, mainCamera.transform.forward * hit.distance, Color.yellow);
            VoidSphere.Instance.transform.position = hit.point;
            targetSphere.transform.position = hit.point;

        } else {
            // Set the target to invalid.
            validTarget = false;

            // Set the VoidSphere's position to negative world origin.
            VoidSphere.Instance.transform.position = new Vector3(0, -100, 0);
            targetSphere.transform.position = new Vector3(0, -100, 0);
        }

    }

    public void Reveal() {
        targetSphere.transform.position = new Vector3(0, -100, 0);

        if (voidActive) {
            Debug.Log("Only one void may be active at a time.");
            return;
        }

        voidActive = true;
        AudioDirector.Instance.PlayRandomAudioAtPoint(sfx_OpenVoid, VoidSphere.Instance.transform.position);

        // If the void is currently changing in size, end that process.
        if (voidAdjustmentCoroutine != null) {
            StopCoroutine(voidAdjustmentCoroutine);
        }

        // Assign the new coroutine to the shared slot.
        voidAdjustmentCoroutine = AdjustVoidSize(11f, voidSpeed);

        StartCoroutine(voidAdjustmentCoroutine);
    }

    public void FocusRepress() {
        Debug.Log("Focusing Void...");

        focusTime += Time.fixedDeltaTime;

    }

    public void Repress() {

        if (!voidActive) {
            Debug.Log("The void is already closed.");
            return;
        }

        voidActive = false;
        AudioDirector.Instance.PlayRandomAudioAtPoint(sfx_CloseVoid, VoidSphere.Instance.transform.position);

        if (voidAdjustmentCoroutine != null) {
            StopCoroutine(voidAdjustmentCoroutine);
        }

        voidAdjustmentCoroutine = AdjustVoidSize(0f, voidSpeed);

        StartCoroutine(voidAdjustmentCoroutine);
    }

    protected IEnumerator AdjustVoidSize(float targetRadius, float adjustionSpeed) {
        // Case that the sphere must expand
        if (targetRadius > VoidSphere.Instance.voidRadius) {

            while (VoidSphere.Instance.voidRadius <= targetRadius) {
                // The positive adjustment is clamped to never exceed that target radius
                VoidSphere.Instance.voidRadius = Mathf.Clamp(VoidSphere.Instance.voidRadius + adjustionSpeed * Time.fixedDeltaTime, 0, targetRadius);
                yield return new WaitForFixedUpdate();
            }

            VoidSphere.Instance.voidRadius = targetRadius;
        }

        // Case that the sphere must retract
        else if (targetRadius < VoidSphere.Instance.voidRadius) {
            
            while (VoidSphere.Instance.voidRadius >= targetRadius) {
                // The negative adjustment is clamped to never exceed 0
                VoidSphere.Instance.voidRadius = Mathf.Clamp(VoidSphere.Instance.voidRadius - adjustionSpeed * Time.fixedDeltaTime, 0, Mathf.Infinity);
                yield return new WaitForFixedUpdate();
            }

            VoidSphere.Instance.voidRadius = targetRadius;
        }
    }
}
