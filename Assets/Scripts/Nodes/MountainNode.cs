using UnityEngine;

public class MountainNode : Node_Blueprint
{
    Node_Blueprint ridged;
    Node_Blueprint warp;
    Node_Blueprint macro;

    public float warpStrength = 0.3f;
    public float height = 1f;

    public MountainNode(Node_Blueprint ridged, Node_Blueprint warp, Node_Blueprint macro)
    {
        this.ridged = ridged;
        this.warp = warp;
        this.macro = macro;
    }

    public override void Init()
    {
        ridged.Init();
        warp.Init();
        macro.Init();
    }

    public override float Evaluate(float x, float y)
    {
        // domain warp
        float wx =
            (warp.Evaluate(x, y) - 0.5f)
            * warpStrength;

        float wy =
            (warp.Evaluate(x + 31.7f, y + 91.3f) - 0.5f)
            * warpStrength;

        float nx = x + wx;
        float ny = y + wy;

        nx = Mathf.Clamp01(nx);
        ny = Mathf.Clamp01(ny);

        // ridged mountain structure
        float r = ridged.Evaluate(nx, ny);

        // macro mountain range mask
        float m = macro.Evaluate(x, y);

        // prevent negative values
        m = Mathf.Max(0f, m);

        m = Mathf.Pow(m, 1.5f);

        float mountain = r * m;

        // extra safety against invalid values
        if (float.IsNaN(mountain) || float.IsInfinity(mountain))
            mountain = 0f;

        return mountain * height;
    }

}