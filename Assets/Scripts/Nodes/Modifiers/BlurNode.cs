using UnityEngine;

public class BlurNode : Node_Blueprint
{
    public Node_Blueprint input;

    public int resolution = 256;
    public int radius = 3;

    public ComputeShader blurShader;

    private float[] cachedMap;

    public BlurNode(Node_Blueprint input, ComputeShader shader, int resolution = 256)
    {
        this.input = input;
        this.blurShader = shader;
        this.resolution = resolution;
    }

    public override void Init()
    {
        input.Init();

        Bake();
    }

    void Bake()
    {
        float[] map = new float[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float u = (float)x / (resolution - 1);
                float v = (float)y / (resolution - 1);

                map[y * resolution + x] = input.Evaluate(u, v);
            }
        }

        cachedMap = BlurCompute.Blur(map, resolution, radius, blurShader);
    }

    public override float Evaluate(float x, float y)
    {

        float fx = x * (resolution - 1);
        float fy = y * (resolution - 1);

        int x0 = Mathf.Clamp((int)fx, 0, resolution - 1);
        int y0 = Mathf.Clamp((int)fy, 0, resolution - 1);

        int x1 = Mathf.Min(x0 + 1, resolution - 1);
        int y1 = Mathf.Min(y0 + 1, resolution - 1);

        float tx = fx - x0;
        float ty = fy - y0;

        float a = cachedMap[y0 * resolution + x0];
        float b = cachedMap[y0 * resolution + x1];
        float c = cachedMap[y1 * resolution + x0];
        float d = cachedMap[y1 * resolution + x1];

        return Mathf.Lerp(
            Mathf.Lerp(a, b, tx),
            Mathf.Lerp(c, d, tx),
            ty
        );
    }
}