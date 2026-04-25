using Unity.VisualScripting;
using UnityEngine;

public class PerlinNoise : GeneratorNode
{
    public PerlinNoise(int seed, float frequency)
        : base(seed, frequency)
    {
    }

    public override void Init()
    {
        base.Init();
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
    }
}