using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Test
{

public class VectorLineUI : MonoBehaviour
{
    [SerializeField]
    private VectorLine vectorLineBase;

    private VectorLine vectorLine;

    public TMP_Text textMesh;

    public Material material;

    ComputeBuffer collisionBuffer;
    private void Awake()
    {
        collisionBuffer = new ComputeBuffer(1, sizeof(uint), ComputeBufferType.Structured);
        collisionBuffer.SetData(new uint[] { 0 });
        Graphics.SetRandomWriteTarget(1, collisionBuffer, true);
        material.SetBuffer("_collisionBuffer", collisionBuffer);

        vectorLine = GameObject.Instantiate(vectorLineBase.gameObject).GetComponent<VectorLine>();
    }

    /*private void CreateMesh(List<Vector2> linepoints)
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
            mesh.bounds.Encapsulate(linepoints[i]);
        }
        mesh.bounds.Encapsulate(linepoints[linepoints.Count - 1]);

        mesh.vertices = vertices;

        Vector3[] uvs = new Vector3[5 * (linepoints.Count - 1)];
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            uvs[5 * i] = (linepoints[i + 1] - linepoints[i]);
            uvs[5 * i + 1] = (linepoints[i + 1] - linepoints[i]);
            uvs[5 * i + 2] = (linepoints[i] - linepoints[i + 1]);
            uvs[5 * i + 3] = (linepoints[i] - linepoints[i + 1]);
            uvs[5 * i + 4] = Vector3.zero;
        }


        mesh.SetUVs(0, uvs);

        int[] triangles = new int[12 * (linepoints.Count - 1)];
        for (int i = 0; i < linepoints.Count - 1; i++)
        {
            triangles[i * 12] = i * 5 + 1;
            triangles[i * 12 + 1] = i * 5;
            triangles[i * 12 + 2] = i * 5 + 2;
            triangles[i * 12 + 3] = i * 5 + 3;
            triangles[i * 12 + 4] = i * 5 + 2;
            triangles[i * 12 + 5] = i * 5;
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

        mesh.SetTriangles(triangles,0,false);

        GetComponent<MeshFilter>().mesh = mesh;
    }*/

    Vector3 lastMousePosition = new Vector3(float.NaN, float.NaN, float.NaN);

    private void Update()
    {
        uint[] data = new uint[1];
        collisionBuffer.GetData(data);
        textMesh.text = data[0].ToString();
        collisionBuffer.SetData(new uint[] { 0 });
        if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            if (Physics.Raycast(r, out var raycastHit))
            {
                vectorLine.AddPoint(raycastHit.point);
            }
        }
    }
}
}
