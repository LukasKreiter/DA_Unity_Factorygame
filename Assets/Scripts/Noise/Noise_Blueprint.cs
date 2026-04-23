using UnityEngine;

public abstract class Noise_Blueprint
{
    public int seed;
    public float frequency = 1f;

    public FastNoiseLite noise;

    public Noise_Blueprint (int seed, float frequency)
    {
        this.seed = seed;
        this.frequency = frequency;
    }

    public virtual void Init()
    {
        noise = new FastNoiseLite();
        noise.SetSeed(seed);
    }

    public abstract float Evaluate(float x, float y);

    protected float ApplyCommonTransforms(float n)
    {
        return n; // later: clamp, bias, gain, etc.
    }
}