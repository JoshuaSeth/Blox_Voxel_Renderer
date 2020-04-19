using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenUtil
{
    public static int[,] GetHeightGrid(Biome[] cornerBiomes, int chunkSize, int chunkX, int chunkZ)
    {
        int[,] heightGrid = new int[chunkSize, chunkSize];

        //What the heights initially would be
        heightGrid[chunkSize - 1, chunkSize - 1] = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8 + chunkSize, cornerBiomes[0], null);
        heightGrid[chunkSize - 1, 0] = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8, cornerBiomes[0], null);
        heightGrid[0, 0] = Chance.GetHeight(chunkX * 8, chunkZ * 8, cornerBiomes[0], null);
        heightGrid[0, chunkSize - 1] = Chance.GetHeight(chunkX * 8, chunkZ * 8 + chunkSize, cornerBiomes[0], null);

        //NE
        if (cornerBiomes[1] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8 + chunkSize, cornerBiomes[1], null);
            if (temp < heightGrid[chunkSize - 1, chunkSize - 1])
                heightGrid[chunkSize - 1, chunkSize - 1] = temp;
        }
        if (cornerBiomes[2] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8 + chunkSize, cornerBiomes[2], null);
            if (temp < heightGrid[chunkSize - 1, chunkSize - 1])
                heightGrid[chunkSize - 1, chunkSize - 1] = temp;
        }
        if (cornerBiomes[5] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8 + chunkSize, cornerBiomes[5], null);
            if (temp < heightGrid[chunkSize - 1, chunkSize - 1])
                heightGrid[chunkSize - 1, chunkSize - 1] = temp;
        }

        //SE
        if (cornerBiomes[3] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8, cornerBiomes[3], null);
            if (temp < heightGrid[chunkSize - 1, 0])
                heightGrid[chunkSize - 1, 0] = temp;
        }
        if (cornerBiomes[2] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8, cornerBiomes[2], null);
            if (temp < heightGrid[chunkSize - 1, 0])
                heightGrid[chunkSize - 1, 0] = temp;
        }
        if (cornerBiomes[6] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8 + chunkSize, chunkZ * 8, cornerBiomes[6], null);
            if (temp < heightGrid[chunkSize - 1, 0])
                heightGrid[chunkSize - 1, 0] = temp;
        }

        //SW
        if (cornerBiomes[3] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8, cornerBiomes[3], null);
            if (temp < heightGrid[0, 0])
                heightGrid[0, 0] = temp;
        }
        if (cornerBiomes[4] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8, cornerBiomes[4], null);
            if (temp < heightGrid[0, 0])
                heightGrid[0, 0] = temp;
        }
        if (cornerBiomes[7] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8, cornerBiomes[7], null);
            if (temp < heightGrid[0, 0])
                heightGrid[0, 0] = temp;
        }

        //NW
        if (cornerBiomes[1] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8 + chunkSize, cornerBiomes[1], null);
            if (temp < heightGrid[0, chunkSize - 1])
                heightGrid[0, chunkSize - 1] = temp;
        }
        if (cornerBiomes[4] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8 + chunkSize, cornerBiomes[4], null);
            if (temp < heightGrid[0, chunkSize - 1])
                heightGrid[0, chunkSize - 1] = temp;
        }
        if (cornerBiomes[8] != cornerBiomes[0])
        {
            int temp = Chance.GetHeight(chunkX * 8, chunkZ * 8 + chunkSize, cornerBiomes[8], null);
            if (temp < heightGrid[0, chunkSize - 1])
                heightGrid[0, chunkSize - 1] = temp;
        }

        //Create some cylinders for debugging
        //PrimSpawn.inst.RequestRender(new Vector3(chunkX * 8+ 8, heightGrid[chunkSize-1, chunkSize-1], chunkZ * 8 + 8), Color.cyan);
        //PrimSpawn.inst.RequestRender(new Vector3(chunkX * 8+ 8, heightGrid[chunkSize-1, 0], chunkZ * 8), Color.cyan);
        //PrimSpawn.inst.RequestRender(new Vector3(chunkX * 8, heightGrid[0, 0], chunkZ * 8), Color.cyan);
        //PrimSpawn.inst.RequestRender(new Vector3(chunkX * 8, heightGrid[0, chunkSize-1], chunkZ * 8 + 8), Color.cyan);

        //Fill in the height grid

        //Lines between corners
        for (int i = 0; i < chunkSize; i++)
        {
            heightGrid[chunkSize - 1, i] = Mathf.RoundToInt(Mathf.Lerp(heightGrid[chunkSize - 1, 0], heightGrid[chunkSize - 1, chunkSize - 1], 0.125f * (i + 1)));
            heightGrid[i, 0] = Mathf.RoundToInt(Mathf.Lerp(heightGrid[0, 0], heightGrid[chunkSize - 1, 0], 0.125f * (i + 1)));
            heightGrid[0, i] = Mathf.RoundToInt(Mathf.Lerp(heightGrid[0, 0], heightGrid[0, chunkSize - 1], 0.125f * (i + 1)));
            heightGrid[i, chunkSize - 1] = Mathf.RoundToInt(Mathf.Lerp(heightGrid[0, chunkSize - 1], heightGrid[chunkSize - 1, chunkSize - 1], 0.125f * (i + 1)));
        }

        //lines
        for (int i = 1; i < chunkSize - 1; i++)
        {
            for (int j = 1; j < chunkSize - 1; j++)
            {
                heightGrid[i, j] = Mathf.RoundToInt(Mathf.Lerp(heightGrid[i, 0], heightGrid[i, chunkSize - 1], 0.125f * j));
                //+ Mathf.Lerp(heightGrid[j, 0], heightGrid[j, chunkSize-1], 0.125f * i)/2f)
            }
        }

        return heightGrid;
    }

    public static Biome[] GetCornerBiomes(Region[] regions,int chunkX,int chunkZ, int regionSize, int chunkSize, out bool isSeamingChunk)
    {
        isSeamingChunk = false;
        Biome[] cornerBiomes = new Biome[9];
        cornerBiomes[0] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX, chunkZ, regionSize, true);
        cornerBiomes[1] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX, chunkZ + chunkSize / 8, regionSize, true);
        cornerBiomes[2] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX + chunkSize / 8, chunkZ, regionSize, true);
        cornerBiomes[3] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX, chunkZ - chunkSize / 8, regionSize, true);
        cornerBiomes[4] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX - chunkSize / 8, chunkZ, regionSize, true);
        cornerBiomes[5] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX + chunkSize / 8, chunkZ + chunkSize / 8, regionSize, true);
        cornerBiomes[6] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX + chunkSize / 8, chunkZ - chunkSize / 8, regionSize, true);
        cornerBiomes[7] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX - chunkSize / 8, chunkZ - chunkSize / 8, regionSize, true);
        cornerBiomes[8] = (Biome)Chance.RandomRegionLandscapeAndBiome(regions, chunkX - chunkSize / 8, chunkZ + chunkSize / 8, regionSize, true);
        foreach (Biome biome in cornerBiomes)
            if (cornerBiomes[0] != biome)
                isSeamingChunk = true;

        return cornerBiomes;
    }
}
