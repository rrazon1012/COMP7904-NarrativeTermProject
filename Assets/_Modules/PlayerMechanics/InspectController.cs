using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectController : MonoBehaviour
{
    const int INSPECTABLE_LAYER = 13;
    const float inspectTurnSpeed = 0.15f;
    private PlayerInput inputcomp;
    private InputBuffer inputBuffer;

    [SerializeField] public GameObject InspectorContainer;
    [SerializeField] public GameObject InspectorLight;
    [SerializeField] private PlayerInputActions controls;
    [SerializeField] protected Camera mainCam;
    [SerializeField] protected Camera inspectCam;
    [SerializeField] protected RenderTexture mainCamTex;
    private Quaternion initialRotation;

    private GameObject insItem = null;
    private GameObject itemRef = null;
    [SerializeField] protected bool isInspecting = false;


    public bool IsInspecting { get { return isInspecting; } }

    // States
    [Header("State Slots")]

    [SerializeField] protected Substate initiateInspectState;
    public Substate InitiateInspectState
    {
        get { return Instantiate(initiateInspectState); }
    }

    [SerializeField] protected Substate exitInspectState;
    public Substate ExitInspectState
    {
        get { return Instantiate(exitInspectState); }
    }

    private void Awake()
    {
        inputcomp = this.GetComponentInParent<PlayerInput>();
        inputBuffer = this.GetComponentInParent<InputBuffer>();
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = InspectorContainer.transform.localRotation;
    }

    private void Update()
    {
        Vector2 axis = inputBuffer.GetInspectAxis();

        InspectorContainer.transform.Rotate(-axis.y * inspectTurnSpeed, 0.0f, 0.0f,Space.Self);
        InspectorContainer.transform.Rotate(0.0f, -axis.x * inspectTurnSpeed, 0.0f, Space.World);
    }

    public void InitiateInspect(GameObject item) {
        
        //set the isInspecting to true
        isInspecting = true;

        //enable the canvas to overlay on top of the main camera
        inspectCam.gameObject.SetActive(true);
        // mainCam.gameObject.SetActive(false);
        InspectorContainer.SetActive(true);
        InspectorLight.SetActive(true);
        
        //disable player controls to avoid player from moving and enable inspect controls to allow the player to rotate object around
        inputcomp.actions.FindActionMap("PlayerControls").Disable();
        inputcomp.actions.FindActionMap("InspectControls").Enable();

        inputcomp.actions.FindAction("MouseAxisX").Disable();
        inputcomp.actions.FindAction("MouseAxisY").Disable();
        
        inputcomp.actions.FindAction("InspectAxisX").Enable();
        inputcomp.actions.FindAction("InspectAxisY").Enable();
        //hides gameobject
        //instantiate gameobject and place it inside Inspector container, change its layer to inspectable layer
        insItem = GameObject.Instantiate(item);

        //zero out the transforms of the object
        insItem.transform.SetParent(InspectorContainer.transform);
        insItem.transform.localPosition = Vector3.zero;
        insItem.transform.localRotation = Quaternion.identity;

        insItem.layer = INSPECTABLE_LAYER;
        foreach (Transform child in insItem.transform) {
            if (child.name != "InteractionCanvas") {
                child.gameObject.layer = INSPECTABLE_LAYER;

                foreach(Transform childOfChild in child.transform) {
                    if (childOfChild.name != "InteractionCanvas") {
                        childOfChild.gameObject.layer = INSPECTABLE_LAYER;
                    }
                }
            }
        }
        item.SetActive(false);
        itemRef = item;
    }

    public void ExitInspect() {
        //set inspecting to false
        isInspecting = false;

        //reset the rotation of the inspector container
        InspectorContainer.transform.localRotation = initialRotation;

        //disables canvas overlay
        inspectCam.gameObject.SetActive(false);
        // mainCam.gameObject.SetActive(true);
        InspectorContainer.SetActive(false);
        InspectorLight.SetActive(false);
        //removes render texture
        //mainCam.targetTexture = null;
        //goes back to the default action map
        inputcomp.actions.FindActionMap("PlayerControls").Enable();
        inputcomp.actions.FindActionMap("InspectControls").Disable();
        inputcomp.actions.FindAction("MouseAxisX").Enable();
        inputcomp.actions.FindAction("MouseAxisY").Enable();
        inputcomp.actions.FindAction("InspectAxisX").Disable();
        inputcomp.actions.FindAction("InspectAxisY").Disable();
        //reset inspector container rotation
        //gets rid of the object in inspector container
        Destroy(insItem);
        itemRef.SetActive(true);
        itemRef = null;
        insItem = null;
    }
    
}
