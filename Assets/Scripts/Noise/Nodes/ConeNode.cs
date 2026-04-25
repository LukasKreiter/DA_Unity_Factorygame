using UnityEngine;

public class ConeNode : Node_Blueprint
{
    public Vector2 center = new Vector2(0.5f, 0.5f);
    public float radius = 0.35f;

    public ConeNode(float radius)
    {
        this.radius = radius;
    }

    public override float Evaluate(float x, float y)
    {
        float dx = x - center.x;
        float dy = y - center.y;

        float d = Mathf.Sqrt(dx * dx + dy * dy);

        float t = 1f - (d / radius);

        return Mathf.Clamp01(t);
    }
}