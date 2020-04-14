using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionInfo
{
    public RenderThreadTask thread;
    public GameObject chunk;
    public int LODLevel;
    public int chunkSize;
    public Vector2Int key;
}
