using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable Object construct to store a collection 
[CreateAssetMenu (menuName = "Audio/Collection")]
public class AudioCollection : ScriptableObject {
    [Range(0, 1)] [SerializeField] float clipVolume = 1f;
    public float Volume { get { return clipVolume; } set { clipVolume = value; } }

    [Range(0, 1)] [SerializeField] float pitchVariance = 0f;
    public float PitchVariance { get { return pitchVariance; } }

    [SerializeField] private AudioClip[] clips;

    // Select and return a random Audio Clip from the collection
    public AudioClip GetRandomClip() {
        int selectedClip = Random.Range(0, clips.Length);
        return clips[selectedClip];
    }
}
