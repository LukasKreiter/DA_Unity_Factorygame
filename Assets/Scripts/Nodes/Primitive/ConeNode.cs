using UnityEngine;

public class ConeNode : Node_Blueprint
{
    public float radius = 0.5f;

    // NEW: controls slope shape
    [Range(0.1f, 5f)]
    public float shape = 1f;

    // optional center offset if needed later
    public Vector2 center = new Vector2(0.5f, 0.5f);

    public ConeNode(float radius, float shape = 1f)
    {
        this.radius = radius;
        this.shape = shape;
    }

    public override float Evaluate(float x, float y)
    {
        // distance from center
        float dx = x - center.x;
        float dy = y - center.y;

        float dist = Mathf.Sqrt(dx * dx + dy * dy);

        // normalize distance to [0,1] based on radius
        float t = dist / radius;

        // outside radius → 0 (hard cutoff)
        if (t >= 1f)
            return 0f;

        // invert so center = 1, edge = 0
        float height = 1f - t;

        // apply shape curve
        height = Mathf.Pow(height, shape);

        return height;
    }
}