using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grid hits_grid;
    private Grid misses_grid;
    private Mesh mesh;

    public void SetGrid(Grid hits_grid, Grid misses_grid, string type)
    {
        this.hits_grid = hits_grid;
        this.misses_grid = misses_grid;
        if (type == "byPercentage")
        {
            UpdateHeatMapVisual();
        }
        else if (type == "byHits")
        {
            UpdateHeatMapVisual_Hits();
        }
        else if (type == "byMisses")
        {
            UpdateHeatMapVisual_Misses();
        }
    }

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void UpdateHeatMapVisual()
    {
        CreateEmptyMeshData(hits_grid.GetWidth() * hits_grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < hits_grid.GetWidth(); x++)
        {
            for (int y = 0; y < hits_grid.GetHeight(); y++)
            {
                int index = x * hits_grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(2, 2) * hits_grid.GetCellSize();
                int hitsValue = hits_grid.gridArray[x, y];
                int missesValue = misses_grid.gridArray[x, y];
                if (hits_grid.gridArray[x, y] != 0 || misses_grid.gridArray[x, y] != 0)
                {
                    float avgHits = (float)hitsValue / (hitsValue + missesValue);
                    // float gridValueNormalized = (float)avgHits * 100;
                    //if a spot hits every spot, avgHits will = 1,
                    Vector2 gridValueUV = new Vector2(avgHits, 0f);
                    // Debug.Log("hits value = " + hitsValue);
                    // Debug.Log("misses value = " + missesValue);
                    // Debug.Log("Avg value = " + avgHits);
                    // Debug.Log("Uv value = " + gridValueUV);
                    AddQuad(vertices, uv, triangles, index, hits_grid.GetWorldPosition(x, y) + quadSize * 0.5f, quadSize, gridValueUV);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void UpdateHeatMapVisual_Hits()
    {
        CreateEmptyMeshData(hits_grid.GetWidth() * hits_grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < hits_grid.GetWidth(); x++)
        {
            for (int y = 0; y < hits_grid.GetHeight(); y++)
            {
                int index = x * hits_grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(2, 2) * hits_grid.GetCellSize();
                int hitsValue = hits_grid.gridArray[x, y];
                if (hits_grid.gridArray[x, y] != 0)
                {
                    float avgHits = (float)(hitsValue - 1) / 4;
                    // float gridValueNormalized = (float)avgHits * 100;
                    //if a spot gets 5 hits, the number will be 5 and it will be green
                    Vector2 gridValueUV = new Vector2(avgHits, 0f);
                    // Debug.Log("hits value = " + hitsValue);
                    // Debug.Log("misses value = " + missesValue);
                    // Debug.Log("Avg value = " + avgHits);
                    // Debug.Log("Uv value = " + gridValueUV);
                    AddQuad(vertices, uv, triangles, index, hits_grid.GetWorldPosition(x, y) + quadSize * 0.5f, quadSize, gridValueUV);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }


    private void UpdateHeatMapVisual_Misses()
    {
        CreateEmptyMeshData(hits_grid.GetWidth() * hits_grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < hits_grid.GetWidth(); x++)
        {
            for (int y = 0; y < hits_grid.GetHeight(); y++)
            {
                int index = x * hits_grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(2, 2) * hits_grid.GetCellSize();
                int missesValue = misses_grid.gridArray[x, y];
                if (misses_grid.gridArray[x, y] != 0)
                {
                    float avgHits = (float)(missesValue - 1) / 4;
                    // float gridValueNormalized = (float)avgHits * 100;
                    //if a spot gets 5 misses, the number will be 5 and it will be green
                    Vector2 gridValueUV = new Vector2(avgHits, 0f);
                    // Debug.Log("hits value = " + hitsValue);
                    // Debug.Log("misses value = " + missesValue);
                    // Debug.Log("Avg value = " + avgHits);
                    // Debug.Log("Uv value = " + gridValueUV);
                    AddQuad(vertices, uv, triangles, index, hits_grid.GetWorldPosition(x, y) + quadSize * 0.5f, quadSize, gridValueUV);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
    {
        int vertexIndex = index * 4;
        int uvIndex = index * 4;
        int triangleIndex = index * 6;

        // Assign vertices
        vertices[vertexIndex] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
        vertices[vertexIndex + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[vertexIndex + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[vertexIndex + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

        // Debugging output for vertices
        // Debug.Log(vertices[vertexIndex]);
        // Debug.Log(vertices[vertexIndex + 1]);
        // Debug.Log(vertices[vertexIndex + 2]);
        // Debug.Log(vertices[vertexIndex + 3]);

        // Assign UVs
        uvs[uvIndex] = Uv;
        uvs[uvIndex + 1] = Uv;
        uvs[uvIndex + 2] = Uv;
        uvs[uvIndex + 3] = Uv;

        // Assign triangles
        triangles[triangleIndex] = vertexIndex;
        triangles[triangleIndex + 1] = vertexIndex + 1;
        triangles[triangleIndex + 2] = vertexIndex + 2;
        triangles[triangleIndex + 3] = vertexIndex + 2;
        triangles[triangleIndex + 4] = vertexIndex + 3;
        triangles[triangleIndex + 5] = vertexIndex;
    }




    private void CreateEmptyMeshData(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)

    {

        vertices = new Vector3[quadCount * 4];

        uvs = new Vector2[quadCount * 4];

        triangles = new int[quadCount * 6];

    }
}
