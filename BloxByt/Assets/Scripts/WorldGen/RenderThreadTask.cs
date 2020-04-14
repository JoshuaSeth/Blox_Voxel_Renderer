//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Profiling;
using System.Threading;

[System.Serializable]
public class RenderThreadTask
{

    public MeshData chunkMeshData;
    public MeshData natureMeshData;
    public Region[] regions;
    public int regionSize;
    public int chunkSize;
    public Biome[] cornerBiomes;
    public int chunkX;
    public int chunkZ;
    public bool finished;
    public bool quitThread;
    public Chunk chunk;
    public bool isSeamingChunk;
    public bool onDiskAlready;
    public int[,] heightGrid;
    public int LODLevel = 1;
    

    public void CreateMeshData(object token)
    {
        try
        {
            //Thread.CurrentThread.Name = "CRT" + chunkX.ToString() + "," + chunkZ.ToString();
            //Create an empty meshdata object
            chunkMeshData = new MeshData();
            natureMeshData = new MeshData();

            //Start with determining the biome for this chunk
            //Biome biome = (Biome)Chance.RandomItemFromList(biomes, chunkX, chunkZ, 4);
            //Profiler.BeginThreadProfiling("My threads", "Chunk Builder");
            //CustomSampler sampler = CustomSampler.Create("Chunk builder");
            //sampler.Begin();

            cornerBiomes = WorldGenUtil.GetCornerBiomes(regions, chunkX, chunkZ,regionSize,chunkSize, out isSeamingChunk);



            //If the chunk is not on the disk and is not a seaming chunk just calculate it
            if (!isSeamingChunk)
            {
                for (int x = 0; x < chunkSize; x += LODLevel)
                {
                    if (quitThread)
                        break;
                    for (int z = 0; z < chunkSize; z += LODLevel)
                    {
                        if (quitThread)
                            break;
                        int heighthere = Chance.GetHeight(chunkX * 8 + x, chunkZ * 8 + z, cornerBiomes[0], null); chunkMeshData = GetMeshData.Blockdata(chunkX * 8 + x, heighthere, chunkZ * 8 + z, chunkMeshData, cornerBiomes[0], null, true, x, z, LODLevel, chunkSize);
                        Item vegetationItem = Chance.Vegetation(chunkX * 8 + x, chunkZ * 8 + z, heighthere, cornerBiomes[0]);
                        if (vegetationItem != null)
                            natureMeshData = GetItemMeshData.MeshForObject(chunkX * 8 + x, heighthere, chunkZ * 8 + z, natureMeshData, vegetationItem);
                    }
                }
            }


            //If the chunk is not on disk but is seaming collect the height of the corners and sent this with the data
            if (isSeamingChunk)
            {
                heightGrid = WorldGenUtil.GetHeightGrid(cornerBiomes, chunkSize, chunkX, chunkZ);
                for (int x = 0; x < chunkSize; x += LODLevel)
                {
                    if (quitThread)
                        break;
                    for (int z = 0; z < chunkSize; z += LODLevel)
                    {
                        if (quitThread)
                            break;
                        int heighthere = heightGrid[x, z];
                        chunkMeshData = GetMeshData.Blockdata(chunkX * 8 + x, heighthere, chunkZ * 8 + z, chunkMeshData, cornerBiomes[0], heightGrid, true, x, z, LODLevel, chunkSize);
                        Item vegetationItem = Chance.Vegetation(chunkX * 8 + x, chunkZ * 8 + z, heighthere, cornerBiomes[0]);
                        if (vegetationItem != null)
                            natureMeshData = GetItemMeshData.MeshForObject(chunkX * 8 + x, heighthere, chunkZ * 8 + z, natureMeshData, vegetationItem);
                    }
                }
            }

            finished = true;
            //sampler.End();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }



}
