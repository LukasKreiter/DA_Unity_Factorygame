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
    
    [Header("Blur")]
    public ComputeShader blurShader;

    public event Action<Node_Blueprint> OnGraphBuilt;

    Texture2D result;
    Texture2D slopeMaskTex;
    Texture2D heightMaskTex;
    Texture2D flowMaskTex;
    Node_Blueprint outputNode;
    Node_Blueprint slopeMask;
    Node_Blueprint heightMask;
    Node_Blueprint flowMask;

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
        // ------------------------------------------------------- primitve noises 

        // mountain noise
        var mountain_ridged = new RidgedNoiseNode(5, 3f)
        {
            octaves = 5,
            lacunarity = 2f,
            gain = 0.6f
        };
        var mountain_warp = new PerlinNoise(8, 9f)
        {
            useFractal = true,
            octaves = 3
        };
        var mountain_macro = new PerlinNoise(2, 1.4f)
        {
            useFractal = true,
            octaves = 2
        };
        
        // island noise
        var island_macro = new PerlinNoise(2, 9f)
        {
            useFractal = true,
            octaves = 3
        };
        var island_coast = new PerlinNoise(7, 8f)
        {
            useFractal = true,
            octaves = 5
        };
        var island_warp = new PerlinNoise(9, 8f)
        {
            useFractal = true,
            octaves = 3
        };

        var mountainDetail = new PerlinNoise(8, 12f)
        {
            useFractal = true,
            octaves = 10
        };

        // ------------------------------------------------------- mountain generation

        // unmasked mountain range
        var mountainRange = new MountainNode(mountain_ridged, mountain_warp, mountain_macro)
        {
            warpStrength = 0.1f,
            height = 1f
        };

        // falloff mask with edge noise
        var falloff = new WarpedFalloffNode
        {
            radius = 0.4f,
            falloffPower = .8f,  
            edgeNoise = mountain_warp, // reuse low freq noise
            edgeStrength = .8f
        };

        // mask mountain with falloff
        var maskedMountains = new CombineNode(
            mountainRange,
            falloff,
            CombineMode.Multiply
        );

        var maskedDetail = new CombineNode(
        mountainDetail,
        falloff,
        CombineMode.Multiply
        );

        var detailedMountain = new CombineNode(
            maskedMountains,
            maskedDetail,
            CombineMode.Add
        );

        // warp edges slightly
        var warpedMountain = new SwirlNode(
            detailedMountain,
            .6f,
            1.5f
        );

        // move mountain to island edge
        var transformedMountain = new TransformNode(warpedMountain)
        {
            position = new Vector2(0f, 0f),
            scale = new Vector2(2, 2),
            heightScale = 1.5f,
        };

        // ------------------------------------------------------- island generation

        // generate island
        var island =
        new IslandNode(
            0.45f,
            .15f,
            island_macro,
            island_coast,
            island_warp
        );

        // blur island mask for slopes
        var blurredIsland = new BlurNode(island, blurShader, 512)
        {
            radius = 6
        };
        
        // ------------------------------------------------------- final output
        
        // combine island with mountain
        var mountainIsland = new CombineNode(blurredIsland, transformedMountain, CombineMode.Add);

        outputNode =
        new ErosionNode(
            mountainIsland,
            erosionShader,
            erosionResolution,
            erosionSettings
        );

        slopeMask = new MaskNode(outputNode, MaskNode.MaskType.Slope)
        {
            minSlope = 0f,
            maxSlope = .5f
        };

        heightMask = new MaskNode(outputNode, MaskNode.MaskType.Height)
        {
            minHeight = 0.15f,
            maxHeight = 0.75f
        };
        
        flowMask = new MaskNode(outputNode, MaskNode.MaskType.Flow)
        {
            minFlow = 0.1f,
            maxFlow = 0.5f
        };
        
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

        slopeMaskTex = NoiseTextureBuilder.Generate(slopeMask, resolution);
        heightMaskTex = NoiseTextureBuilder.Generate(heightMask, resolution);
        flowMaskTex = NoiseTextureBuilder.Generate(flowMask, erosionResolution);

        if (targetRenderer != null)
        {
            Material mat = targetRenderer.material;     // changed to material because sharedMaterial isnt suitable for viewport Shadergraph testing
            Texture2D sampleTexture = heightMaskTex;    // initialize preview noise texture
            //mat.SetTexture("_BaseColorMap", flowMaskTex);
            //mat.SetTexture("_UnlitColorMap", flowMaskTex);
            mat.SetTexture("_NoiseTexture", sampleTexture);
            Debug.Log(sampleTexture.width + " x " + sampleTexture.height);
        }
    }

    public Node_Blueprint GetOutputNode()
    {
        return outputNode;
    }
}