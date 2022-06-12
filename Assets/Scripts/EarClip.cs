using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EarClip
{

    public enum WindingOrder
    {
        Invalid = 0,
        CounterClockwise = 1,
        Clockwise = 2,
    }

    public static float Cross(Vector3 a, Vector3 c)
    {
        return a.x * c.y - a.y * c.x;
    }

    public static T GetItem<T>(T[] array, int index)
    {
        if (index >= array.Length)
            return array[index % array.Length];
        else if (index < 0)
            return array[index % array.Length + array.Length];
        else
            return array[index];
    }

    public static T GetItem<T>(List<T> list, int index)
    {
        if (index >= list.Count)
            return list[index % list.Count];
        else if (index < 0)
            return list[index % list.Count + list.Count];
        else
            return list[index];
    }

    public static bool Triangulate(Vector3[] vertices, out int[] triangles, out string errorMessage)
    {
        triangles = null;
        errorMessage = string.Empty;
        if (vertices is null)
        {
            errorMessage = "The vertex list is null.";
            return false;
        }

        if (vertices.Length < 3)
        {
            errorMessage = "The vertex list must have at least 3 vertices.";
            return false;
        }

        if (vertices.Length > 1024)
        {
            errorMessage = "The max vertex list length is 1024";
            return false;
        }

        // if (!EarClip.IsSimplePolygon(vertices))
        // {
        //     errorMessage = "The vertex list does not defined a simple polygon.";
        //     return false;
        // }

        // if (EarClip.ContainsColinearEdges(vertices))
        // {
        //     errorMessage = "The vertex list contains colinear edges.";
        //     return false;
        // }

        EarClip.ComputePolygonArea(vertices, out float area, out WindingOrder windingOrder);

        if (windingOrder is WindingOrder.Invalid)
        {
            errorMessage = "The vertices list does not contain a valid polygon.";
            return false;
        }

        if (windingOrder is WindingOrder.CounterClockwise)
        {
            System.Array.Reverse(vertices);
        }

        List<int> indexList = new List<int>();
        for (int i = 0; i < vertices.Length; i++)
            indexList.Add(i);

        int totalTriangleCount = vertices.Length - 2;
        int totalTriangleIndexCount = totalTriangleCount * 3;

        triangles = new int[totalTriangleIndexCount];
        int triangleIndexCount = 0;

        int failsafe = 0;
        while (indexList.Count > 3)
        {
            failsafe++;
            if (failsafe > 500)
            {
                Debug.Log("You have triggered a failsafe. You likely have created an invalid simple polygon.");
                break;
            }
            for (int i = 0; i < indexList.Count; i++)
            {
                int a = indexList[i];
                int b = EarClip.GetItem(indexList, i - 1);
                int c = EarClip.GetItem(indexList, i + 1);

                Vector3 va = vertices[a];
                Vector3 vb = vertices[b];
                Vector3 vc = vertices[c];

                Vector3 va_to_vb = vb - va;
                Vector3 va_to_vc = vc - va;

                // if reflex
                if (EarClip.Cross(va_to_vb, va_to_vc) < 0f) continue;

                bool isEar = true;

                // check for other points in triangle
                for (int j = 0; j < vertices.Length; j++)
                {
                    // already a part of the triangle
                    if (j == a || j == b || j == c) continue;

                    Vector3 p = vertices[j];
                    if (EarClip.IsPointInTriangle(p, vb, va, vc))
                    {
                        isEar = false;
                        break;
                    }
                }

                if (isEar)
                {
                    triangles[triangleIndexCount++] = b;
                    triangles[triangleIndexCount++] = a;
                    triangles[triangleIndexCount++] = c;

                    indexList.RemoveAt(i);
                    break;
                }
            }
        }

        triangles[triangleIndexCount++] = indexList[0];
        triangles[triangleIndexCount++] = indexList[1];
        triangles[triangleIndexCount++] = indexList[2];

        return true;
    }

    public static bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 ab = b - a;
        Vector3 bc = c - b;
        Vector3 ca = a - c;

        Vector3 ap = p - a;
        Vector3 bp = p - b;
        Vector3 cp = p - c;

        float cross1 = EarClip.Cross(ab, ap);
        float cross2 = EarClip.Cross(bc, bp);
        float cross3 = EarClip.Cross(ca, cp);

        // if all are negative, it is within triangle
        return cross1 < 0f && cross2 < 0f && cross3 < 0f;
    }

    // public static bool IsSimplePolygon(Vector3[] vertices)
    // {
    //     throw new System.NotImplementedException();
    // }

    // public static bool ContainsColinearEdges(Vector3[] vertices)
    // {
    //     throw new System.NotImplementedException();
    // }

    public static void ComputePolygonArea(Vector3[] vertices, out float area, out WindingOrder windingOrder)
    {
        // for now, don't compute area
        area = -1;

        // windingOrder
        float sum = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v1 = vertices[i];
            Vector3 v2 = GetItem(vertices, i + 1);
            float edge = (v2.x - v1.x) * (v2.y + v1.y);
            sum += edge;
        }
        if (sum > 0)
            windingOrder = WindingOrder.Clockwise;
        else if (sum < 0)
            windingOrder = WindingOrder.CounterClockwise;
        else
            windingOrder = WindingOrder.Invalid;
    }

}
