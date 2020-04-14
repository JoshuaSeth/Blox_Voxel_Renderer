using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class NormalsThreadCalculation : MonoBehaviour
{
    private void StartNatureNormalsCalcThread(int chunkX, int chunkZ, Mesh meshn)
    {
        CalculateNormalsThread normalsNTask = new CalculateNormalsThread();
        normalsNTask.vertices = meshn.vertices;
        normalsNTask.triangles = meshn.triangles;
        normalsNTask.chunkX = chunkX;
        normalsNTask.chunkZ = chunkZ;
        normalsNTask.isNature = true;
        ThreadPool.QueueUserWorkItem(normalsNTask.CalculateNormals);
    }

    private void StartNormalCalcThread(int chunkX, int chunkZ, Mesh mesh)
    {
        CalculateNormalsThread normalsTask = new CalculateNormalsThread();
        normalsTask.vertices = mesh.vertices;
        normalsTask.triangles = mesh.triangles;
        normalsTask.chunkX = chunkX;
        normalsTask.chunkZ = chunkZ;
        ThreadPool.QueueUserWorkItem(normalsTask.CalculateNormals);
    }
}
