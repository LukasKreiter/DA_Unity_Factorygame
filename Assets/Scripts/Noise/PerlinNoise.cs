using Unity.VisualScripting;
using UnityEngine;

public class PerlinNoise : Noise_Blueprint
{
    public PerlinNoise(int seed, float frequency) : base(seed, frequency)
    {
    }

    public override void Init()
    {
        base.Init();
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        Debug.Log("initialized perlin");
    }

    public override float Evaluate(float x, float y)
    {
        float n = noise.GetNoise(x * frequency, y * frequency);
        return ApplyCommonTransforms(n);
    }
}