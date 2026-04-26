using UnityEngine;

public class ErosionNode : Node_Blueprint
{
    public Node_Blueprint input;

    public ComputeShader erosionShader;

    public ErosionSettings settings;

    public int bakeResolution = 256;

    float[] cachedMap;
    bool built;

    public ErosionNode(
        Node_Blueprint input,
        ComputeShader shader,
        int resolution,
        ErosionSettings settings)
    {
        this.input = input;
        this.erosionShader = shader;
        this.bakeResolution = resolution;
        this.settings = settings;
    }

    public override void Init()
    {
        if (input == null)
            return;

        input.Init();

        Rebuild();
    }

    public void Rebuild()
    {
        built = false;

        BuildCache();
    }

    void BuildCache()
    {
        if (built)
            return;

        float[] map =
            new float[bakeResolution * bakeResolution];

        for (int y = 0; y < bakeResolution; y++)
        {
            for (int x = 0; x < bakeResolution; x++)
            {
                float u =
                    (float)x / (bakeResolution - 1);

                float v =
                    (float)y / (bakeResolution - 1);

                map[y * bakeResolution + x] =
                    input.Evaluate(u, v);
            }
        }

        cachedMap = Erode.Run(
            erosionShader,
            map,
            bakeResolution,
            settings
        );

        built = true;
    }

    public override float Evaluate(float x, float y)
    {
        if (!built || cachedMap == null)
            return 0f;

        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        float px = x * (bakeResolution - 1);
        float py = y * (bakeResolution - 1);

        int ix = Mathf.FloorToInt(px);
        int iy = Mathf.FloorToInt(py);

        int ix1 = Mathf.Min(ix + 1, bakeResolution - 1);
        int iy1 = Mathf.Min(iy + 1, bakeResolution - 1);

        float tx = px - ix;
        float ty = py - iy;

        float a = cachedMap[iy * bakeResolution + ix];
        float b = cachedMap[iy * bakeResolution + ix1];
        float c = cachedMap[iy1 * bakeResolution + ix];
        float d = cachedMap[iy1 * bakeResolution + ix1];

        float ab = Mathf.Lerp(a, b, tx);
        float cd = Mathf.Lerp(c, d, tx);

        return Mathf.Lerp(ab, cd, ty);
    }
}