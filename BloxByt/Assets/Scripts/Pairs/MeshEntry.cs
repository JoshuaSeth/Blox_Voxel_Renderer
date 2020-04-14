using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshEntry
{
    public enum Type { Box, Pyramid, ReverseParamid};
    public Type type;

    public float ySize;
    public float xSize;
    public float zSize;

    public float difLeftRightYBottom;
    public float difLeftRightYTop;

    public float xPos;
    public float yPos;
    public float zPos;

    public float deformAmount;
    public float deformPointCount;

    public Cube.Type face;

    public enum UVPart { Whole, Half, Quarter, Eight};
    public UVPart uvPart;
}
