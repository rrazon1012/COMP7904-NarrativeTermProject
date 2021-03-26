using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This vfx script points the mirror camera in an angle opposite to the player's view direction.
//      This will result in a more realistic effect, as well as providing some mechanical utility.
public class vfx_MirrorAngle : MonoBehaviour
{

    [SerializeField] protected GameObject playerObject; // This should be found automatically, through some singleton GameDirector.
    [SerializeField] protected float maxViewAngle = 70f;
    protected Camera mirrorCamera;

    protected FieldOfView fov;
    protected float maxAngle;

    // Start is called before the first frame update
    void Start() {
        fov = this.GetComponent<FieldOfView>();
        mirrorCamera = this.GetComponentInChildren<Camera>();

        if (mirrorCamera == null) {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        SetCameraAngle();
    }

    protected void SetCameraAngle() {
        // 1. Get the vector between the mirror and the player
        Vector3 camToPlayer = this.transform.DirectionTo(playerObject.transform.position) * 2;
        camToPlayer.y = 0;

        // 2. Get the vector representing the mirror's forward facing direction.
        Vector3 forward = -transform.TransformDirection(this.transform.forward) * 2;
        forward.y = 0;
        
        // 3. Find the opposing angle between those two vectors, clamped bewteen the max viewing angle.
        float viewingAngle = Mathf.Clamp(Vector3.Angle(forward, camToPlayer), 0, maxViewAngle);
        if (fov.WithinAngle(playerObject.transform.position, -this.transform.right, 180f)) {
            viewingAngle = -viewingAngle;
        }

        // 4. Set the rotation of the camera to be that opposing angle, clamped to a max angle.
        mirrorCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 0 + viewingAngle, 0));
    }
}
