using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Input the current meshdata and block and get the meshdata of the block added to it back.
/// </summary>
[Serializable]
public static class GetMeshData
{
    public static float Steepness(int x, int y, int z, Biome biome, int[,] heightGrid, int realX, int realY, int chunkSize)
    {
        if (heightGrid == null)
        {
            float step1 = (Chance.GetHeight(x, z + 1, biome, heightGrid) + 0.0001f) / (Chance.GetHeight(x, z - 1, biome, heightGrid) + 0.0001f);
            float step2 = (Chance.GetHeight(x + 1, z, biome, heightGrid) + 0.0001f) / (Chance.GetHeight(x - 1, z, biome, heightGrid) + 0.0001f);
            return Mathf.Abs(((step1 + step2) / 2f) - 1);
        }
        if (heightGrid != null)
        {
            float step1 = (heightGrid[realX, Math.Min(realY+1, (chunkSize-1))] + 0.0001f) / (heightGrid[realX, Math.Max(realY - 1, 0)] + 0.0001f);
            float step2 = (heightGrid[Math.Min(realX, (chunkSize-1)), realY] + 0.0001f) / (heightGrid[Math.Max(realX - 1, 0), realY] + 0.0001f);
            return Mathf.Abs(((step1 + step2) / 2f) - 1);
        }
        return -99;
    }

    public static int RoundToLodFrequency(int i, int LODLevel)
    {
        return Mathf.FloorToInt(i / LODLevel) * LODLevel;
    }

    public static MeshData Blockdata
     (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, bool isTop, int realX, int realZ, int LODLevel, int chunkSize)
    {
        y = y - LODLevel;
        bool slopedTop = false;

        float steepness = Steepness(x, y, z, biome, heightGrid,realX, realZ, chunkSize);


        //De blok wordt gerenderd vanwege de hoogte. Daarom altijd facedata up.
        if (isTop)
        {
            meshData = UpFace(x, y, z, meshData, biome, heightGrid, out slopedTop, steepness, LODLevel);
        }

        if (!Settings.onlyRenderTopFaces)
        {
            if (heightGrid != null)
            {
                meshData = FacesHeightGridEdge(x, y, z, meshData, biome, heightGrid, realX, realZ, LODLevel, chunkSize, steepness);
                meshData = FacesHeightGridMiddle(x, y, z, meshData, biome, heightGrid, realX, realZ, LODLevel, chunkSize, steepness);
            }



            if (heightGrid == null)
            {
                int hieghtOtherSpot = Chance.GetHeight(x, z + LODLevel, biome, heightGrid);
                if (hieghtOtherSpot - LODLevel < y + LODLevel)
                    for (int i = hieghtOtherSpot - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = NorthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                hieghtOtherSpot = Chance.GetHeight(x, z - LODLevel, biome, heightGrid);
                if (hieghtOtherSpot - LODLevel < y + LODLevel)
                    for (int i = hieghtOtherSpot - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = SouthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                hieghtOtherSpot = Chance.GetHeight(x + LODLevel, z, biome, heightGrid);
                if (hieghtOtherSpot - LODLevel < y + LODLevel)
                    for (int i = hieghtOtherSpot - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = EastFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                hieghtOtherSpot = Chance.GetHeight(x - LODLevel, z, biome, heightGrid);
                if (hieghtOtherSpot - LODLevel < y + LODLevel)
                    for (int i = hieghtOtherSpot - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = WestFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);



                if (realZ + LODLevel - 1 == (chunkSize - 1) && realZ != 0)
                    for (int i = y - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = NorthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                if (realX + LODLevel - 1 == (chunkSize - 1) && realX != 0)
                    for (int i = y - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = EastFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                if (realZ == 0)
                    for (int i = y - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = SouthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

                if (realX == 0)
                    for (int i = y - LODLevel; i < y + LODLevel; i += LODLevel)
                        if (i != y || !slopedTop)
                            meshData = WestFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

            }
        }


        return meshData;
    }

    private static MeshData FacesHeightGridEdge(int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, int realX, int realZ, int LODLevel, int chunkSize, float steepness)
    {
        if (realZ < (chunkSize - 1))
            if (RoundToLodFrequency(heightGrid[realX, Mathf.Min(realZ + LODLevel, (chunkSize - 1))], LODLevel) < y + 1)
                for (int i = RoundToLodFrequency(heightGrid[realX, Mathf.Min(realZ + LODLevel, (chunkSize - 1))], LODLevel) - LODLevel; i < y + 1; i += LODLevel)
                    meshData = NorthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
        if (realZ > 0)
            if (RoundToLodFrequency(heightGrid[realX, Mathf.Max(realZ - LODLevel, 0)], LODLevel) < y + 1)
                for (int i = RoundToLodFrequency(heightGrid[realX, Mathf.Max(realZ - LODLevel, 0)], LODLevel) - LODLevel; i < y + 1; i += LODLevel)
                    meshData = SouthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

        if (realX < (chunkSize - 1))
            if (RoundToLodFrequency(heightGrid[Mathf.Min(realX + LODLevel, (chunkSize - 1)), realZ], LODLevel) < y + 1)
                for (int i = RoundToLodFrequency(heightGrid[Mathf.Min(realX + LODLevel, chunkSize - 1), realZ], LODLevel) - LODLevel; i < y + 1; i += LODLevel)
                    meshData = EastFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
        if (realX > 0)
            if (RoundToLodFrequency(heightGrid[Mathf.Max(realX - LODLevel, 0), realZ], LODLevel) < y + 1)
                for (int i = RoundToLodFrequency(heightGrid[Mathf.Max(realX - LODLevel, 0), realZ], LODLevel) - LODLevel; i < y + 1; i += LODLevel)
                    meshData = WestFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);

        return meshData;
    }

    private static MeshData FacesHeightGridMiddle(int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, int realX, int realZ, int LODLevel, int chunkSize, float steepness)
    {
        //Max Z
        if (realZ + LODLevel - 1 == (chunkSize - 1) && realZ != 0)
        {
            int hypotheticalHeightGridDist = heightGrid[realX, Mathf.Max(realZ - LODLevel, 0)] - heightGrid[realX, realZ];
            int hypotheticalHeight = heightGrid[realX, realZ] - hypotheticalHeightGridDist;
            meshData = IterateGenerateFaces(x, y, z, meshData, biome, heightGrid, LODLevel, steepness, hypotheticalHeight, 3);
        }

        //0 Z
        if (realZ == 0)
        {
            int hypotheticalHeightGridDist = heightGrid[realX, Mathf.Min(realZ + LODLevel, (chunkSize - 1))] - heightGrid[realX, realZ];
            int hypotheticalHeight = heightGrid[realX, realZ] - hypotheticalHeightGridDist;
            meshData = IterateGenerateFaces(x, y, z, meshData, biome, heightGrid, LODLevel, steepness, hypotheticalHeight, 2);
        }

        //Max X
        if (realX + LODLevel - 1 == (chunkSize - 1) && realX != 0)
        {
            int hypotheticalHeightGridDist = heightGrid[Mathf.Max(realX - LODLevel, 0), realZ] - heightGrid[realX, realZ];
            int hypotheticalHeight = heightGrid[realX, realZ] - hypotheticalHeightGridDist;
            meshData = IterateGenerateFaces(x, y, z, meshData, biome, heightGrid, LODLevel, steepness, hypotheticalHeight, 1);
        }
        //0 X
        if (realX == 0)
        {
            int hypotheticalHeightGridDist = heightGrid[Mathf.Min(realX + LODLevel, (chunkSize - 1)), realZ] - heightGrid[realX, realZ];
            int hypotheticalHeight = heightGrid[realX, realZ] - hypotheticalHeightGridDist;
            meshData = IterateGenerateFaces(x, y, z, meshData, biome, heightGrid, LODLevel, steepness, hypotheticalHeight, 0);
        }

        return meshData;
    }

    private static MeshData IterateGenerateFaces(int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, int LODLevel, float steepness, int hypotheticalHeight, int direction)
    {
        for (int i = hypotheticalHeight - LODLevel; i < y + LODLevel; i += LODLevel)
        {
            if (direction == 0)
                meshData = WestFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 1)
                meshData = EastFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 2)
                meshData = SouthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 3)
                meshData = NorthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
        }
        for (int i = y - Mathf.CeilToInt(3f / LODLevel); i < y + LODLevel; i += LODLevel)
        {
            if (direction == 0)
                meshData = WestFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 1)
                meshData = EastFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 2)
                meshData = SouthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
            if (direction == 3)
                meshData = NorthFace(x, i, z, meshData, biome, heightGrid, steepness, LODLevel);
        }

        return meshData;
    }

    static MeshData UpFace
        (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, out bool slopedTop, float steepness, int LODLevel)
    {
        slopedTop = false;
        bool nedown = false;
        bool sedown = false;
        bool nwdown = false;
        bool swdown = false;

        Vector3[] vertCollection = new Vector3[4];

        //TODO
        //This can be uncommented again! Is temporarily disabled to have straight blocks without sloped tops
        #region disabled sloped tops
        //if (heightGrid == null)
        //{
        //    if (Chance.GetHeight(x, z + LODLevel, biome, heightGrid) < y + 1 || Chance.GetHeight(x + LODLevel, z, biome, heightGrid) < y + 1 || Chance.GetHeight(x + LODLevel, z + LODLevel, biome, heightGrid) < y + 1)
        //    {
        //        vertCollection[1] = new Vector3(0, -1f, 0);
        //        slopedTop = true;
        //        //steepness = 0.(chunkSize-1)5f;
        //        nedown = true;
        //        //if(Chance.GetHeight(x + LODLevel, z + LODLevel, biome, heightGrid) < y)
        //        //vertCollection[1] = new Vector3(0, -2f, 0);
        //    }
        //    if (Chance.GetHeight(x + LODLevel, z, biome, heightGrid) < y + 1 || Chance.GetHeight(x, z - LODLevel, biome, heightGrid) < y + 1 || Chance.GetHeight(x + LODLevel, z - LODLevel, biome, heightGrid) < y + 1)
        //    {
        //        vertCollection[2] = new Vector3(0, -1f, 0);
        //        slopedTop = true;
        //        //steepness = 0.(chunkSize-1)5f;
        //        sedown = true;
        //        //if(Chance.GetHeight(x + LODLevel, z - LODLevel, biome, heightGrid) < y)
        //        //vertCollection[2] = new Vector3(0, -2f, 0);
        //    }
        //    if (Chance.GetHeight(x - LODLevel, z, biome, heightGrid) < y + 1 || Chance.GetHeight(x, z - LODLevel, biome, heightGrid) < y + 1 || Chance.GetHeight(x - LODLevel, z - LODLevel, biome, heightGrid) < y + 1)
        //    {
        //        vertCollection[3] = new Vector3(0, -1f, 0);
        //        slopedTop = true;
        //        //steepness = 0.(chunkSize-1)5f;
        //        swdown = true;
        //        //if(Chance.GetHeight(x - LODLevel, z - LODLevel, biome, heightGrid) < y)
        //        //vertCollection[3] = new Vector3(0, -2f, 0);
        //    }
        //    if (Chance.GetHeight(x - LODLevel, z, biome, heightGrid) < y + 1 || Chance.GetHeight(x, z + LODLevel, biome, heightGrid) < y + 1 || Chance.GetHeight(x - LODLevel, z + LODLevel, biome, heightGrid) < y + 1)
        //    {
        //        vertCollection[0] = new Vector3(0, -1f, 0);
        //        slopedTop = true;
        //        //steepness = 0.(chunkSize-1)5f;
        //        nwdown = true;
        //        //if(Chance.GetHeight(x - LODLevel, z + LODLevel, biome, heightGrid) < y + 1)
        //        //vertCollection[0] = new Vector3(0, -2f, 0);
        //    }
        //}

        //TODO
        //These were already commented before disabling the sloped tops
        //if (nedown)
        //{
        //    if (Chance.GetHeight(x - LODLevel, z, biome, heightGrid) >= y)
        //        meshData = NorthFaceBottomTriangle(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //    if (Chance.GetHeight(x, z - LODLevel, biome, heightGrid) >= y)
        //        meshData = EastFaceBottomTriangleReversed(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //}

        //if (sedown)
        //{
        //    if (Chance.GetHeight(x, z + LODLevel, biome, heightGrid) >= y)
        //        meshData = EastFaceBottomTriangle(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //    if (Chance.GetHeight(x - LODLevel, z, biome, heightGrid) >= y)
        //        meshData = SouthFaceBottomTriangleReversed(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //}
        //if (swdown)
        //{
        //    if (Chance.GetHeight(x + LODLevel, z, biome, heightGrid) >= y)
        //        meshData = SouthFaceBottomTriangle(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //    if (Chance.GetHeight(x, z + LODLevel, biome, heightGrid) >= y)
        //        meshData = WestFaceBottomTriangleReversed(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //}
        //if (nwdown)
        //{
        //    if (Chance.GetHeight(x, z - LODLevel, biome, heightGrid) >= y)
        //        meshData = WestFaceBottomTriangle(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //    if (Chance.GetHeight(x + LODLevel, z, biome, heightGrid) >= y)
        //        meshData = NorthFaceBottomTriangleReversed(x, y, z, meshData, biome, heightGrid, steepness, LODLevel);
        //}
        #endregion

        vertCollection[0] += (new Vector3(x, y + LODLevel, z + LODLevel));
        vertCollection[1] += (new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        vertCollection[2] += (new Vector3(x + LODLevel, y + LODLevel, z));
        vertCollection[3] += (new Vector3(x, y + LODLevel, z));

        //Add to meshdata
        foreach (Vector3 vert in vertCollection)
            meshData.AddVertex(vert);

        meshData.AddQuadTriangles();
        GetUVs.GetUVWithRiver(x, y, z, meshData, biome, steepness);
        return meshData;
    }



    static MeshData DownFace
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData NorthFace
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData EastFace
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData SouthFace
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData WestFace
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x, y, z));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }



    //Bottom triangles
    static MeshData NorthFaceBottomTriangle
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));

        meshData.AddTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData EastFaceBottomTriangle
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));

        meshData.AddTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData SouthFaceBottomTriangle
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));

        meshData.AddTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData WestFaceBottomTriangle
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x, y, z));

        meshData.AddTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    //Bottom triangles
    static MeshData NorthFaceBottomTriangleReversed
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));

        meshData.AddReverseTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData EastFaceBottomTriangleReversed
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z + LODLevel));

        meshData.AddReverseTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData SouthFaceBottomTriangleReversed
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x + LODLevel, y, z));

        meshData.AddReverseTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }

    static MeshData WestFaceBottomTriangleReversed
       (int x, int y, int z, MeshData meshData, Biome biome, int[,] heightGrid, float steepness, int LODLevel)
    {
        meshData.AddVertex(new Vector3(x, y, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z + LODLevel));
        meshData.AddVertex(new Vector3(x, y + LODLevel, z));
        meshData.AddVertex(new Vector3(x, y, z));

        meshData.AddReverseTriTriangles();
        meshData.uv.AddRange(GetUVs.FaceUVs(biome.geography.GetType(y, steepness)));
        return meshData;
    }





}