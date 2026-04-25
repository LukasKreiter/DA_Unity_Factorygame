using System;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    [Header("Output")]
    public int resolution = 256;
    public Renderer targetRenderer;

    private Texture2D result;
    private Node_Blueprint outputNode;

    void OnValidate()
    {
        BuildGraph();
        Generate();
    }

    void Start()
    {
        BuildGraph();
        Generate();
    }

    void BuildGraph()
    {

        var perlin1 = new PerlinNoise(1, 10f)
        {
            useFractal = false,
            octaves = 1,
            gain = 1f
        };

        var perlin2 = new PerlinNoise(1, 6f)
        {
            useFractal = true,
            octaves = 6,
            gain = 0.5f
        };

        outputNode = new CombineNode(perlin1, perlin2, CombineMode.Add);
        //outputNode = perlin1;

        outputNode.Init();
    }

    public void Generate()
    {
        if (outputNode == null)
        {
            Debug.LogWarning("No output node assigned.");
            return;
        }

        result = NoiseTextureBuilder.Generate(outputNode, resolution);

        if (targetRenderer != null)
        {
            Material mat = targetRenderer.sharedMaterial;
            mat.SetTexture("_BaseColorMap", result);
            mat.SetTexture("_UnlitColorMap", result);
        }
    }
}