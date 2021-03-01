using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectController : MonoBehaviour
{
    const int INSPECTABLE_LAYER = 13;
    const float inspectTurnSpeed = 1.0f;
    private PlayerInput inputcomp;
    private InputBuffer inputBuffer;
    private float xRotation = 0.0f;


    [SerializeField] public GameObject InspectorContainer;
    [SerializeField] private PlayerInputActions controls;
    [SerializeField] protected Camera mainCam;
    [SerializeField] protected RenderTexture mainCamTex;
    [SerializeField] protected GameObject Inspector;

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
    }

    private void Update()
    {
        Vector2 axis = inputBuffer.GetInspectAxis();

        float lookX = axis.x * inspectTurnSpeed * Mathf.Deg2Rad;
        float lookY = axis.y * inspectTurnSpeed * Mathf.Deg2Rad;

        InspectorContainer.transform.Rotate(Vector3.up,-lookX);
        InspectorContainer.transform.Rotate(Vector3.right,lookY);

        //Vector2 scroll = inputBuffer.GetScrollZoom();
        //if scroll.y is -120 = downward , scrolly is +120 = upward
        //Debug.Log(scroll.y);
    }

    public void InitiateInspect(GameObject item) {
        
        //set the isInspecting to true
        isInspecting = true;

        //enable the canvas to overlay on top of the main camera
        Inspector.SetActive(true);

        //set the raw image on the canvas to the rendered texture to display the object and the background simulating a popup object
        mainCam.targetTexture = mainCamTex;
        
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
        insItem.transform.position = InspectorContainer.transform.position;
        insItem.transform.SetParent(InspectorContainer.transform);
        insItem.layer = INSPECTABLE_LAYER;
        foreach (Transform child in insItem.transform) {
            child.gameObject.layer = INSPECTABLE_LAYER;
        }
        item.SetActive(false);
        itemRef = item;
    }

    public void ExitInspect() {
        //set inspecting to false
        isInspecting = false;
        //disables canvas overlay
        Inspector.SetActive(false);
        //removes render texture
        mainCam.targetTexture = null;
        //goes back to the default action map
        inputcomp.actions.FindActionMap("PlayerControls").Enable();
        inputcomp.actions.FindActionMap("InspectControls").Disable();
        inputcomp.actions.FindAction("MouseAxisX").Enable();
        inputcomp.actions.FindAction("MouseAxisY").Enable();
        inputcomp.actions.FindAction("InspectAxisX").Disable();
        inputcomp.actions.FindAction("InspectAxisY").Disable();
        //reset inspector container rotation
        xRotation = 0.0f;
        //gets rid of the object in inspector container
        Destroy(insItem);
        itemRef.SetActive(true);
        itemRef = null;
        insItem = null;
    }
    
}
