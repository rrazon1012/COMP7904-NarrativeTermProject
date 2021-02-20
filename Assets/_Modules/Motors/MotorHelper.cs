using UnityEngine;

public static class MotorHelper {

	/* Multiply input vector by speed */
	public static void ApplySpeed(ref Vector3 vector, float speed) {
		vector *= speed;
	}

	public static float ApplyGravity(ref Vector3 vector, ref float verticalVelocity, float gravity, float terminalVelocity) {
		verticalVelocity -= (gravity * Time.deltaTime);
		verticalVelocity = (verticalVelocity < -terminalVelocity) ? -terminalVelocity : verticalVelocity;
		vector.Set(vector.x, verticalVelocity, vector.z);
		return verticalVelocity;
	}

	// Get rid of input's components that are going in the direction of the toKill Vector
	public static void KillVector(ref Vector3 vector, Vector3 toKill) {
		toKill.Set(toKill.x, 0, toKill.z);
		toKill.Normalize();

		if (toKill.x > 0 && vector.x < 0)
			vector.Set((1 - toKill.x) * vector.x, vector.y, vector.z);
		if (toKill.x < 0 && vector.x > 0)
			vector.Set((1 + toKill.x) * vector.x, vector.y, vector.z);

		if (toKill.z > 0 && vector.z < 0)
			vector.Set(vector.x, vector.y, (1 - toKill.z) * vector.z);
		if (toKill.z < 0 && vector.z > 0)
			vector.Set(vector.x, vector.y, (1 + toKill.z) * vector.z);
	}

	// Rotate the vector so it matches the floor's normal
	public static void FollowVector(ref Vector3 vector, Vector3 slopeNormal) {
		Vector3 right = new Vector3 (slopeNormal.y, -slopeNormal.x, 0).normalized;
		Vector3 forward = new Vector3 (0, -slopeNormal.z, slopeNormal.y).normalized;
		vector = right * vector.x + forward * vector.z;
	}

	// Returns Vector3 x, 0, z
	public static void KillVertical(ref Vector3 vector) {
		vector = new Vector3 (vector.x, 0, vector.z);
	}

	// Returns rotation facing move
	public static Quaternion FaceDirection(Vector3 move) {
		Vector3 dir = move;
		MotorHelper.KillVertical(ref dir);
		if (dir == Vector3.zero) {
			return Quaternion.identity;
		}
		return Quaternion.LookRotation(dir, Vector3.up);
	}
}
