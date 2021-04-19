using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour {
    [SerializeField] protected PlayableDirector director;
    protected bool played = false;
    public void StartCutscene() {
        if (director != null && !played) {
            director.gameObject.SetActive(true);
            director.Play();
            played = true;
        }
    }
}
