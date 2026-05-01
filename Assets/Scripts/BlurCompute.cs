using UnityEngine;

public static class BlurCompute
{
    public static float[] Blur(
        float[] input,
        int resolution,
        int radius,
        ComputeShader shader)
    {
        int kernel = shader.FindKernel("CSMain");

        ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, sizeof(float));
        ComputeBuffer resultBuffer = new ComputeBuffer(input.Length, sizeof(float));

        inputBuffer.SetData(input);

        shader.SetBuffer(kernel, "input", inputBuffer);
        shader.SetBuffer(kernel, "result", resultBuffer);
        shader.SetInt("resolution", resolution);
        shader.SetInt("radius", radius);

        int groups = Mathf.CeilToInt(resolution / 8f);

        shader.Dispatch(kernel, groups, groups, 1);

        float[] result = new float[input.Length];
        resultBuffer.GetData(result);

        inputBuffer.Release();
        resultBuffer.Release();

        return result;
    }
}