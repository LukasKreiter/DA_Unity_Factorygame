using System;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    [Header("Output")]
    public int resolution = 256;
    public Renderer targetRenderer;

    [Header("Erosion")]
    public bool useErosion = true;

    public ComputeShader erosionShader;

    [Range(32, 1024)]
    public int erosionResolution = 256;

    public ErosionSettings erosionSettings =
        new ErosionSettings();

    public event Action<Node_Blueprint> OnGraphBuilt;

    Texture2D result;
    Node_Blueprint outputNode;

    void Start()
    {
        Rebuild();
    }

    void OnValidate()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        BuildGraph();
        Generate();

        OnGraphBuilt?.Invoke(outputNode);
    }


    void BuildGraph()
    {
        var baseNoise = new PerlinNoise(3, 3f) // base shape is created 
        {
            useFractal = false
        };

        var cone = new ConeNode(0.35f); // cone shape for masking

        var maskedNoise = new CombineNode(baseNoise, cone, CombineMode.Multiply); // mask with multiply

        var mountainShape = new CombineNode(cone, maskedNoise, CombineMode.Add); // lift maskedNoise with cone shape to make it more mountain-like

        var mountainDetail = new PerlinNoise(4, 4f) // fractal noise for mountain detail
        {
            useFractal = true,
            octaves = 5,
            gain = 0.5f,
        };

        var maskedDetail = new CombineNode(mountainDetail, cone, CombineMode.Multiply); // mask the noise to keep surrounding terrain flat

        var mountain = new CombineNode(mountainShape, maskedDetail, CombineMode.Add); // apply detail to mountain

        var warpedMountain = new SwirlNode(
            mountain,
            .6f,
            1.5f
        );

        //if (useErosion && erosionShader != null)
            //outputNode =
            //    new ErosionNode(
            //        warpedMountain,
            //        erosionShader, 
            //        erosionResolution,
            //        erosionSettings);
        //else
            //outputNode = warpedMountain;

        //var island = new IslandNode(0.45f, coastNoise);

        //var shaped = new CurveNode(island, 5f);

        //var terrace = new TerraceNode(warpedMountain, 2, 0.2f);

        var macro = new PerlinNoise(2, 9f)
        {
            useFractal = true,
            octaves = 3
        };

        var coast = new PerlinNoise(7, 8f)
        {
            useFractal = true,
            octaves = 5
        };

        var warp = new PerlinNoise(9, 8f)
        {
            useFractal = true,
            octaves = 3
        };

        outputNode =
            new IslandNode(
                0.35f,
                macro,
                coast,
                warp
            );

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