using UnityEngine;

public class NoiseGenerator
{
    private Noise_Blueprint node;

    public NoiseGenerator(int seed, float frequency)
    {
        node = new PerlinNoise(seed, frequency);
        node.Init();
    }

    public float Get(float x, float y)
    {
        return node.noise.GetNoise(x, y);
    }
}