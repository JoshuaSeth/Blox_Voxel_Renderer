using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtil
{
    public static Mesh TransferMeshData(MeshData chunkMeshData)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = chunkMeshData.vertices.ToArray();
        mesh.triangles = chunkMeshData.triangles.ToArray();
        mesh.uv = chunkMeshData.uv.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
}
