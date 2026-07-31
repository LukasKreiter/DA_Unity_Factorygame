using UnityEngine;

public static class Erode
{
    public static ErosionResult Run(
        ComputeShader shader,
        float[] sourceMap,
        int resolution,
        ErosionSettings settings)
    {
        if (shader == null)
        {
            Debug.LogError("Missing erosion compute shader.");
            return new ErosionResult(sourceMap, new float[sourceMap.Length]);
        }

        if (sourceMap == null || sourceMap.Length == 0)
        {
            return new ErosionResult(sourceMap, new float[0]);
        }

        int kernel = shader.FindKernel("CSMain");

        int paddedSize = resolution + settings.brushRadius * 2;
        int totalSize = paddedSize * paddedSize;

        float[] map = new float[totalSize];
        float[] flowMap = new float[totalSize];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int src = y * resolution + x;

                int dst =
                    (y + settings.brushRadius) * paddedSize +
                    (x + settings.brushRadius);

                map[dst] = sourceMap[src];
            }
        }

        var brush = BuildBrush(settings.brushRadius, paddedSize);

        int[] randomIndices = new int[settings.iterations];

        for (int i = 0; i < settings.iterations; i++)
        {
            int rx = Random.Range(
                settings.brushRadius,
                resolution + settings.brushRadius);

            int ry = Random.Range(
                settings.brushRadius,
                resolution + settings.brushRadius);

            randomIndices[i] = ry * paddedSize + rx;
        }

        ComputeBuffer mapBuffer = null;
        ComputeBuffer randomBuffer = null;
        ComputeBuffer brushIndexBuffer = null;
        ComputeBuffer brushWeightBuffer = null;
        ComputeBuffer flowBuffer = null;

        try
        {
            mapBuffer = new ComputeBuffer(map.Length, sizeof(float));
            randomBuffer = new ComputeBuffer(randomIndices.Length, sizeof(int));
            brushIndexBuffer = new ComputeBuffer(brush.indices.Length, sizeof(int));
            brushWeightBuffer = new ComputeBuffer(brush.weights.Length, sizeof(float));
            flowBuffer = new ComputeBuffer(flowMap.Length, sizeof(float));

            mapBuffer.SetData(map);
            randomBuffer.SetData(randomIndices);
            brushIndexBuffer.SetData(brush.indices);
            brushWeightBuffer.SetData(brush.weights);
            flowBuffer.SetData(flowMap);

            shader.SetBuffer(kernel, "map", mapBuffer);
            shader.SetBuffer(kernel, "randomIndices", randomBuffer);
            shader.SetBuffer(kernel, "brushIndices", brushIndexBuffer);
            shader.SetBuffer(kernel, "brushWeights", brushWeightBuffer);
            shader.SetBuffer(kernel, "flowMap", flowBuffer);

            shader.SetInt("mapSize", paddedSize);
            shader.SetInt("brushLength", brush.indices.Length);
            shader.SetInt("borderSize", settings.brushRadius);

            shader.SetInt("maxLifetime", settings.maxLifetime);

            shader.SetFloat("inertia", settings.inertia);
            shader.SetFloat("sedimentCapacityFactor", settings.sedimentCapacityFactor);
            shader.SetFloat("minSedimentCapacity", settings.minSedimentCapacity);
            shader.SetFloat("depositSpeed", settings.depositSpeed);
            shader.SetFloat("erodeSpeed", settings.erodeSpeed);
            shader.SetFloat("evaporateSpeed", settings.evaporateSpeed);
            shader.SetFloat("gravity", settings.gravity);
            shader.SetFloat("startSpeed", settings.startSpeed);
            shader.SetFloat("startWater", settings.startWater);
            shader.SetFloat("flowStrength", settings.flowStrength);
            shader.SetFloat("flowExponent", settings.flowExponent);

            int groups = Mathf.CeilToInt(settings.iterations / 1024f);

            shader.Dispatch(kernel, groups, 1, 1);

            mapBuffer.GetData(map);
            flowBuffer.GetData(flowMap);
        }
        finally
        {
            mapBuffer?.Release();
            randomBuffer?.Release();
            brushIndexBuffer?.Release();
            brushWeightBuffer?.Release();
            flowBuffer?.Release();
        }

        float maxFlow = 0f;

        for (int i = 0; i < flowMap.Length; i++)
        {
            if (flowMap[i] > maxFlow)
                maxFlow = flowMap[i];
        }

        if (maxFlow > 0f)
        {
            for (int i = 0; i < flowMap.Length; i++)
            {
                flowMap[i] /= maxFlow;
            }
        }

        float[] finalMap = new float[resolution * resolution];
        float[] finalFlow = new float[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int src =
                    (y + settings.brushRadius) * paddedSize +
                    (x + settings.brushRadius);

                int dst = y * resolution + x;

                finalMap[dst] = map[src];
                finalFlow[dst] = flowMap[src];
            }
        }

        return new ErosionResult(
            finalMap,
            finalFlow
        );
    }

    static BrushData BuildBrush(int radius, int mapSize)
    {
        System.Collections.Generic.List<int> ids =
            new System.Collections.Generic.List<int>();

        System.Collections.Generic.List<float> weights =
            new System.Collections.Generic.List<float>();

        float sum = 0f;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                float sqr = x * x + y * y;

                if (sqr < radius * radius)
                {
                    ids.Add(y * mapSize + x);

                    float w = 1f - Mathf.Sqrt(sqr) / radius;

                    sum += w;
                    weights.Add(w);
                }
            }
        }

        for (int i = 0; i < weights.Count; i++)
            weights[i] /= sum;

        return new BrushData
        {
            indices = ids.ToArray(),
            weights = weights.ToArray()
        };
    }

    struct BrushData
    {
        public int[] indices;
        public float[] weights;
    }
}