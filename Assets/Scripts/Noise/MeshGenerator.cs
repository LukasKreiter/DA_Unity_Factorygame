using UnityEngine;

/// <summary>
/// Builds a terrain-style mesh from the final output node of your noise graph.
/// Attach to a GameObject with MeshFilter + MeshRenderer.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteAlways]
public class NoiseMeshGenerator : MonoBehaviour
{
    [Header("Resolution")]
    [Range(2, 512)]
    public int resolution = 128;

    [Header("Size")]
    public float width = 10f;
    public float length = 10f;

    [Header("Height")]
    public float heightMultiplier = 3f;

    [Header("Preview")]
    public bool autoUpdate = true;

    [SerializeField] 
    private NoiseSystem noiseSystem;

    [SerializeField, HideInInspector]
    private Node_Blueprint outputNode;

    MeshFilter meshFilter;
    Mesh mesh;

    void OnEnable()
    {
        noiseSystem = GetComponent<NoiseSystem>();
        meshFilter = GetComponent<MeshFilter>();

        if (noiseSystem != null)
        {
            noiseSystem.OnGraphBuilt += HandleGraphBuilt;

            // if already built
            outputNode = noiseSystem.GetOutputNode();

            if (outputNode != null)
                Generate();
        }

        if (mesh == null) 
        { 
            mesh = new Mesh();
            mesh.name = "Noise Mesh"; 
        }

        meshFilter.sharedMesh = mesh;
    }

    void OnDisable()
    {
        if (noiseSystem != null)
            noiseSystem.OnGraphBuilt -= HandleGraphBuilt;
    }

    void OnValidate()
    {
        if (!autoUpdate) return;

        if (noiseSystem == null) return;

        outputNode = noiseSystem.GetOutputNode();

        if (outputNode == null) return;

        Generate();
    }

    public void Generate()
    {
        if (outputNode == null)
            return;

        if (resolution < 2)
            resolution = 2;

        outputNode.Init();

        BuildMesh();
    }

    void BuildMesh()
    {
        int vertCount = resolution * resolution;
        int quadCount = (resolution - 1) * (resolution - 1);
        int triIndexCount = quadCount * 6;

        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] triangles = new int[triIndexCount];

        float stepX = width / (resolution - 1);
        float stepZ = length / (resolution - 1);

        int v = 0;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float u = (float)x / (resolution - 1);
                float w = (float)z / (resolution - 1);

                float n = outputNode.Evaluate(u, w);

                float y = n * heightMultiplier;

                vertices[v] = new Vector3(
                    x * stepX - width * 0.5f,
                    y,
                    z * stepZ - length * 0.5f
                );

                uvs[v] = new Vector2(u, w);

                v++;
            }
        }

        int t = 0;

        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = z * resolution + x;

                triangles[t++] = i;
                triangles[t++] = i + resolution;
                triangles[t++] = i + 1;

                triangles[t++] = i + 1;
                triangles[t++] = i + resolution;
                triangles[t++] = i + resolution + 1;
            }
        }

        mesh.Clear();
        mesh.indexFormat = vertCount > 65000 ?
            UnityEngine.Rendering.IndexFormat.UInt32 :
            UnityEngine.Rendering.IndexFormat.UInt16;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        Debug.Log("Mesh built: " + vertices.Length + " vertices");
    }

    void HandleGraphBuilt(Node_Blueprint node)
    {
        Debug.Log("HandleGraphBuilt");
        outputNode = node;
        Generate();
    }
}