using System;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    [Header("Noise A")]
    public int seedA = 1;
    public float freqA = 3f;

    [Header("Noise B")]
    public int seedB = 2;
    public float freqB = 6f;

    [Header("Blend")]
    public int blendMode = 0;

    [Header("Output")]
    public int resolution = 256;
    public Renderer targetRenderer;

    Texture2D texA;
    Texture2D texB;
    Texture2D result;
    bool generated = false;


    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        // Create nodes
        var nodeA = new PerlinNoise(seedA, freqA);
        var nodeB = new PerlinNoise(seedB, freqB);

        // Build textures
        texA = NoiseTextureBuilder.Generate(nodeA, resolution);
        texB = NoiseTextureBuilder.Generate(nodeB, resolution);

        // Blend
        result = NoiseBlend.Blend(texA, texB, blendMode);

        // Preview
        if (targetRenderer != null)
        {
            Material mat = targetRenderer.material;

            mat.mainTexture = result;
            mat.SetTexture("_BaseColorMap", result);
            mat.SetTexture("_UnlitColorMap", result);

            Debug.Log("Renderer material assigned: " + mat.name);
        }
    }
}