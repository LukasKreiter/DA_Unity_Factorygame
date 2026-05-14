using UnityEngine;

public class SwirlNode : Node_Blueprint
{
    public Node_Blueprint input;

    public Vector2 center = new Vector2(0.5f, 0.5f);

    public float radius = 0.4f;

    public float strength = 6f; // radians

    public bool smoothFalloff = true;

    public SwirlNode(
        Node_Blueprint input,
        float radius,
        float strength)
    {
        this.input = input;
        this.radius = radius;
        this.strength = strength;
    }

    public override void Init()
    {
        input?.Init();
    }

    public override float Evaluate(float x, float y)
    {
        float dx = x - center.x;
        float dy = y - center.y;

        float dist = Mathf.Sqrt(dx * dx + dy * dy);

        if (dist >= radius)
            return input.Evaluate(x, y);

        float t = dist / radius;   // 0 center -> 1 edge

        // outer weighted swirl
        float influence =
            t * t * (1f - t) * 4f;

        float angle = strength * influence;

        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        float rx = dx * cos - dy * sin;
        float ry = dx * sin + dy * cos;

        return input.Evaluate(
            center.x + rx,
            center.y + ry
        );
    }
}