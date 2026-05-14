using UnityEngine;

public class WarpedFalloffNode : Node_Blueprint
{
    public Vector2 center = new Vector2(0.5f, 0.5f);
    public float radius = 0.3f;

    public float falloffPower = 2.5f; // controls slope shape

    public Node_Blueprint warp;       // low freq noise
    public float warpStrength = 0.15f;

    public Node_Blueprint edgeNoise;  // mid freq noise
    public float edgeStrength = 0.2f;

    public override void Init()
    {
        warp?.Init();
        edgeNoise?.Init();
    }

    public override float Evaluate(float x, float y)
    {
        // --- DOMAIN WARP (breaks circular symmetry)
        float wx = (warp?.Evaluate(x, y) ?? 0f) - 0.5f;
        float wy = (warp?.Evaluate(x + 37f, y + 91f) ?? 0f) - 0.5f;

        x += wx * warpStrength;
        y += wy * warpStrength;

        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        // --- DISTANCE FIELD
        float dx = x - center.x;
        float dy = y - center.y;
        float dist = Mathf.Sqrt(dx * dx + dy * dy);

        float baseMask = 1f - Mathf.Clamp01(dist / radius);

        // --- NON-LINEAR FALLOFF (this is critical)
        baseMask = Mathf.Pow(baseMask, falloffPower);

        // --- EDGE BREAKUP (prevents smooth perfect edge)
        float noise = edgeNoise?.Evaluate(x, y) ?? 0f;
        baseMask += (noise - 0.5f) * edgeStrength;

        return Mathf.Clamp01(baseMask);
    }
}