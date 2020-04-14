using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CalculateNormalsThread
{
    public Vector3[] vertices;
    public int[] triangles;
    public bool finished = false;
    public Vector3[] normals;
    public int chunkX;
    public int chunkZ;
    public bool quitThread = false;
    public bool isNature = false;

    public void CalculateNormals(object token)
    {

        normals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
            if (quitThread)
            {
                finished = true;
                return;
            }
            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            normals[vertexIndexA] += triangleNormal;
            normals[vertexIndexB] += triangleNormal;
            normals[vertexIndexC] += triangleNormal;
        }


        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        finished = true;
    }

    Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 pointA = vertices[indexA];
        Vector3 pointB = vertices[indexB];
        Vector3 pointC = vertices[indexC];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }


}
