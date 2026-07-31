using UnityEngine;

public class ErosionResult
{
    public float[] heightMap;
    public float[] flowMap;

    public ErosionResult(float[] heightMap, float[] flowMap)
    {
        this.heightMap = heightMap;
        this.flowMap = flowMap;
    }
}
