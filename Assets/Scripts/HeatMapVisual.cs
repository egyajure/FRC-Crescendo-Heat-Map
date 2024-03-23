using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
        UpdateHeatMapVisual();
    }

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void UpdateHeatMapVisual()
    {
        Debug.Log("Update mesh test");
        CreateEmptyMeshData(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        Debug.Log("uv " + uv.Length);
        Debug.Log("vertices " + vertices.Length);
        Debug.Log("traingles " + triangles.Length);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(2, 2) * grid.GetCellSize();
                int gridValue = grid.GetValue(x, y);
                float gridValueNormalized = (float)gridValue / 10;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0);
                if (grid.gridArray[x, y] != 0)
                {
                    AddQuad(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * 0.5f, quadSize, gridValueUV);
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
