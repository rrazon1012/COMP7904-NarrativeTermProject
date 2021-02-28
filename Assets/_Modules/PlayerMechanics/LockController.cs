using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LockController : MonoBehaviour
{
    private PlayerInput inputcomp;
    private InputBuffer inputBuffer;

    [SerializeField] protected bool isLock = false;
    [SerializeField] protected bool isLocked = true;
    [SerializeField] protected intr_Lock currentLock;

    public bool IsLock { get { return isLock; } }

    [Header("State Slots")]

    [SerializeField] protected Substate lockState;
    public Substate LockState
    {
        get { return Instantiate(lockState); }
    }

    private void Awake()
    {
        inputcomp = this.GetComponentInParent<PlayerInput>();
        inputBuffer = this.GetComponentInParent<InputBuffer>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void interactLock(intr_Lock item) {
        
        isLock = true;

        currentLock = item;
        currentLock.showCanvas();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        inputcomp.actions.FindActionMap("PlayerControls").Disable();
        inputcomp.actions.FindActionMap("LockControls").Enable();
        inputcomp.actions.FindActionMap("InspectControls").Disable();

        inputcomp.actions.FindAction("MouseAxisX").Disable();
        inputcomp.actions.FindAction("MouseAxisY").Disable();
        inputcomp.actions.FindAction("Interact").Disable();
        inputcomp.actions.FindAction("Exit").Disable();
    }

    public bool checkPassCode() {
        return currentLock.checkCombination();
    }

    public void exitLock() {

        isLock = false;

        currentLock.hideCanvas();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputcomp.actions.FindActionMap("PlayerControls").Enable();
        inputcomp.actions.FindActionMap("LockControls").Disable();
        inputcomp.actions.FindActionMap("InspectControls").Disable();

        inputcomp.actions.FindAction("MouseAxisX").Enable();
        inputcomp.actions.FindAction("MouseAxisY").Enable();
        inputcomp.actions.FindAction("Interact").Enable();
        inputcomp.actions.FindAction("Exit").Enable();


    }
}
