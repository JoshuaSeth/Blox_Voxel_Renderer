using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// Chance provides random outcomes that can be exactly reproduced with the same coordinate and the same seed.
/// </summary>
public static class Chance
{
    public static int seed = 1000;
    public static FastNoise fn;
    public static FastNoise fnVoronoi;
    public static FastNoise fnOnelayer;
    public static FastNoise fnWhiteNoise;

    public static void Initialize()
    {
        fn = new FastNoise();
        fn.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
        fn.SetFractalOctaves(4);
        fnVoronoi = new FastNoise();
        fnVoronoi.SetNoiseType(FastNoise.NoiseType.Cellular);
        fnOnelayer = new FastNoise();
        fnOnelayer.SetNoiseType(FastNoise.NoiseType.Simplex);
        fnOnelayer.SetFractalOctaves(1);
        fnWhiteNoise = new FastNoise();
        fnWhiteNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
    }

    public static int GetHeight(int x, int z, Biome biome, int[,] heightGrid)
    {
        int count = 0;
        //count = GetRiverHeights(x, z, count,biome);
        foreach (Perlinlayer layer in biome.perlinLayers)
            count += Mathf.FloorToInt(fn.GetSimplexFractal(x * layer.stretch, z * layer.stretch) * layer.layerMaxHeight + biome.baseHeight);
        if (count > biome.maxHeight)
            count = biome.maxHeight;
        if (count < biome.minHeight)
            count = biome.minHeight;
        return count;
    }

    public static Item Vegetation(int x, int z,int y, Biome biome)
    {
        foreach (VegetationLayer layer in biome.vegetationLayers)
        {
            if (y > layer.minHeight && y < layer.maxHeight)
            {
                float accumulation = 0;
                foreach (Perlinlayer perlinLayer in layer.perlinLayers)
                {
                    accumulation += fn.GetSimplex(x * perlinLayer.stretch +10000, z * perlinLayer.stretch- 10000);
                    if (accumulation/(layer.perlinLayers.Count) > layer.thresHold)
                    {
                        if (layer.useWhiteNoiseFiler)
                        {
                            if (fn.GetWhiteNoise(x*0.02f, z*0.02f) > 0.5f)
                                return layer.item;
                        }
                        else
                            return layer.item;
                    }
                }
            }
        }
        return null;
    }

    public static Cube.Type TypeOverlay(int x, int z, int y, Biome biome)
    {
        foreach (TypeLayer layer in biome.typeOverlays)
        {
            if (y > layer.minHeight && y < layer.maxHeight)
            {
                float accumulation = 0;
                foreach (Perlinlayer perlinLayer in layer.perlinLayers)
                {
                    accumulation += fn.GetSimplex(x * perlinLayer.stretch + 1000, z * perlinLayer.stretch - 4253);
                    if (accumulation / layer.perlinLayers.Count > layer.thresHold)
                    {
                        return layer.type;
                    }
                }
            }
        }
        return Cube.Type.None;
    }


    public static int SimplexNr(int x, int z, int total) {
        float stepSize = 1f / total;
        float perlinChance = fn.GetSimplex(x * 10000, z * 10000);
        for (float i = 0; i <= 1; i += stepSize)
        {
            if (i > perlinChance)
            {
                int index = Mathf.FloorToInt(i / stepSize);
                if (index < 0)
                    index = 0;
                if (index >= total)
                    index = total - 1;
                return index;
            }
        }
        return -1;
    }


    public static object RandomItemFromList(object[] options, float x, float z, float stretch, bool shakeList, Landscape.BiomeDistribution distribution)
    {
        float stepSize = 1f / options.Length;
        float perlinChance=-1f;
        if (distribution==Landscape.BiomeDistribution.Simplex)
            perlinChance = fn.GetSimplex(235+x * 2f / stretch, -48838+z * 2f / stretch);
        if (distribution == Landscape.BiomeDistribution.Cellular)
            perlinChance = fnVoronoi.GetCellular(x * 6f / stretch, z * 6f / stretch);
        for (float i = 0; i <= 1; i += stepSize)
        {
            if (i > perlinChance)
            {
                int index = Mathf.FloorToInt(i / stepSize);
                if (index < 0)
                    index = 0;
                if (index >= options.Length)
                    index = options.Length - 1;
                return options[index];
            }
        }
        Debug.LogError("Item from list function incorrect, chance:" + perlinChance);
        return null;
    }

    /// <summary>
    /// Absulate x and z. Gets the exact height at this place and calculates landscape and biome itself. If it is a blending chunk it calculates that too. Very Expensive Method.
    /// </summary>
    public static int GetHeightPrecise(int x, int z, int chunkX, int chunkZ) {
        Biome biome = (Biome)RandomRegionLandscapeAndBiome(ChunksManager.regions, x, z, ChunksManager.regionSize, false);
        Biome[] cornerBiomes = WorldGenUtil.GetCornerBiomes(ChunksManager.regions, chunkX, chunkZ, ChunksManager.regionSize, Settings.chunkSize, out bool isSeamingChunk);
        if (isSeamingChunk)
            return WorldGenUtil.GetHeightGrid(cornerBiomes, Settings.chunkSize, chunkX, chunkZ)[Mathf.Abs(x%8), Mathf.Abs(z %8)];
        else
            return GetHeight(x, z, biome, null);
    }


    public static object RandomRegionLandscapeAndBiome(object[] options, float x, float z, float worldRegionsSize, bool shakeList)
    {
        Region region = (Region)RandomItemFromList(options, x, z, worldRegionsSize, shakeList, Landscape.BiomeDistribution.Simplex);
        Landscape landscape = (Landscape)RandomItemFromList(region.landscapes, x, z, region.stretch, shakeList, region.distribution);
        return RandomItemFromList(landscape.biomes, x, z, landscape.stretch, shakeList, landscape.biomeDistribution);
    }
}
