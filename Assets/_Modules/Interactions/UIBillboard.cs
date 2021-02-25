using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    protected Transform targetCamera;

    void Start() {
        targetCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (targetCamera != null) {
            transform.LookAt(transform.position + targetCamera.forward);
        }
    }
}
