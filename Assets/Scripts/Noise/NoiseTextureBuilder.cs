using Unity.Mathematics;
using UnityEngine;

public static class NoiseTextureBuilder
{
    public static Texture2D Generate(Node_Blueprint node, int resolution)
    {
        Texture2D tex = new Texture2D(
            resolution,
            resolution,
            TextureFormat.RGBA32,
            false,
            true
        )
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float u = (float)x / (resolution - 1);
                float v = (float)y / (resolution - 1);

                float n = node.Evaluate(u, v);

                // normalize from [-1,1] to [0,1]
                n = (n + 1f) * 0.5f;
                n = Mathf.Clamp01(n);

                Color c = new Color(n, n, n, 1f);
                tex.SetPixel(x, y, c);
            }
        }

        tex.Apply();
        return tex;
    }
}