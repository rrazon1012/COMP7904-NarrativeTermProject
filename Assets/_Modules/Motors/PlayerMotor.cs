using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : BaseMotor {
	[SerializeField] protected Camera mainCamera;

	protected NavMeshAgent pathingAgent;
	private float xRotation = 0f;

	// Component references:
	public Transform CameraTransform { set; get; }
	private InputBuffer inputBuffer;
	
	public override void Start() {
		base.Start();

		// Component reference setup
		inputBuffer = this.GetComponent<InputBuffer>();
		pathingAgent = this.GetComponent<NavMeshAgent>();

		// Initialize Player's base stats
		// baseSpeed = statManager.GetBaseStatOfType(StatManager.StatType.Speed);

		//Initialize the player's camera
		CameraTransform = Camera.main.transform;
		mainCamera = Camera.main;

		// Set locks to false
		movementLocked = false;
		rotationLocked = false;
	}

	public void FixedUpdate() {
		UpdateMotor();
	}

	protected override void Move() {
		// Apply Movement Vector over time, plus any additional force.
		// ColFlags = controller.Move ( (MoveVector * Time.deltaTime) + (ForceVector));
		pathingAgent.Move((MoveVector * Time.deltaTime) + (ForceVector));

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

	// The method that actually looks for input.
	public override void UpdateMotor() {
		if (!active) {
			return;
		}
		
		// Get Joysticks Input, store in MoveVector
		Vector3 input = CreateInputVector(inputBuffer.GetMovementVector());
			InputVector = input;
		
		
		// Transfer inputs to MoveVector before processing
		// Vector3 localInputVector = new Vector3();
		
		// this.transform.forward

		MoveVector = MoveInputToLocalVector(input);
		DirectionVector = MoveInputToLocalVector(input);

		// Mobility State Calculates motion
		if (Grounded()) {
			MoveVector = ProcessWalk(MoveVector);
		} else {
			MoveVector = ProcessFall(MoveVector);
		}
		
		// Moving plaform pre-move
		MovingPlatformPreMove();

		// Move the controller
		if (!movementLocked) {
			Move();
		}
		
		if (!rotationLocked) {
			Rotate();
		}
		
		// Moving platform post-move
		MovingPlatformPostMove();

		// Store velocity for next frame
		LastDirection = new Vector3(MoveVector.x, 0, MoveVector.z);
		HorizontalVelocity = LastDirection.magnitude;
	}

	public Vector3 CreateInputVector(Vector2 input) {
		Vector3 dir = Vector3.zero;

		dir.x = input.x;
		dir.z = input.y;

		if (dir.sqrMagnitude > 1) {
			dir.Normalize();
		}

		return dir;
	}

	public new void Rotate() {
		// Mouse looking variables
		// Plane groundPlane = new Plane(Vector3.up, this.transform.position);
		Vector3 rotationVector;

		// Controller Rotations
		if (inputBuffer.GamepadInputActive()) {
			rotationVector = CreateInputVector(inputBuffer.GetRotationVector()) ;
			if (rotationVector.sqrMagnitude > 0.0f) {
				float curX = transform.rotation.x;

				float lookX = rotationVector.x * controllerTurnSpeed;
				float lookY = rotationVector.z * controllerTurnSpeed;
				xRotation -= lookY;
				xRotation = Mathf.Clamp(xRotation, -90f, 90f);
				//Debug.Log("xRotation: " + xRotation);
				//IT IS DONE!!!!
				CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
				transform.Rotate(Vector3.up * lookX);
			}
		// Mouse Rotations
		} else {
			Vector2 axis = inputBuffer.GetMouseAxis();

			float lookX = axis.x * mouseTurnSpeed;
			float lookY = axis.y * mouseTurnSpeed;
			xRotation -= lookY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			transform.Rotate(Vector3.up * lookX);
		}
		
		Vector3 moveDirection = CreateInputVector(inputBuffer.GetMovementVector());

		Vector3 localInputVector = new Vector3();
		localInputVector = (this.transform.forward * moveDirection.z) + (this.transform.right * moveDirection.x);

		// Get current move vectors for animation.
		float moveX = transform.InverseTransformDirection(moveDirection).x * SpeedModifier;
		float moveZ = transform.InverseTransformDirection(moveDirection).z * SpeedModifier;

		// Anim.SetFloat("MoveX", moveX, .05f, Time.deltaTime);
		// Anim.SetFloat("MoveZ", moveZ, .05f, Time.deltaTime);

		// Vector2 moveVector2 = new Vector2(moveX, moveZ);
		// Anim.SetBool("Running", moveVector2.magnitude > Mathf.Epsilon);

		// // See what vector has a greater value to determine speed of motion.
		// // It's a value between 0 and 1 such that the animation is relative to max speed.
		// if (Mathf.Abs(moveX) > Mathf.Abs(moveZ)) {
		// 	Anim.SetFloat("Speed",  Mathf.Abs(moveX), .05f, Time.deltaTime);
		// } else {
		// 	Anim.SetFloat("Speed", Mathf.Abs(moveZ), .05f, Time.deltaTime);
		// }

	}

	protected Vector3 MoveInputToLocalVector(Vector3 input) {
		Vector3 localVector = Vector3.zero;
		localVector = (this.transform.forward * input.z) + (this.transform.right * input.x);
		return localVector;
	}

	protected Vector3 RotationInputToLocalVector(Vector3 input) {
		Vector3 localVector = Vector3.zero;
		localVector = (this.transform.up * input.z) + (this.transform.right * input.x);
		return localVector;
	}

	// Used by the motor to create a target Vector from input, at modified Speed
	private Vector3 ProcessWalk(Vector3 input) {
		VerticalVelocity = 0;
		MotorHelper.KillVector(ref input, WallVector);
		MotorHelper.FollowVector(ref input, SlopeNormal);
		MotorHelper.ApplySpeed(ref input, Speed);
		return input;
	}

	public Vector3 ProcessFall(Vector3 input) {
		MotorHelper.KillVector(ref input, WallVector);
		MotorHelper.ApplySpeed(ref input, Speed);
		MotorHelper.ApplyGravity(ref input, ref VerticalVelocity, Gravity, TerminalVelocity);
		// Influence Air Velocity function not written currently
		return input;
	}
}
