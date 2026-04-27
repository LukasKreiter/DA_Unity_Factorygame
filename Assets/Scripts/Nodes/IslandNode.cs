using UnityEngine;

public class IslandNode : Node_Blueprint
{
    public Vector2 center = new Vector2(0.5f, 0.5f);

    public float radius = 0.42f;

    public float macroStrength = 0.22f;
    public float coastStrength = 0.16f;

    public Node_Blueprint macroNoise;
    public Node_Blueprint coastNoise;
    public Node_Blueprint warpNoise;

    public float threshold = 0f;

    public IslandNode(
        float radius,
        Node_Blueprint macroNoise,
        Node_Blueprint coastNoise,
        Node_Blueprint warpNoise)
    {
        this.radius = radius;
        this.macroNoise = macroNoise;
        this.coastNoise = coastNoise;
        this.warpNoise = warpNoise;
    }

    public override void Init()
    {
        macroNoise?.Init();
        coastNoise?.Init();
        warpNoise?.Init();
    }

    public override float Evaluate(float x, float y)
    {
        float dx = x - center.x;
        float dy = y - center.y;

        float dist = Mathf.Sqrt(dx * dx + dy * dy);

        // Macro island shape
        float radial = 1f - (dist / radius);

        float macro = 0f;
        if (macroNoise != null)
            macro = macroNoise.Evaluate(x, y) * macroStrength;

        float field = radial + macro;

        // coastline band only
        float coastBand = 1f - Mathf.Abs(field) * 5f;
        coastBand = Mathf.Clamp01(coastBand);

        // warped path-like noise
        float wx = x;
        float wy = y;

        if (warpNoise != null)
        {
            float warp = warpNoise.Evaluate(x, y) * 0.06f;
            wx += warp;
            wy += warp;
        }

        float coast = 0f;

        if (coastNoise != null)
        {
            coast = coastNoise.Evaluate(wx, wy)
                    * coastStrength
                    * coastBand;
        }

        field += coast;

        return field > threshold ? 1f : 0f;
    }
}