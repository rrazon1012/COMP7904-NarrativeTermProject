using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(FieldOfView))]
[RequireComponent(typeof(MeshCollider))]
public abstract class InteractableObject : MonoBehaviour {
    public static float INTERACTION_RANGE = 4f;
    public static float INTERACTION_ANGLE = 90f;
    protected FieldOfView fov;
    protected Canvas promptCanvas;
    protected TextMeshProUGUI interactionText;
    protected Image interactionPrompt;

    [Header("Configurable Fields")]
    [SerializeField] protected bool directional = true;
    public bool Directional { get { return directional; } }

    [SerializeField] protected string interactionName = "Pick Up";
    // [SerializeField] protected AudioCollection interactionAudio = null;
    // [SerializeField] protected AudioCollection interactionFailedAudio = null;
    [SerializeField] protected bool active = false;
    
    // Start is called before the first frame update
    void Awake() {

        // this.gameObject.layer = LayerMask.NameToLayer("Interactable");

        fov = this.GetComponent<FieldOfView>();
        fov.ViewAngle = 360;

        interactionText = this.GetComponentInChildren<TextMeshProUGUI>();
        interactionText.text = interactionName;

        interactionPrompt = this.GetComponentInChildren<Image>();

        promptCanvas = this.GetComponentInChildren<Canvas>();
        promptCanvas.gameObject.SetActive(false);
    }

    // If an actor enters the interaction range, add this interactable to their interaction list.
    public void UpdateInteraction() {
        fov.FindVisibleTargets();

        if ((fov.VisibleTargets.Count > 0)) {
            // active = true;
            
            int targets = 0;
            foreach (Transform target in fov.VisibleTargets) {
                if (directional && fov.SourceWithinAngle(target.transform.position, this.transform.position, target.transform.forward, INTERACTION_ANGLE)) {
                    targets++;
                } else if (!directional) {
                    targets++;
                }
            }

            if (targets > 0) {
                active = true;
                promptCanvas.gameObject.SetActive(true);
            } else {
                active = false;
                promptCanvas.gameObject.SetActive(false);
            }
            
        } else if (!(fov.VisibleTargets.Count > 0)) {

            promptCanvas.gameObject.SetActive(false);
            active = false;
        }

    }

    public virtual void OnInteraction(InteractionManager interactor) {

        // Check cooldown.

        // Debug.Log(interactor.name + " is interacting with " + this.name);
        PlayInteractionAudio();

        // onCooldown = true;
        // StartCoroutine(Cooldown());

        // StopCoroutine(PromptColorPulse());
        // StartCoroutine(PromptColorPulse());

    }

    protected IEnumerator PromptColorPulse() {

        yield return null;
    }

    protected void PlayInteractionAudio() {
        // if (interactionAudio != null) {
        //     AudioDirector.Instance.PlayRandomAudioAtPoint(interactionAudio, this.transform.position);
        // }
    }

    protected void PlayInteractionFailedAudio() {
        // if (interactionFailedAudio != null) {
        //     AudioDirector.Instance.PlayRandomAudioAtPoint(interactionFailedAudio, this.transform.position);
        // }
    }
    
}
