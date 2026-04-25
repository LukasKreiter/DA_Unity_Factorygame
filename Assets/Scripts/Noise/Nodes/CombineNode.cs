using UnityEngine;

public class CombineNode : BinaryNode
{
    public CombineMode mode;
    public float blend = 0.5f; // used for Blend mode

    public CombineNode(
        Node_Blueprint a,
        Node_Blueprint b,
        CombineMode mode,
        float blend = 0.5f
    ) : base(a, b)
    {
        this.mode = mode;
        this.blend = blend;
    }

    public override float Evaluate(float x, float y)
    {
        float A = inputA.Evaluate(x, y);
        float B = inputB.Evaluate(x, y);

        switch (mode)
        {
            case CombineMode.Add:
                return A + B;

            case CombineMode.Subtract:
                return A - B;

            case CombineMode.Multiply:
                return A * B;

            case CombineMode.Divide:
                return B == 0f ? 0f : A / B;

            case CombineMode.Max:
                return Mathf.Max(A, B);

            case CombineMode.Min:
                return Mathf.Min(A, B);

            case CombineMode.Blend:
                return Mathf.Lerp(A, B, blend);
        }

        return A;
    }
}