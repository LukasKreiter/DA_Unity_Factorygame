using UnityEngine;

public class ScaleNode : UnaryNode
{
    public float strength;

    public ScaleNode(Node_Blueprint input, float strength)
        : base(input)
    {
        this.strength = strength;
    }

    public override float Evaluate(float x, float y)
    {
        return input.Evaluate(x, y) * strength;
    }
}