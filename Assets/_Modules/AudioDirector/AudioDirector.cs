using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDirector : MonoBehaviour {

    // Singleton instance
    [SerializeField] private static AudioDirector _instance;
    public static AudioDirector Instance { get { return _instance; } }

    [Header("Configuration")]
    public const float TRACK_TRANSITION_TIME = 3f;
    [SerializeField] protected AudioSource trackMixer;
    [SerializeField] protected float baseTrackVolume = 0.5f;
    protected IEnumerator trackTransitionCoroutine;
    [SerializeField] protected AudioSource ambienceMixer;

    [Header("Tunable Parameters")]
    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
    [SerializeField] private float maxDistance = 20f;

    // Global SFX Resources
    // [Header("Global Audio Collections")]

    [Header("Global UI Audio Collections")]
    public AudioCollection SFX_UI_Positive;
    public AudioCollection SFX_UI_Negative;
    
    private AudioSource source;
    private GameObject oneShotClips;

    [Header("Camera (Listener) Config")]
    private Camera mainCamera;
    [SerializeField] protected Vector3 cameraLocationOffset;

    // Start is called before the first frame update
    void Start() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(this.gameObject);
        }

        mainCamera = Camera.main;
        source = this.GetComponent<AudioSource>();
        
        // New gameObject to store audio clips.
        oneShotClips = new GameObject("OneShotAudio");
        oneShotClips.transform.parent = this.transform;
    }

    public void SetTrack(AudioClip newTrack, float volume, bool loop = false) {
        if (newTrack != trackMixer.clip) {
            
            trackMixer.loop = loop;

            if (trackTransitionCoroutine != null) {
                StopCoroutine(trackTransitionCoroutine);
            }

            trackTransitionCoroutine = TrackTransition(newTrack, volume);
            StartCoroutine(trackTransitionCoroutine);
            
        }
        
    }

    protected IEnumerator TrackTransition(AudioClip newTrack, float newVolume) {
        float startingVolume = baseTrackVolume;

        // Fade out the current track.
        while (trackMixer.volume > 0.0f) {
            trackMixer.volume -= Mathf.Clamp(startingVolume * (Time.fixedDeltaTime / TRACK_TRANSITION_TIME), 0, Mathf.Infinity);
            yield return new WaitForFixedUpdate();
        }

        // At this point, volume is zero. Swap tracks.
        trackMixer.Stop();
        trackMixer.clip = newTrack;

        if (newTrack != null) {
            trackMixer.Play();

            // Bring new track up to it's target volume
            baseTrackVolume = newVolume;
            while (trackMixer.volume < baseTrackVolume) {
                trackMixer.volume += Mathf.Clamp (baseTrackVolume * (Time.fixedDeltaTime / TRACK_TRANSITION_TIME), 0, baseTrackVolume);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void SetAmbience(AudioClip newAmbience, float volume) {
        if (newAmbience != null && newAmbience != ambienceMixer.clip) {
            ambienceMixer.Stop();
        
            ambienceMixer.clip = newAmbience;
            ambienceMixer.Play();
        }
    }

    // Point functions (Diagetic, position based)

    // Plays a single random audio clip at a given position.
    // Multiple Audio Clips can play simultaneously.
    public void PlayRandomAudioAtPoint(AudioCollection clipCollection, Vector3 position) {
        if (clipCollection == null) {
            return;
        }

        // Try to get a random clip. If unsuccessful, 
        AudioClip randomClip = clipCollection.GetRandomClip();
        
        if (randomClip != null) {

            // 1. Create new Audio object
            GameObject oneShotObject = new GameObject(randomClip.name);
            oneShotObject.transform.parent = oneShotClips.transform;

            CreateAudioSource(randomClip, oneShotObject, position, clipCollection.Volume, clipCollection.PitchVariance);

            Destroy(oneShotObject, randomClip.length);
        }
        
    }

    // Plays a continuous random audio clip parented to a given source.
    // Multiple Audio Clips can play simultaneously.
    public GameObject PlayRandomContinuousAudio(AudioCollection clipCollection, Transform source, float maxDuration = 10f) {
        
        if (clipCollection == null) {
            return null;
        }

        // Try to get a random clip. If unsuccessful, 
        AudioClip randomClip = clipCollection.GetRandomClip();
        GameObject continuousObject = null;

        if (randomClip != null) {

            // 1. Create new Audio object
            continuousObject = new GameObject(randomClip.name);
            continuousObject.transform.parent = source.transform;
            Vector3 positionOffset = cameraLocationOffset;

            CreateAudioSource(randomClip, continuousObject, source.position - new Vector3(positionOffset.x, 0, positionOffset.z), clipCollection.Volume, clipCollection.PitchVariance, true);
            Destroy(continuousObject, maxDuration);
        }

        return continuousObject;
    }

    // Creates an audio source at the given position, excluding the Y axis.
    public void CreateAudioSource(AudioClip clip, GameObject audioObject, Vector3 position, float volume, float pitchVariance = 0f, bool loop = false, bool diagetic = true) {
        
        if (clip != null) {
            
            // 1. Create new AudioSource component
            AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
            float clipDuration = clip.length;

            // 2. Calculate the position relative to the listener
            Vector3 offsetVector = cameraLocationOffset;
            Vector3 listenerPosition = position + offsetVector;
            audioObject.transform.position = listenerPosition;

            // 3. Configure the Audio Source
            newAudioSource.playOnAwake = false;
            newAudioSource.clip = clip;
            newAudioSource.volume = volume * source.volume;
            newAudioSource.loop = loop;

            if (diagetic) {
                newAudioSource.spatialBlend = 1f;
            } else {
                newAudioSource.spatialBlend = 0f;
            }
            

            newAudioSource.rolloffMode = rolloffMode;
            newAudioSource.maxDistance = maxDistance;

            // 4. Adjust pitch if provided with a variance
            if (pitchVariance > 0) {
                newAudioSource.pitch += Random.Range(-pitchVariance, pitchVariance);
            }
            
            // 5. Play the audio
            newAudioSource.Play();
        }
        
    }


    // One-shot functions (Non-diagetic)
    public void PlayRandomUIAudio(AudioCollection clipCollection) {
        if (clipCollection == null) {
            return;
        }

        // Try to get a random clip. If unsuccessful, 
        AudioClip randomClip = clipCollection.GetRandomClip();
        if (randomClip != null) {
            // 1. Create new Audio object
            GameObject oneShotObject = new GameObject(randomClip.name);
            oneShotObject.transform.parent = oneShotClips.transform;

            CreateAudioSource(randomClip, oneShotObject, this.transform.position, clipCollection.Volume, clipCollection.PitchVariance, false, false);
            Destroy(oneShotObject, randomClip.length);
        }
    }
    
    public void PlayUIAudio(AudioClip clip) {
        // Try to get a random clip. If unsuccessful, 
        if (clip != null) {
            source.PlayOneShot(clip);
        }
    }
}
