using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetItemMeshData
{
    public static MeshData MeshForObject(int x, int y, int z, MeshData meshData, Item item)
    {
        foreach (MeshEntry meshPrfb in item.meshParts)
        {
            meshData = BoxData(x, y, z, meshData, meshPrfb);
        }
        return meshData;
    }

    public static MeshData MeshForObject(int x, int y, int z, MeshData meshData, Item item, Cube.Type optionalFace)
    {
        foreach (MeshEntry meshPrfb in item.meshParts)
        {
            meshData = BoxData(x, y, z, meshData, meshPrfb, optionalFace);
        }
        return meshData;
    }

    public static MeshData BoxData(int x, int y, int z, MeshData meshData, MeshEntry meshPrfb)
    {
        float bottom = 0.5f;
        float yVals = meshPrfb.ySize - bottom;//
        float xVals = meshPrfb.xSize / 2f;
        float zVals = meshPrfb.zSize / 2f;

        //List<float> deformations = new List<float>(8);
        //for(int i = 0; i < meshPrfb.deformPointCount; i++)
        //{
        //    //set a random entry to random dformation
        //    SetRandomIndexValue(meshPrfb, deformations);
        //}

        //Trunk
        //North
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z + zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //East
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //South
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //West
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //Up
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        return meshData;
    }

    public static MeshData BoxData(int x, int y, int z, MeshData meshData, MeshEntry meshPrfb, Cube.Type optionalFace)
    {
        float bottom = 0.5f;
        float yVals = meshPrfb.ySize - bottom;//
        float xVals = meshPrfb.xSize / 2f;
        float zVals = meshPrfb.zSize / 2f;


        //Trunk
        //North
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z + zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        if (meshPrfb.face == Cube.Type.None)
            meshData.uv.AddRange(GetUVs.FaceUVs(optionalFace));
        else
            meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //East
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        if (meshPrfb.face == Cube.Type.None)
            meshData.uv.AddRange(GetUVs.FaceUVs(optionalFace));
        else
            meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //South
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        if (meshPrfb.face == Cube.Type.None)
            meshData.uv.AddRange(GetUVs.FaceUVs(optionalFace));
        else
            meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //West
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y - bottom + meshPrfb.yPos + meshPrfb.difLeftRightYBottom, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        if (meshPrfb.face == Cube.Type.None)
            meshData.uv.AddRange(GetUVs.FaceUVs(optionalFace));
        else
            meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));

        //Up
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z + zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x + xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos, z - zVals + meshPrfb.zPos));
        meshData.AddVertex(new Vector3(x - xVals + meshPrfb.xPos, y + yVals + meshPrfb.yPos + meshPrfb.difLeftRightYTop, z - zVals + meshPrfb.zPos));
        meshData.AddQuadTriangles();
        if (meshPrfb.face == Cube.Type.None)
            meshData.uv.AddRange(GetUVs.FaceUVs(optionalFace));
        else
            meshData.uv.AddRange(GetUVs.FaceUVs(meshPrfb.face));
        return meshData;
    }
}
