using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Settings
{
    public static int chunkSize = 8, LODLevel = 1, finalizeChunksPerFrame = 32, startChunkThreadsPerFrame=32;
    public static float chunkFinalizationRefreshRate = 0, frustrumDataRefreshRate = 0.2f;
    public static bool frustrumUpdateOnlyWhenMoved = true, onlyRenderTopFaces = false;
}
