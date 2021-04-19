using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        this.GetComponent<CutsceneController>().StartCutscene();
    }

}
