using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLine : MonoBehaviour
{
    private List<Vector3> vertices;
    private List<Vector3> uvs;
    private List<int> triangles;

    [SerializeField]
    private Material material;

    [SerializeField]
    private Mesh mesh;

    public void SetPoints(List<Vector3> newvertices)
    {

    }

    public void AddPoint(Vector3 newVertex)
    {

    }

    private void CreateMesh(List<Vector2> linepoints)
    {
        if (linepoints.Count < 2)
            return;

        vertices = new List<Vector3>(5 * (linepoints.Count - 1));

        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            vertices.Add(new Vector3(linepoints[i].x, linepoints[i].y, 0) );
            vertices.Add(new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0));
            vertices.Add(new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0));
            vertices.Add(new Vector3(linepoints[i].x, linepoints[i].y, 0));
            vertices.Add(new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0));
            mesh.bounds.Encapsulate(linepoints[i]);
        }
        mesh.bounds.Encapsulate(linepoints[linepoints.Count - 1]);

        mesh.SetVertices(vertices);

        uvs = new List<Vector3>(5 * (linepoints.Count - 1));
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            uvs.Add(linepoints[i + 1] - linepoints[i]);
            uvs.Add(linepoints[i + 1] - linepoints[i]);
            uvs.Add(linepoints[i] - linepoints[i + 1]);
            uvs.Add(linepoints[i] - linepoints[i + 1]);
            uvs.Add(Vector3.zero);
        }


        mesh.SetUVs(0, uvs);

        triangles = new List<int>(12 * (linepoints.Count - 1));
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            triangles.Add(i * 5 + 1);
            triangles.Add(i * 5);
            triangles.Add(i * 5 + 2);
            triangles.Add(i * 5 + 3);
            triangles.Add(i * 5 + 2);
            triangles.Add(i * 5);
        }

        for (int i = 1; i < linepoints.Count - 1; i++)
        {
            //if (Vector2.SignedAngle(linepoints[i - 1] - linepoints[i], linepoints[i + 1] - linepoints[i]) < 0)
            {
                triangles[(i - 1) * 12 + 6] = (i - 1) * 5 + 4;
                triangles[(i - 1) * 12 + 7] = (i - 1) * 5 + 5;
                triangles[(i - 1) * 12 + 8] = (i - 1) * 5 + 1;
            }
            //else
            {
                triangles[(i - 1) * 12 + 9] = (i - 1) * 5 + 4;
                triangles[(i - 1) * 12 + 10] = (i - 1) * 5 + 2;
                triangles[(i - 1) * 12 + 11] = (i - 1) * 5 + 8;
            }
        }

        mesh.SetTriangles(triangles, 0, false);

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
