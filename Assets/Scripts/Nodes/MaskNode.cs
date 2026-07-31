using UnityEngine;

public class MaskNode : Node_Blueprint
{
    public Node_Blueprint input;

    public MaskNode (Node_Blueprint input, MaskType type)
    {
        this.input = input;
        this.type = type;
    }

    public enum MaskType
    {
        Height,
        Slope,
        Noise,
        Flow
    }

    public MaskType type;

    float epsilon = 1f / 256f;

    [Header("Height Settings")]
    public float minHeight = 0.2f;
    public float maxHeight = 0.8f;

    [Header("Slope Settings")]
    public float minSlope = 0f;
    public float maxSlope = 45f;

    [Header("Noise Settings")]
    public Node_Blueprint noise;
    public float noiseStrength = 1;

    [Header("Flow Settings")]
    public float minFlow = 0f;
    public float maxFlow = 1f;

    public override void Init()
    {
        input?.Init();
        noise?.Init();
    }

    public override float Evaluate(float x, float y)
    {
        switch(type)
        {
            case MaskType.Height:
                return HeightMask(x,y);

            case MaskType.Slope:
                return SlopeMask(x,y);

            case MaskType.Noise:
                return NoiseMask(x,y);
            case MaskType.Flow:
                return FlowMask(x,y);
        }

        return 0;
    }

    float HeightMask(float x,float y)
    {
        float h = input.Evaluate(x,y);

        return Mathf.InverseLerp(
            minHeight,
            maxHeight,
            h
        );
    }

    float SlopeMask(float x,float y)
    {
        float dx = input.Evaluate(x+epsilon,y) - input.Evaluate(x-epsilon,y);
        float dy = input.Evaluate(x,y+epsilon) - input.Evaluate(x,y-epsilon);

        float slope = Mathf.Sqrt(dx*dx + dy*dy);

        return Mathf.InverseLerp(
            minSlope,
            maxSlope,
            slope
        );
    }

    float NoiseMask(float x,float y)
    {
        return noise.Evaluate(x,y) * noiseStrength;
    }

    float FlowMask(float x,float y)
    {
        ErosionNode erosion = input as ErosionNode;

        if (erosion == null)
        {
            Debug.LogWarning(
                "Flow mask requires an ErosionNode input."
            );

            return 0;
        }

        float flow = erosion.EvaluateFlow(x,y);

        return Mathf.InverseLerp(
            minFlow,
            maxFlow,
            flow
        );
    }
}