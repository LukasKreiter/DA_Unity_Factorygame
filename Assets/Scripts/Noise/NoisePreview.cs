using UnityEngine;

public class NoisePreview : MonoBehaviour
{
    public PerlinNoise node;              // your OOP node
    public Material previewMaterial;    // HDRP Unlit material
    public int resolution = 256;

    Texture2D tex;

    void Start()
    {
        Generate();
    }

    void OnValidate()
    {
        if (Application.isPlaying)
            Generate();
    }

    void Generate()
    {
        if (node == null || previewMaterial == null)
            return;

        node.Init();

        tex = new Texture2D(resolution, resolution);

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float u = (float)x / resolution;
                float v = (float)y / resolution;

                float n = node.Evaluate(u, v);

                // normalize (-1..1 → 0..1)
                n = (n + 1f) * 0.5f;

                tex.SetPixel(x, y, new Color(n, n, n));
            }
        }

        tex.Apply();

        previewMaterial.SetTexture("_BaseColorMap", tex);
    }
}