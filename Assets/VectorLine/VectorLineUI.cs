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
    int blacklistedId = -2;

    private void Update()
    {
        int[] data = new int[1];
        collisionBuffer.GetData(data);
        collisionBuffer.SetData(new int[] { -1 });
        material.SetVector("mousePosition", Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            blacklistedId = id;
            vectorLine = GameObject.Instantiate(vectorLineBase.gameObject).GetComponent<VectorLine>();
            vectorLine.SetId(id++);
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, lastMousePosition) > 4 )
        {
            lastMousePosition = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            if (Physics.Raycast(r, out var raycastHit))
            {
                vectorLine.AddPoint(raycastHit.point);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            blacklistedId = -2;
        }

        material.SetInteger("selectedIdentifier", blacklistedId == data[0] ? -1 : data[0]);
    }
}
}
