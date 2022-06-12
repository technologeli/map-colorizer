using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    [SerializeField] private Vector3[] vertices;
    [SerializeField] private int[] triangles;
    private LineController line;

    // Start is called before the first frame update
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        line = GetComponent<LineController>();
    }

    public void UpdateShape()
    {
        vertices = line.GetVertices();
        bool t = EarClip.Triangulate(vertices, out int[] tris, out string errorMessage);
        triangles = tris;
        // if (!t)
        // {
        //     // TODO 06/09/22 - 11:48 AM : implement this
        //     Debug.Log(errorMessage);
        // }

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
