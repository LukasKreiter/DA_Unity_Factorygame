using UnityEngine;

public abstract class GeneratorNode : Node_Blueprint
{
    public int seed;
    public float frequency = 1f;

    public FastNoiseLite noise;

    // Fractal
    public bool useFractal = false;
    public FastNoiseLite.FractalType fractalType =
        FastNoiseLite.FractalType.FBm;

    public int octaves = 3;
    public float lacunarity = 2f;
    public float gain = 0.5f;

    // Warp
    public bool useWarp = false;
    public float warpAmplitude = 20f;

    protected GeneratorNode(int seed, float frequency)
    {
        this.seed = seed;
        this.frequency = frequency;
    }

    public override void Init()
    {
        noise = new FastNoiseLite();

        noise.SetSeed(seed);
        noise.SetFrequency(frequency);

        if (useFractal)
        {
            noise.SetFractalType(fractalType);
            noise.SetFractalOctaves(octaves);
            noise.SetFractalLacunarity(lacunarity);
            noise.SetFractalGain(gain);
        }

        if (useWarp)
        {
            noise.SetDomainWarpAmp(warpAmplitude);
        }
    }

    public override float Evaluate(float x, float y)
    {
        if (useWarp)
            noise.DomainWarp(ref x, ref y);

        return noise.GetNoise(x, y);
    }
}