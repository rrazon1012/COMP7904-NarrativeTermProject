using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidSphere : MonoBehaviour {

    // Implemented VoidSphere as a singleton, so it's location can be easily referenced by shaders & scripts.
    protected static VoidSphere _instance;
    public static VoidSphere Instance {
        get {

            // If a Void Sphere has not been created, load one from resources.
            if (_instance != null) {
                return _instance;
            } else {
                _instance = Instantiate( Resources.Load<VoidSphere>("VoidSphere") );
                return _instance;
            }
        }
    }

    protected ColliderContainer collisionManager;
    protected AudioSource audioSource;

    // Particle emitter and shape modules
    protected ParticleSystem particles;
    protected ParticleSystem.ShapeModule particleShape;

    public bool ObjectJustOnEdgeCollision(Collider collision) {
        List<Collider> activeCollisions = collisionManager.GetAllColliders;
        return activeCollisions.Contains(collision);
    }

    public bool ObjectFullyWithinCollision(Collider collision, float colliderRadius) {
        Vector3 distanceToObject = (collision.transform.position - this.transform.position);

        // Check what kind of collider we're working with.
        
        if (distanceToObject.magnitude < (voidRadius - (colliderRadius * 0.5f))) {
            return true;
        }
        return false;
    }

    [SerializeField] [Range(0f, 15f)] public float voidRadius = 3.5f;
    protected float VoidScale { get { return voidRadius * 2; } }
    public bool Active { get { return voidRadius > 0; } }

    [Header("Shader Config")]
    [SerializeField] protected Texture2D voidNoise;
    [SerializeField] protected float noiseScale = 0.3f;

    [SerializeField] protected Color edgeColor;
    [SerializeField] [Range(0.01f, 1f)] protected float edgeWidth = 0.01f;

    void Awake() {
        _instance = this;
        collisionManager = this.GetComponent<ColliderContainer>();
        audioSource = this.GetComponent<AudioSource>();
        particles = this.GetComponentInChildren<ParticleSystem>();
        particleShape = particles.shape;
    }

    void Start() {
        Shader.SetGlobalTexture("_NoiseTexture", voidNoise);
        Shader.SetGlobalColor("_EdgeColor", edgeColor);
        Shader.SetGlobalFloat("_EdgeWidth", edgeWidth);
        Shader.SetGlobalVector("_NoiseScale", new Vector2(noiseScale, noiseScale));
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.transform.localScale = new Vector3(VoidScale, VoidScale, VoidScale);

        UpdateGlobalShaderProperties();
        UpdateParticleEmission();

        if (Active) {
            audioSource.volume = 0.2f;
        } else {
            audioSource.volume = 0.0f;
        }
    }

    protected void UpdateGlobalShaderProperties() {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_SphereRadius", transform.localScale.x/2);
    }

    protected void UpdateParticleEmission() {
        ParticleSystem.EmissionModule emission = particles.emission;

        if (Active) {
            emission.rateOverTime = 120;
        } else {
            emission.rateOverTime = 0;
        }

        particleShape.radius = voidRadius;
    }
}
