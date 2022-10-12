using System;
using System.Collections.Generic;
using UnityEngine;

public class VectorLine : MonoBehaviour
{
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> vertices;
    private List<Vector3> uvs;
    private List<int> triangles;
    private Mesh mesh;
    MaterialPropertyBlock materialPropertyBlock;

    [SerializeField]
    private Material material;

    public void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetInt("identifier", -1);
    }

    public void SetId(int id)
    {
        materialPropertyBlock.SetInt("identifier", id);
        GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
    }

    public void SetPoints(List<Vector3> newPoints)
    {
        if (newPoints.Count <= 1)
            throw new Exception("Vector Line must contain at least 2 points");
        points = newPoints;
        CreateMesh(points);
    }

    public void AddPoint(Vector3 newPoint)
    {
        points.Add(newPoint);

        if (points.Count < 2)
            return;
        else if (points.Count == 2)
            CreateMesh(points);
        else
        {
            vertices.Add(points[points.Count - 2]);
            vertices.Add(newPoint);
            vertices.Add(newPoint);
            vertices.Add(points[points.Count - 2]);
            vertices.Add(newPoint);
            mesh.bounds.Encapsulate(newPoint);
            mesh.SetVertices(vertices);

            uvs.Add(newPoint - points[points.Count - 2]);
            uvs.Add(newPoint - points[points.Count - 2]);
            uvs.Add(points[points.Count - 2] - newPoint);
            uvs.Add(points[points.Count - 2] - newPoint);
            uvs.Add(Vector3.zero);

            mesh.SetUVs(0, uvs);

            triangles.Add((points.Count - 3) * 5 + 4);
            triangles.Add((points.Count - 3) * 5 + 5);
            triangles.Add((points.Count - 3) * 5 + 1);
            triangles.Add((points.Count - 3) * 5 + 4);
            triangles.Add((points.Count - 3) * 5 + 2);
            triangles.Add((points.Count - 3) * 5 + 8);

            triangles.Add((points.Count - 2) * 5 + 1);
            triangles.Add((points.Count - 2) * 5);
            triangles.Add((points.Count - 2) * 5 + 2);
            triangles.Add((points.Count - 2) * 5 + 3);
            triangles.Add((points.Count - 2) * 5 + 2);
            triangles.Add((points.Count - 2) * 5);

            mesh.SetTriangles(triangles, 0, false);

            mesh.bounds.Encapsulate(newPoint);
        }
    }

    private void CreateMesh(List<Vector3> newPoints)
    {
        mesh = new Mesh();

        vertices = new List<Vector3>(5 * (newPoints.Count - 1));

        for (int i = 0; i < newPoints.Count - 1; i++)
        {
            vertices.Add(newPoints[i]);
            vertices.Add(newPoints[i + 1]);
            vertices.Add(newPoints[i + 1]);
            vertices.Add(newPoints[i]);
            vertices.Add(newPoints[i + 1]);
        }

        mesh.SetVertices(vertices);

        uvs = new List<Vector3>(5 * (newPoints.Count - 1));
        for (int i = 0; i < newPoints.Count - 1; i++)
        {
            uvs.Add(newPoints[i + 1] - newPoints[i]);
            uvs.Add(newPoints[i + 1] - newPoints[i]);
            uvs.Add(newPoints[i] - newPoints[i + 1]);
            uvs.Add(newPoints[i] - newPoints[i + 1]);
            uvs.Add(Vector3.zero);
        }


        mesh.SetUVs(0, uvs);

        triangles = new List<int>(12 * (newPoints.Count - 1) - 6);
        for (int i = 0; i < newPoints.Count - 1; i++)
        {
            triangles.Add(i * 5 + 1);
            triangles.Add(i * 5);
            triangles.Add(i * 5 + 2);
            triangles.Add(i * 5 + 3);
            triangles.Add(i * 5 + 2);
            triangles.Add(i * 5);
            if ( i < newPoints.Count - 2)
            {
                triangles.Add(i * 5 + 4);
                triangles.Add(i * 5 + 5);
                triangles.Add(i * 5 + 1);
                triangles.Add(i * 5 + 4);
                triangles.Add(i * 5 + 2);
                triangles.Add(i * 5 + 8);
            }
        }

        mesh.SetTriangles(triangles, 0, true);

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
