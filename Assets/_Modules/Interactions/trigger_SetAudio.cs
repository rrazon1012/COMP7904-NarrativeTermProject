using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_SetAudio : MonoBehaviour
{
    [SerializeField] AudioClip track;
    [SerializeField] float trackVolume = 0.5f;
    [SerializeField] AudioClip ambience;
    [SerializeField] float ambientVolume = 0.5f;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            AudioDirector.Instance.SetTrack(track, trackVolume);
            AudioDirector.Instance.SetAmbience(ambience, ambientVolume);
        }
    }
}
