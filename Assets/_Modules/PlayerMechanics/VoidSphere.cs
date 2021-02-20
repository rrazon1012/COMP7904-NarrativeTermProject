using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidSphere : MonoBehaviour {

    [SerializeField] [Range(0f, 15f)] public float voidRadius = 3.5f;
    protected float VoidScale { get { return voidRadius * 2; } }

    [Header("Shader Config")]
    [SerializeField] protected Texture2D voidNoise;
    [SerializeField] protected float noiseScale = 0.3f;

    [SerializeField] protected Color edgeColor;
    [SerializeField] [Range(0.01f, 1f)] protected float edgeWidth = 0.01f;

    void Start() {
        Shader.SetGlobalTexture("_NoiseTexture", voidNoise);
        Shader.SetGlobalColor("_EdgeColor", edgeColor);
        Shader.SetGlobalFloat("_EdgeWidth", edgeWidth);
        Shader.SetGlobalVector("_NoiseScale", new Vector2(noiseScale, noiseScale));
    }

    // Update is called once per frame
    void Update() {
        this.transform.localScale = new Vector3(VoidScale, VoidScale, VoidScale);

        UpdateGlobalShaderProperties();
    }

    protected void UpdateGlobalShaderProperties() {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_SphereRadius", transform.localScale.x/2);
    }
}
