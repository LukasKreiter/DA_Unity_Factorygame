using UnityEngine;

public class ClampNode : UnaryNode
{
    public float min;
    public float max;

    public ClampNode(Node_Blueprint input, float min, float max)
        : base(input)
    {
        this.min = min;
        this.max = max;
    }

    public override float Evaluate(float x, float y)
    {
        return Mathf.Clamp(input.Evaluate(x, y), min, max);
    }
}