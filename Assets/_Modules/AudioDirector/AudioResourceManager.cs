using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to components with animators, allowing them to respond to Audio-related events.
public class AudioResourceManager : MonoBehaviour
{
    
    [Header("Footsteps")]
    [SerializeField] AudioCollection[] footstepClips;
    private void Footstep() {
        if (footstepClips == null) {
            return;
        }

        foreach(AudioCollection collection in footstepClips) {
            AudioDirector.Instance.PlayRandomAudioAtPoint(collection, this.transform.position);
        }
        
    }
}
