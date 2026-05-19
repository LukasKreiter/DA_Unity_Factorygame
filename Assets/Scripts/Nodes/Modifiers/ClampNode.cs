using UnityEngine;

public class ClampNode : Node_Blueprint
{
    public Node_Blueprint input;

    public float min = 0f;
    public float max = 1f;

    public ClampNode(Node_Blueprint input)
    {
        this.input = input;
    }

    public override void Init()
    {
        input.Init();
    }

    public override float Evaluate(float x, float y)
    {
        return Mathf.Clamp(
            input.Evaluate(x, y),
            min,
            max
        );
    }
}