using System;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    [Header("Output")]
    public int resolution = 256;
    public Renderer targetRenderer;
    public event Action<Node_Blueprint> OnGraphBuilt;
    private Texture2D result;
    private Node_Blueprint outputNode;

    void OnValidate()
    {
        Rebuild();
    }

    void Start()
    {
        Rebuild();
    }

    void Rebuild()
    {
        BuildGraph();
        Generate();

        OnGraphBuilt?.Invoke(outputNode);
    }

    void BuildGraph()
    {
        var baseNoise = new PerlinNoise(1, 3f)
        {
            useFractal = false
        };

        var cone = new ConeNode(0.35f);

        // mask with multiply
        var maskedNoise = new CombineNode(baseNoise, cone, CombineMode.Multiply);

        var mountainShape = new CombineNode(cone, maskedNoise, CombineMode.Add);

        var mountainDetail = new PerlinNoise(4, 4f)
        {
            useFractal = true,
            octaves = 5,
            gain = 0.5f,
        };

        var maskedDetail = new CombineNode(mountainDetail, cone, CombineMode.Multiply);

        outputNode = new CombineNode(mountainShape, maskedDetail, CombineMode.Add);

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

    public Node_Blueprint GetOutputNode()
    {
        return outputNode;
    }
}