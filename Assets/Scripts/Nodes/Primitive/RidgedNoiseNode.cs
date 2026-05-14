using UnityEngine;

public class RidgedNoiseNode : Node_Blueprint
{
    int seed;
    float scale;

    public int octaves = 4;
    public float lacunarity = 2f;
    public float gain = 0.5f;

    public RidgedNoiseNode(int seed, float scale)
    {
        this.seed = seed;
        this.scale = scale;
    }

    public override float Evaluate(float x, float y)
    {
        float amplitude = 1f;
        float frequency = scale;

        float sum = 0f;
        float weight = 1f;

        for (int i = 0; i < octaves; i++)
        {
            float n = Mathf.PerlinNoise(
                x * frequency + seed,
                y * frequency + seed
            );

            n = 1f - Mathf.Abs(n * 2f - 1f);

            // sharpen ridges
            n *= n;

            // weighting 
            n *= weight;
            weight = Mathf.Clamp01(n * gain);

            sum += n * amplitude;

            amplitude *= 0.5f;
            frequency *= lacunarity;
        }

        return sum;
    }
}