using UnityEngine;

public class TransformNode : Node_Blueprint
{
    public Node_Blueprint input;

    public Vector2 position = Vector2.zero;
    
    // domain (x,y)
    public Vector2 scale = Vector2.one;
    public float rotation = 0f;
    // range (height)
    public float heightScale = 1f;

    public Vector2 pivot = new Vector2(0.5f, 0.5f);

    public TransformNode(Node_Blueprint input)
    {
        this.input = input;
    }

    public override void Init()
    {
        input?.Init();
    }

    public override float Evaluate(float x, float y)
    {
        float px = x - pivot.x;
        float py = y - pivot.y;

        px /= scale.x;
        py /= scale.y;

        float rad = rotation * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float rx = px * cos - py * sin;
        float ry = px * sin + py * cos;

        float fx = rx + pivot.x + position.x;
        float fy = ry + pivot.y + position.y;

        float h = input.Evaluate(fx, fy);

        return h * heightScale;
    }
}