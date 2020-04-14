using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetUVs
{
    public struct Tile { public int x; public int y; }
    const float tileSize = 0.125f;

    public static Tile TexturePosition(Cube.Type type)
    {
        Tile tile = new Tile();
        tile.x = 1;
        tile.y = 0;

        if (type == Cube.Type.Grass)
        {
            tile.x = 2;
            tile.y = 4;
        }
        if (type == Cube.Type.Rock)
        {
            tile.x = 0;
            tile.y = 4;
        }
        if (type == Cube.Type.MarbleWall)
        {
            tile.x = 0;
            tile.y = 5;
        }
        if (type == Cube.Type.Wood)
        {
            tile.x = 1;
            tile.y = 5;
        }
        if (type == Cube.Type.Sand)
        {
            tile.x = 3;
            tile.y = 5;
        }
        if (type == Cube.Type.Water)
        {
            tile.x = 2;
            tile.y = 5;
        }
        if (type == Cube.Type.Sandstone)
        {
            tile.x = 3;
            tile.y = 6;
        }
        if (type == Cube.Type.Terracotta)
        {
            tile.x = 3;
            tile.y = 4;
        }
        if (type == Cube.Type.Snow)
        {
            tile.x = 5;
            tile.y = 6;
        }
        if (type == Cube.Type.Ice)
        {
            tile.x = 6;
            tile.y = 6;
        }
        if (type == Cube.Type.Clay)
        {
            tile.x = 4;
            tile.y = 7;
        }
        if (type == Cube.Type.Dry_Grass)
        {
            tile.x = 4;
            tile.y = 5;
        }
        if (type == Cube.Type.Desert_Grass)
        {
            tile.x = 4;
            tile.y = 4;
        }
        if (type == Cube.Type.Lush_Grass)
        {
            tile.x = 5;
            tile.y = 7;
        }
        if (type == Cube.Type.Dirt)
        {
            tile.x = 1;
            tile.y = 4;
        }
        if (type == Cube.Type.Green_Grass)
        {
            tile.x = 0;
            tile.y = 3;
        }
        if (type == Cube.Type.Brown_Grass)
        {
            tile.x = 1;
            tile.y = 3;
        }
        if (type == Cube.Type.Dark_Grass)
        {
            tile.x = 2;
            tile.y = 3;
        }
        if (type == Cube.Type.Light_Grass)
        {
            tile.x = 3;
            tile.y = 3;
        }
        if (type == Cube.Type.Oak_Wood)
        {
            tile.x = 4;
            tile.y = 3;
        }
        if (type == Cube.Type.Pine_Wood)
        {
            tile.x = 5;
            tile.y = 3;
        }
        if (type == Cube.Type.Birch_Wood)
        {
            tile.x = 6;
            tile.y = 3;
        }
        if (type == Cube.Type.Oak_Planks)
        {
            tile.x = 5;
            tile.y = 4;
        }
        if (type == Cube.Type.Pine_Planks)
        {
            tile.x = 5;
            tile.y = 5;
        }
        if (type == Cube.Type.Birch_Planks)
        {
            tile.x = 6;
            tile.y = 4;
        }
        if (type == Cube.Type.Planks)
        {
            tile.x = 6;
            tile.y = 5;
        }

        return tile;
    }

    public static Vector2[] FaceUVs(Cube.Type type)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(type);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);

        return UVs;
    }


    public static void GetUVWithRiver(int x, int y, int z, MeshData meshData, Biome biome, float steepness)
    {
        //This  is working but inactivated, reactivating it should work normally 
        //
        //
        //float ridgeHeight = Mathf.Abs(100 * Chance.fnOnelayer.GetSimplex(x + 34, z - 84740));
        //if (ridgeHeight < biome.beakWidth)
        //{
        //    meshData.uv.AddRange(FaceUVs(Cube.Type.Water));
        //    return;
        //}

        //ridgeHeight = Mathf.Abs(100 * Chance.fnOnelayer.GetSimplex(x / 8 + 34, z / 8 - 84740));
        //if (ridgeHeight < biome.riverWidth)
        //{
        //    meshData.uv.AddRange(FaceUVs(Cube.Type.Water));
        //    return;
        //}

        //ridgeHeight = Mathf.Abs(100 * Chance.fnOnelayer.GetSimplex(x / 64 + 34, z / 64 - 84740));
        //if (ridgeHeight < biome.bigRiverWidth)
        //{
        //    meshData.uv.AddRange(FaceUVs(Cube.Type.Water));
        //    return;
        //}

        //Cube.Type type = Chance.TypeOverlay(x, z, y, biome);
        //if (type != Cube.Type.None)
        //{
        //    meshData.uv.AddRange(FaceUVs(type));
        //    return;
        //}

        //else
            meshData.uv.AddRange(FaceUVs(biome.geography.GetType(y, steepness)));
    }
}
