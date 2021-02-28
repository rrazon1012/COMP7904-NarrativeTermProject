using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/* Input Buffer v2
    Published by Kurtis Lawson
*/

// For consistent references of input names from outside components.
public static class InputName {
    // Weapon Actions
	public static string Action1 { get { return "Action1"; } }
	public static string Action2 { get { return "Action2"; } }
	public static string Action3 { get { return "Action3"; } }
	public static string Action4 { get { return "Action4"; } }
	public static string Exit { get { return "Exit"; } }
	public static string Interact { get { return "Interact"; } }

}

// Input Buffer is made to be a resource that is polled by other components.
//      Specifically, it is designed to handle player combat-related inputs, and attempts
//      to simplify access to specific Action Inputs, such as Movement Vectors and Attack Triggers.

// REQUIREMENTS:
[RequireComponent(typeof(PlayerInput))]

public class InputBuffer : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private PlayerInput inputComponent;

    [Header("Logistic Fields")]
    [SerializeField] private float bufferTime = 0.1f; // How long a trigger is stored and considered "active"
    [SerializeField] private bool gamepadInput = true; // Set if a gamepad is detected as the last input device
    public bool GamepadInputActive() { return gamepadInput; }

    // Subscribed Vectors
    [Header("Vectors")]
    [SerializeField] private Vector2 movementVector;
    public Vector2 GetMovementVector() { return movementVector; }

	[SerializeField] private Vector2 rotationVector;
    public Vector2 GetRotationVector() { return rotationVector; }

	[SerializeField] private Vector2 mousePosition;
    public Vector2 GetMousePosition() { return mousePosition; }

    [SerializeField] private Vector2 mouseAxis;
    public Vector2 GetMouseAxis() { return mouseAxis; }
    
    [SerializeField] private Vector2 inspectAxis;
    public Vector2 GetInspectAxis() { return inspectAxis; }

    // Both Combat Action Triggers and Holds are stored in a Lookup Table.
    //      Access to the values in the table are controlled within the InputBuffer class.
    //      Actions are NOT added to the table until their first invocation.
    //      ONLY explicitly subscribed actions are added to the table.
    private Dictionary<string, bool> triggeredActions;
    private Dictionary<string, IEnumerator> bufferCoroutines;
    private Dictionary<string, bool> heldActions;


    private Queue<string> inputQueueActions;
    private Queue<float> inputQueueExpirations;

    private void PushInput(string actionName) {
        inputQueueActions.Enqueue(actionName);
        inputQueueExpirations.Enqueue(Time.time + bufferTime);
    }

    private string PopInput() {
        if (inputQueueActions.Count > 0 && inputQueueExpirations.Count > 0) {

            // Remove the expiration, and return the top input.
            inputQueueExpirations.Dequeue();
            return inputQueueActions.Dequeue();

        }
        
        return "";
    }

    // Discards all expired inputs, and returns the first valid input.
    public string PopQueuedInput() {
        // bool validInputFound = false;
        string topInput = PopInput();

        // // This loop will continue until either a valid input is found or the queues are completely emptied.
        // while (!validInputFound && inputQueueExpirations.Count > 0) {

        //     // This input has expired if the current time has exceeded the expiration.
        //     if ( Time.time > inputQueueExpirations.Peek() ) {
        //         PopInput();

        //     } else {
        //         topInput = PopInput();
        //         validInputFound = true;
        //     }
        // }

        return topInput;
    }

    // Discards all expired inputs, and returns the first valid input. Does not remove it from the queue.
    public string PeekQueuedInput() {
        bool validInputFound = false;
        string topInput = "";

        // This loop will continue until either a valid input is found or the queues are completely emptied.
        while (!validInputFound && inputQueueExpirations.Count > 0) {

            // This input has expired if the current time has exceeded the expiration.
            if (inputQueueExpirations.Peek() < Time.time) {
                PopInput();
            } else {
                topInput = inputQueueActions.Peek();
                validInputFound = true;
            }
        }

        // Debug.Log(topInput + " is being polled");

        return topInput;
    }

    // public 

    // Returns the value of corresponding action.
    //      Returns FALSE if the action does not exist in the table.
    //      Optionally, may consume the trigger to reset it's value
    public bool ActionTriggered(string actionName, bool consumeTrigger = false) {
        bool value;
        if (!triggeredActions.TryGetValue(actionName, out value)) {
            return false;
        }

        if (consumeTrigger) {
            ConsumeTrigger(actionName);
        }

        return value;
    }

    public void ConsumeTrigger(string actionName) {
        bool value;
        if (!triggeredActions.TryGetValue(actionName, out value)) {
            return;
        }

        if (value) {
            triggeredActions[actionName] = false;
        }
    }

    // Returns the value of corresponding action.
    //      Returns FALSE if the action does not exist in the table.
    public bool ActionHeld(string actionName) {
        bool value;
        if (!heldActions.TryGetValue(actionName, out value)) {
            return false;
        }
        return value;
    }

    // Awake 
    void Awake() {
        // Component setup
        inputActions = new PlayerInputActions();
        inputComponent = this.GetComponentInParent<PlayerInput>();
        triggeredActions = new Dictionary<string, bool>();
        bufferCoroutines = new Dictionary<string, IEnumerator>();
        heldActions = new Dictionary<string, bool>();

        // Axis init
        mouseAxis = Vector2.zero;

        // Queue setup
        inputQueueActions = new Queue<string>();
        inputQueueExpirations = new Queue<float>();
        
        // Subscribed actions
        inputActions.PlayerControls.Action1.performed += context => BufferInput(context);
        inputActions.PlayerControls.Action2.performed += context => BufferInput(context);
        inputActions.PlayerControls.Action3.performed += context => BufferInput(context);
        inputActions.PlayerControls.Action4.performed += context => BufferInput(context);

        inputActions.InspectControls.Exit.performed += context => BufferInput(context);

    }

    void OnMovement(InputValue value) {
        movementVector = value.Get<Vector2>();
    }

    void OnRotation(InputValue value) {
        rotationVector = value.Get<Vector2>();
    }

    void OnMousePosition(InputValue value) {
        mousePosition = value.Get<Vector2>();
    }

    void OnMouseAxisX(InputValue value) {
        mouseAxis.x = value.Get<float>();
    }

    void OnMouseAxisY(InputValue value) {
        mouseAxis.y = value.Get<float>();
    }
    
    void OnInspectAxisX(InputValue value) {
        inspectAxis.x = value.Get<float>();
    }
    void OnInspectAxisY(InputValue value) {
        inspectAxis.y = value.Get<float>();
    }

    void OnControlsChanged() {
        // Clear previous input data
        // movementVector = Vector2.zero;
        // rotationVector = Vector2.zero;
        
        CheckInputSource();
    }

    // Determines whether gamepad currently being used or not.
    private void CheckInputSource() {
        if (inputComponent.currentControlScheme == "Gamepad") {
            gamepadInput = true;
        } else {
            gamepadInput = false;
        }
    }

    // FixedUpdate is Framerate Independant
    void FixedUpdate() {
        CheckInputSource();
        
        // Check to see if any actions are held this frame
        UpdateHeldAction(InputName.Action1, (inputActions.PlayerControls.Action1.ReadValue<float>() != 0));
        UpdateHeldAction(InputName.Action2, (inputActions.PlayerControls.Action2.ReadValue<float>() != 0));
        UpdateHeldAction(InputName.Action3, (inputActions.PlayerControls.Action3.ReadValue<float>() != 0));
        UpdateHeldAction(InputName.Action4, (inputActions.PlayerControls.Action4.ReadValue<float>() != 0));
    }

    // Take in the current action's context, and buffer it's trigger for some time.
    // This function increases how responsive combat inputs will feel.
    private void BufferInput(InputAction.CallbackContext context) {
        // Make sure the Input is available in the lookup table
        if (!triggeredActions.ContainsKey(context.action.name)) {
            triggeredActions.Add(context.action.name, false);
            bufferCoroutines.Add(context.action.name, null);
        }

        // TEST new Queue system
        if (context.action.name != InputName.Action4 && bufferCoroutines[context.action.name] == null) {
            PushInput(context.action.name);
        }

        // Check if the trigger is already active before starting a coroutine
        if (bufferCoroutines[context.action.name] != null) {
            StopCoroutine(bufferCoroutines[context.action.name]);
        }
        
        IEnumerator newBuffer = Buffer(context.action.name, 0.1f);
        bufferCoroutines[context.action.name] = newBuffer;
        StartCoroutine(newBuffer);
        
    }

    // Check to see if the given Action's input is being held down.
    private void UpdateHeldAction(string actionName, bool value) {
        // Make sure the Action is available in the lookup table
        if (!heldActions.ContainsKey(actionName)) {
            heldActions.Add(actionName, false);
        }

        heldActions[actionName] = value;
    }

    // Activates the designated Action Trigger for some amount of time.
    private IEnumerator Buffer(string actionName, float bufferTime) {
        triggeredActions[actionName] = true;

        while (bufferTime >= 0.0f) {
            // Debug.Log(actionName + " is being buffered");
            bufferTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        triggeredActions[actionName] = false;
        bufferCoroutines[actionName] = null;
    }

    // New Input System requires these function defintions.
    private void OnEnable() {
		inputActions.Enable();
	}

	private void OnDisable() {
		inputActions.Disable();
	}

    void OnDestroy() {
        // Subscribed actions
        inputActions.PlayerControls.Action1.performed -= context => BufferInput(context);
        inputActions.PlayerControls.Action2.performed -= context => BufferInput(context);
        inputActions.PlayerControls.Action3.performed -= context => BufferInput(context);
        inputActions.PlayerControls.Action4.performed -= context => BufferInput(context);

        inputActions.InspectControls.Exit.performed -= context => BufferInput(context);
    }
}
