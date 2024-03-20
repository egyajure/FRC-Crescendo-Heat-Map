using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)

    {

        vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

        vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);

        vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);

        vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);



        Debug.Log(vertices[0]);

        Debug.Log(vertices[1]);

        Debug.Log(vertices[2]);

        Debug.Log(vertices[3]);



        uvs[(index * 4)] = Uv;

        uvs[(index * 4) + 1] = Uv;

        uvs[(index * 4) + 2] = Uv;

        uvs[(index * 4) + 3] = Uv;



        triangles[(index * 6) + 0] = (index * 4) + 0;

        triangles[(index * 6) + 1] = (index * 4) + 1;

        triangles[(index * 6) + 2] = (index * 4) + 2;

        triangles[(index * 6) + 3] = (index * 4) + 2;

        triangles[(index * 6) + 4] = (index * 4) + 3;

        triangles[(index * 6) + 5] = (index * 4) + 0;

    }



    private void CreateEmptyMeshData(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)

    {

        vertices = new Vector3[quadCount * 4];

        uvs = new Vector2[quadCount * 4];

        triangles = new int[quadCount * 6];

    }
}
