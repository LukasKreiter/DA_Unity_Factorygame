using Unity.Mathematics;
using UnityEngine;

public static class NoiseTextureBuilder
{
    public static Texture2D Generate(Noise_Blueprint noise, int resolution)
    {
        Texture2D tex = new Texture2D(resolution, resolution);
        noise.Init();
        Debug.Log("generating texture");

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float u = (float)x / resolution;
                float v = (float)y / resolution;

                float n = noise.Evaluate(u, v);
                n = (n + 1f) * 0.5f; // normalize

                tex.SetPixel(x, y, new Color(n, n, n));
            }
        }

        Debug.Log("texture finished generating");
        tex.Apply();
        return tex;
    }
}