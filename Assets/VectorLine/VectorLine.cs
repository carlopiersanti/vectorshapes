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
            new Vector2(1,0)
        };

        mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-1,0,0),
            new Vector3(1,0,0),
            new Vector3(1,0,0),
            new Vector3(-1,0,0),
        };
        /*
        mesh.SetUVs(0, new Vector3[] {
            new Vector3(0,-1,0),
            new Vector3(0,-1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0)
        });
        */
        mesh.SetUVs(0, new Vector3[] {
            new Vector3(1,0,0),
            new Vector3(1,0,0),
            new Vector3(-1,0,0),
            new Vector3(-1,0,0)
        });
        mesh.triangles = new int[] { 1, 0, 2, 3, 2, 0 };

        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        
    }
}
