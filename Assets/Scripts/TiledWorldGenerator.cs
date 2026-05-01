using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseSystem))]
public class TiledWorldGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public int tilesX = 4;
    public int tilesY = 4;

    public int resolutionPerTile = 64;
    public float worldSize = 50f; // size of total terrain in world units

    [Header("Height Settings")]
    public float heightMultiplier = 3f;

    [Header("Rendering")]
    public Material material;

    private NoiseSystem noiseSystem;

    private TerrainTile[,] tiles;
    private List<GameObject> tileObjects = new List<GameObject>();

    void Awake()
    {
        noiseSystem = GetComponent<NoiseSystem>();
    }

    void Start()
    {
        Generate();
    }

    void OnValidate()
    {
        if (!isActiveAndEnabled) return;

        noiseSystem = GetComponent<NoiseSystem>();
    }

    public void Generate()
    {
        if (noiseSystem == null || noiseSystem.GetOutputNode() == null)
        {
            Debug.LogWarning("Noise graph not ready.");
            return;
        }

        ClearTiles();
        BuildTiles();
    }

    public void Regenerate()
    {
        Generate();
    }

    void BuildTiles()
    {
        tiles = new TerrainTile[tilesX, tilesY];

        // size per tile
        float tileWorldSize = worldSize / tilesX;

        // node instance for all tiles
        var node = noiseSystem.GetOutputNode();

        for (int ty = 0; ty < tilesY; ty++)
        {
            for (int tx = 0; tx < tilesX; tx++)
            {
                TerrainTile tile = BuildTile(tx, ty, tileWorldSize, node);
                tiles[tx, ty] = tile;

                GameObject go = new GameObject($"Tile_{tx}_{ty}");
                go.transform.parent = transform;

                // positions tiles according to local space
                go.transform.position = new Vector3(
                    tx * tileWorldSize,
                    0,
                    ty * tileWorldSize
                );

                var mf = go.AddComponent<MeshFilter>();
                var mr = go.AddComponent<MeshRenderer>();

                mf.sharedMesh = tile.mesh;
                mr.sharedMaterial = material;

                tileObjects.Add(go);
            }
        }
    }

    TerrainTile BuildTile(int tileX, int tileY, float tileWorldSize, Node_Blueprint node)
    {
        Debug.Log("initializing node: "+node);

        TerrainTile tile = new TerrainTile
        {
            x = tileX,
            y = tileY
        };

        int res = resolutionPerTile;

        Vector3[] vertices = new Vector3[res * res];
        Vector2[] uvs = new Vector2[res * res];
        int[] triangles = new int[(res - 1) * (res - 1) * 6];

        int vertIndex = 0;

        for (int yIndex = 0; yIndex < res; yIndex++)
        {
            for (int xIndex = 0; xIndex < res; xIndex++)
            {
                // normalize coords
                float localU = xIndex / (float)(res - 1);
                float localV = yIndex / (float)(res - 1);

                // Convert to global world position for noise sampling
                float worldX = (tileX * tileWorldSize) + (localU * tileWorldSize);
                float worldZ = (tileY * tileWorldSize) + (localV * tileWorldSize);

                // normalize to 0–1 range across entire world
                float u = worldX / worldSize;
                float vCoord = worldZ / worldSize;

                float height = node.Evaluate(u, vCoord);

                // mesh vertices are local (0 -> tileWorldSize) not worldX/worldZ, otherwise you'd offset them twice
                vertices[vertIndex] = new Vector3(
                    localU * tileWorldSize,
                    height * heightMultiplier,
                    localV * tileWorldSize
                );

                uvs[vertIndex] = new Vector2(u, vCoord);
                vertIndex++;
            }
        }

        // grid triangulation
        int triIndex = 0;

        for (int yIndex = 0; yIndex < res - 1; yIndex++)
        {
            for (int xIndex = 0; xIndex < res - 1; xIndex++)
            {
                int i = yIndex * res + xIndex;

                triangles[triIndex++] = i;
                triangles[triIndex++] = i + res;
                triangles[triIndex++] = i + 1;

                triangles[triIndex++] = i + 1;
                triangles[triIndex++] = i + res;
                triangles[triIndex++] = i + res + 1;
            }
        }

        Mesh mesh = new Mesh();

        // required because large tiles can exceed 65k vertices
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // lighting
        mesh.RecalculateBounds();  // culling

        tile.mesh = mesh;
        Debug.Log("returning tile: "+tile);
        return tile;
    }

    void ClearTiles()
    {
        for (int i = 0; i < tileObjects.Count; i++)
        {
            if (tileObjects[i] != null)
                Destroy(tileObjects[i]);
        }

        tileObjects.Clear();
        tiles = null;
    }
}