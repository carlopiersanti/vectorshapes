using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLine : MonoBehaviour
{
    public Material material;

    public Mesh mesh;

    private void Awake()
    {
        Vector2[] linepoints = new Vector2[]
        {
            new Vector2(-1,0),
            new Vector2(1,0),
            new Vector2(3,3)
        };

        mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * (linepoints.Length - 1)];

        for ( int i = 0; i< linepoints.Length -1; i++)
        {
            vertices[4 * i] = new Vector3(linepoints[i].x, linepoints[i].y, 0);
            vertices[4 * i + 1] = new Vector3(linepoints[i+1].x, linepoints[i + 1].y, 0);
            vertices[4 * i + 2] = new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0);
            vertices[4 * i + 3] = new Vector3(linepoints[i].x, linepoints[i].y, 0);

        }

        mesh.vertices = vertices;

        Vector3 [] uvs = new Vector3[4 * (linepoints.Length - 1)];
        for (int i = 0; i < linepoints.Length - 1; i++)
        {
            uvs[4 * i] = linepoints[i+1] - linepoints[i];
            uvs[4 * i + 1] = linepoints[i + 1] - linepoints[i];
            uvs[4 * i + 2] = linepoints[i] - linepoints[i+1];
            uvs[4 * i + 3] = linepoints[i] - linepoints[i + 1];
        }

        
        mesh.SetUVs(0, uvs);

        int[] triangles = new int[6 * (linepoints.Length-1)];
        for (int i=0; i< linepoints.Length - 1; i++)
        {
            triangles[i * 6 ] = i * 4 + 1;
            triangles[i * 6 + 1] = i * 4;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4 + 3;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4;
        }


        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        
    }
}
