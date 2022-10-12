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

    public Material material;

    ComputeBuffer collisionBuffer;
    private void Awake()
    {
        collisionBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Structured);
        collisionBuffer.SetData(new int[] { 0 });
        Graphics.SetRandomWriteTarget(1, collisionBuffer, true);
        material.SetBuffer("_collisionBuffer", collisionBuffer);
    }

    Vector3 lastMousePosition = new Vector3(float.NaN, float.NaN, float.NaN);
    int id = 0;

    private void Update()
    {
        int[] data = new int[1];
        collisionBuffer.GetData(data);
        collisionBuffer.SetData(new int[] { -1 });
        material.SetInteger("selectedIdentifier", data[0]);
        material.SetVector("mousePosition", Input.mousePosition);
        if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            if (Physics.Raycast(r, out var raycastHit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    vectorLine = GameObject.Instantiate(vectorLineBase.gameObject).GetComponent<VectorLine>();
                    vectorLine.SetId(id++);
                }
                vectorLine.AddPoint(raycastHit.point);
            }
        }
    }
}
}
