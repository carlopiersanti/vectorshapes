using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLine : MonoBehaviour
{
    public Material material;

    public Mesh mesh;

    List<Vector2> linepoints = new List<Vector2>();

    private void Awake()
    {
        mesh = new Mesh();
        CreateMesh(linepoints);
    }

    private void CreateMesh(List<Vector2> linepoints)
    {
        if (linepoints.Count < 2)
            return;

        Vector3[] vertices = new Vector3[5 * (linepoints.Count - 1)];

        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            vertices[5 * i] = new Vector3(linepoints[i].x, linepoints[i].y, 0);
            vertices[5 * i + 1] = new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0);
            vertices[5 * i + 2] = new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0);
            vertices[5 * i + 3] = new Vector3(linepoints[i].x, linepoints[i].y, 0);
            vertices[5 * i + 4] = new Vector3(linepoints[i + 1].x, linepoints[i + 1].y, 0);
        }

        mesh.vertices = vertices;

        Vector3[] uvs = new Vector3[5 * (linepoints.Count - 1)];
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            uvs[5 * i] = (linepoints[i + 1] - linepoints[i]).normalized;
            uvs[5 * i + 1] = (linepoints[i + 1] - linepoints[i]).normalized;
            uvs[5 * i + 2] = (linepoints[i] - linepoints[i + 1]).normalized;
            uvs[5 * i + 3] = (linepoints[i] - linepoints[i + 1]).normalized;
            uvs[5 * i + 4] = Vector3.zero;
        }


        mesh.SetUVs(0, uvs);

        int[] triangles = new int[9 * (linepoints.Count - 1)];
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            triangles[i * 9] = i * 5 + 1;
            triangles[i * 9 + 1] = i * 5;
            triangles[i * 9 + 2] = i * 5 + 2;
            triangles[i * 9 + 3] = i * 5 + 3;
            triangles[i * 9 + 4] = i * 5 + 2;
            triangles[i * 9 + 5] = i * 5;
        }

        for (int i = 1; i < linepoints.Count - 1; i++)
        {
            if (Vector2.SignedAngle(linepoints[i - 1] - linepoints[i], linepoints[i + 1] - linepoints[i]) < 0)
            {
                triangles[(i - 1) * 9 + 6] = (i - 1) * 5 + 4;
                triangles[(i - 1) * 9 + 7] = (i - 1) * 5 + 5;
                triangles[(i - 1) * 9 + 8] = (i - 1) * 5 + 1;
            }
            else
            {
                triangles[(i - 1) * 9 + 6] = (i - 1) * 5 + 4;
                triangles[(i - 1) * 9 + 7] = (i - 1) * 5 + 2;
                triangles[(i - 1) * 9 + 8] = (i - 1) * 5 + 8;
            }
        }

        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;

    }

    Vector3 lastMousePosition = new Vector3(float.NaN, float.NaN, float.NaN);

    private void Update()
    {
        if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            if (Physics.Raycast(r, out var raycastHit))
            {
                Vector3 hitpoint = transform.InverseTransformPoint(raycastHit.point);
                linepoints.Add(hitpoint);
                CreateMesh(linepoints);
            }
        }
    }
}
