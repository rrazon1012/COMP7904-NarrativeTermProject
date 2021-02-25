using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BaseMotor : MonoBehaviour {

	// CONSTANTS
	private const float DISTANCE_GROUNDED = 0.5F;
	private const float INNER_OFFSET_GROUNDED = 0.05f;
	private const float SLOPE_THRESHOLD = 0.55f;

	// Adjustable Fields
	[Header("Logistic Fields")]
	[SerializeField] protected float baseSpeed = 9.0f;
	public float BaseSpeed { get { return baseSpeed; } }
	[SerializeField] protected float baseGravity = 40.0f;
	[SerializeField] protected float terminalVelocity = 152.0f;
	[SerializeField] protected float slopeThreshold = 55.0f;
	[SerializeField] protected float friction = 15f;
	[SerializeField] protected float mouseTurnSpeed = 0.10f;
	[SerializeField] protected float controllerTurnSpeed = 2f;
	[SerializeField] protected float VerticalVelocity = 0f;

	// Components
	protected CharacterController controller;

	// Modifiers
	[Header("Modifiers")]
	[SerializeField] protected float speedModifier;
	public float SpeedModifier { get { return Mathf.Clamp(speedModifier, lowerModBound, upperModBound); } }
	[SerializeField] protected float turnSpeedModifier;
	public float TurnSpeedModifier { get { return Mathf.Clamp(turnSpeedModifier, lowerModBound, upperModBound); } }
	protected float gravityModifier;

	// Motor overrides:
	public bool active = true;
	protected bool movementLocked;
	public void SetMovementLock(bool newVal) { movementLocked = newVal; }
	protected bool rotationLocked;
	public void SetRotationLock(bool newVal) { rotationLocked = newVal; }

	// Modifier Clamps
	[Header("Modifier Clamps")]
	[SerializeField] protected float lowerModBound = 0.2f; // Mods cannot go below the lower bound.
	[SerializeField] protected float upperModBound = 1.5f; // Mods cannot exceed the upper bound.

	// Moving Platform Support
	private Transform activePlatform;
	private Vector3 activeLocalPoint;
	private Vector3 activeGlobalPoint;
	private Vector3 lastPlatformVelocity;

	// Properties
	public float HorizontalVelocity { set; get; }
	public Vector3 MoveVector { set; get; }
	public Vector3 ForceVector { set; get; }
	public Vector3 LastDirection { set; get; }
	public Quaternion RotationQuaternion { set; get; }
	public Vector3 InputVector { set; get; }
	public Vector3 DirectionVector { set; get; }
	public Vector3 WallVector { set; get; }
	public Vector3 SlopeNormal { set; get; }
	public Animator Anim { set; get; }
	public CollisionFlags ColFlags { set; get; }

	#region Getters
	public float Speed { get { return baseSpeed * SpeedModifier; } }
	public float ControllerTurnSpeed { get { return controllerTurnSpeed * TurnSpeedModifier; } }
	public float MouseTurnSpeed { get { return mouseTurnSpeed * TurnSpeedModifier; } }
	public float Gravity { get { return baseGravity * gravityModifier; } }
	public float SlopeThreshold { get { return slopeThreshold; } }
	public float TerminalVelocity { get { return terminalVelocity; } }
	#endregion

	#region Start
	public virtual void Start() {
		// Component setup
		controller = GetComponent<CharacterController>();
		Anim = GetComponentInChildren<Animator>();

		// Modifier setup
		turnSpeedModifier = 1f;
		gravityModifier = 1f;
		speedModifier = 1f;
	}
	#endregion

	#region Update

	// Called by state on Update(), checks for input and moves in direction
	public virtual void UpdateMotor() {
		if (!active) {
			return;
		}	

		// Input movement
		Move();
		Rotate();

		// Moving platforms
		MovingPlatformPostMove();

		LastDirection = new Vector3(MoveVector.x, 0, MoveVector.z);
		HorizontalVelocity = LastDirection.magnitude;
	}

	protected virtual void Move() {
		// Apply Movement Vector over time, plus any additional force.
		if (controller.enabled == false) {
			controller.enabled = true;
		}
		
		ColFlags = controller.Move ( (MoveVector * Time.deltaTime) + (ForceVector));

		// If the motor's collider flags are non-zero, prevent movement & stop Force Vector.
		if ((ColFlags & CollisionFlags.Sides) != 0) {
			WallVector = WallVector;
			if (ForceVector.magnitude > 10f) {
				ForceVector = ForceVector * -0.25f;
			}
			
		} else {
			WallVector = Vector3.zero;
		}
	}

	protected virtual void Rotate() {
		if (MoveVector.magnitude != 0) {
			transform.rotation = RotationQuaternion;
		}
	}
	#endregion

	#region Methods
	
	public virtual void ApplyForce(Vector3 direction, float magnitude, float rateOfDecay = 0f) {
		// Add the new force to the ForceVector, applied away from the incoming attack.
		if (direction.magnitude > 1) {
			Debug.LogWarning("The magnitude of " + this.name + "'s Force Vector exceeds 1, and may lead to unpredictible behavior.");
		}

		// Vector3 normalizedForce = (this.transform.position - sourceDirection.position).normalized;
		direction.y = 0f;
		ForceVector += direction * magnitude;
		
		// Begin decaying ForceVector, which decays at a rate based on the current friction of the Motor.
		StartCoroutine(DecayForceVector(rateOfDecay));
	}

	protected IEnumerator DecayForceVector(float rateOfDecay = 0f) {
		Vector3 startingForceValue = ForceVector;
		float decayDivisor = 1000f; // Scales down friction / rateOfDecay to a decimal timestep.

		while (ForceVector != Vector3.zero) {
			float decay = rateOfDecay > 0f ? (rateOfDecay / decayDivisor) : (friction / decayDivisor);

			// Push the ForceVector towards Vector3.zero
			ForceVector = Vector3.MoveTowards(ForceVector, Vector3.zero, decay);
			yield return new WaitForFixedUpdate();
		}
		
	}

	// A move function that offers more control over the direction and speed of the step.
	public virtual void Step(Vector3 direction, float stepSpeed) {
		// Debug.Log(transform.forward);
		// Debug.Log("Facing Direction:" + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z);
		MoveVector = ProcessStep(transform.forward, stepSpeed);
		
		// Moving plaform pre-move
		MovingPlatformPreMove();

		Move();

		// Moving platform post-move
		MovingPlatformPostMove();

		// Store velocity for next frame
		LastDirection = new Vector3(MoveVector.x, 0, MoveVector.z);
		HorizontalVelocity = LastDirection.magnitude;
	}

	public void CombatStep(float windupSpeed, float attackSpeed, float recoverySpeed, Vector3 direction, float stepSpeed) {
		StartCoroutine(MoveWithDelay(windupSpeed, attackSpeed, recoverySpeed, direction, stepSpeed));
	}

	// Move forward after some delay (windup), for some distance (duration), in some direction, at a set speed.
    public IEnumerator MoveWithDelay(float windupSpeed, float duration, float recovery, Vector3 direction, float stepSpeed)
    {
		yield return new WaitForSeconds(windupSpeed);

		// SetMovementLock(true);
		// SetRotationLock(true);
		
        while (duration >= 0.0f) {
            duration -= Time.fixedDeltaTime;
            Step(direction, stepSpeed);
            yield return new WaitForFixedUpdate();
        }

		// SetMovementLock(false);
		// // If using controller...?
		// SetRotationLock(false);

		while (recovery >= 0.0f) {
            recovery -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

	// Used in combat to create a target Vector from input at a given speed.
	private Vector3 ProcessStep(Vector3 input, float stepSpeed) {
		VerticalVelocity = 0;
		MotorHelper.KillVector(ref input, WallVector);
		MotorHelper.FollowVector(ref input, SlopeNormal);
		MotorHelper.ApplySpeed(ref input, stepSpeed);
		return input;
	}

	public virtual bool Grounded() {
		float yRay = controller.bounds.center.y - (controller.height * 0.5f) + 0.3f;
		RaycastHit hitInfo;

		// Mid
		if (Physics.Raycast(new Vector3(controller.bounds.center.x, yRay, controller.bounds.center.z), Vector3.down, out hitInfo, DISTANCE_GROUNDED)) {
			SlopeNormal = hitInfo.normal;
			return (SlopeNormal.y > SLOPE_THRESHOLD) ? true : false;
		}

		// Front-Right
		if (Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - INNER_OFFSET_GROUNDED), yRay, controller.bounds.center.z - INNER_OFFSET_GROUNDED), Vector3.down, out hitInfo, DISTANCE_GROUNDED)) {
			SlopeNormal = hitInfo.normal;
			return (SlopeNormal.y > SLOPE_THRESHOLD) ? true : false;
		}

		// Front-Left
		if (Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - INNER_OFFSET_GROUNDED), yRay, controller.bounds.center.z + (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)), Vector3.down, out hitInfo, DISTANCE_GROUNDED)) {
			SlopeNormal = hitInfo.normal;
			return (SlopeNormal.y > SLOPE_THRESHOLD) ? true : false;
		}

		// Back-Right
		if (Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - INNER_OFFSET_GROUNDED), yRay, controller.bounds.center.z - (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)), Vector3.down, out hitInfo, DISTANCE_GROUNDED)) {
			SlopeNormal = hitInfo.normal;
			return (SlopeNormal.y > SLOPE_THRESHOLD) ? true : false;
		}

		// Back-Left
		if (Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - INNER_OFFSET_GROUNDED), yRay, controller.bounds.center.z + (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)), Vector3.down, out hitInfo, DISTANCE_GROUNDED)) {
			SlopeNormal = hitInfo.normal;
			return (SlopeNormal.y > SLOPE_THRESHOLD) ? true : false;
		}
		return false;
	}

	protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.moveDirection.y < 0.9f && hit.normal.y > 0.5f) {
			activePlatform = hit.collider.transform;
		}

		if (VerticalVelocity > 0 && ((ColFlags & CollisionFlags.Above) != 0)) {
			VerticalVelocity = 0;
		}
	}

	protected void MovingPlatformPreMove() {
		if (activePlatform != null) {
			var newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPoint);
			var moveDistance = (newGlobalPlatformPoint - activeGlobalPoint);
			if (moveDistance != Vector3.zero) {
				controller.Move(moveDistance);
			}

			lastPlatformVelocity = moveDistance / Time.deltaTime;
		} else {
			lastPlatformVelocity = Vector3.zero;
		}
	}

	protected void MovingPlatformPostMove() {
		if (activePlatform != null) {
			activeGlobalPoint = transform.position;
			activeLocalPoint = activePlatform.InverseTransformPoint(activeGlobalPoint);
		}
	}


	#endregion
}
