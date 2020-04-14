using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System.Threading;


public class ChunksManager : MonoBehaviour
{
    public static ChunksManager instance;

    //World properties
    public static int regionSize = 2, minMapHeight = -200, MapHeight = 200;

    //Resources
    public static Region[] regions;
    public static Material blockMat;
    public static Material waterMat;
    public static GameObject chunkPrfb;

    //Lists keeping check of rendered chunks
    public static Collection chunks = new Collection();

    private void Awake()
    {
        Physics.autoSimulation = false;
        instance = this;
        regions = Resources.LoadAll<Region>("Regions/");
        blockMat = Resources.LoadAll<Material>("Materials/")[0];
        chunkPrfb = Resources.LoadAll<GameObject>("Prefabs/")[0];
        Chance.Initialize();
        GameObjectHandler.Init();
    }

    void Start()
    {
        StartCoroutine(FinalizeFinishedChunksContinuous());
    }

    private void Update()
    {
        UpdateMap();
    }

    public static void UpdateMap()
    {
        if (Frustrum.isUpdated)
        {
            RemoveChunksNotInFrustrum();
            RequestChunksInFrustrum();
            Frustrum.isUpdated = false;
        }
        ReLODChunks();
    }


    public IEnumerator FinalizeFinishedChunksContinuous()
    {
        FinalizeFinishedChunks();

        if (Mathf.Approximately(Settings.chunkFinalizationRefreshRate, 0))
        {
            yield return null;
            StartCoroutine(FinalizeFinishedChunksContinuous());
        }
        else
        {
            yield return new WaitForSeconds(Settings.chunkFinalizationRefreshRate);
            StartCoroutine(FinalizeFinishedChunksContinuous());
        }
    }

    private static void FinalizeFinishedChunks()
    {
        List<PositionInfo> deletables = new List<PositionInfo>();
        int count = 0;

        foreach (PositionInfo info in chunks.AsList)
        {
            {
                if (info.thread != null)
                {
                    if (info.thread.finished)
                    {
                        //if this thread finished without being asked to quit
                        if (!info.thread.quitThread)
                        {
                            FinalizeChunkCreation(info);
                        }

                        //If this thread was asked to quit
                        if(info.thread!=null)
                            if (info.thread.quitThread)
                                StopThreadRemoveChunk(deletables, info);

                    }
                    count++;
                }

                if (count > Settings.finalizeChunksPerFrame)
                    break;
            }
        }



        //GameObjectHandler.StartSetAvailableRequests();
        chunks.StartDeletionActivity();
        foreach (PositionInfo info in deletables)
        {
            //GameObjectHandler.SetAvailable(info.chunk);
            chunks.Remove(info.key);
        }
        chunks.FinishDeletionActivity();
        //GameObjectHandler.FinishSetAvailableRequests();
    }



    private static bool isNotFirstRequest;
    private static void RequestChunksInFrustrum()
    {
        int count = 0;
        List<PositionInfo> chunksCreated = new List<PositionInfo>();
        for (int x = Mathf.RoundToInt(Frustrum.CurrentMinChunkPos.x / (Settings.chunkSize/8))*(Settings.chunkSize/8); x < Frustrum.CurrentMaxChunkPos.x; x+=(Settings.chunkSize/8))
            for (int z = Mathf.RoundToInt(Frustrum.CurrentMinChunkPos.y / (Settings.chunkSize / 8)) * (Settings.chunkSize / 8); z < Frustrum.CurrentMaxChunkPos.y; z+= (Settings.chunkSize / 8))
            {
                Vector2Int chunkPos = new Vector2Int(x,z);
                if (chunkPos.x < Frustrum.PreviousLowestChunkPos.x+2 || chunkPos.y < Frustrum.PreviousLowestChunkPos.y + 2 || chunkPos.x > Frustrum.PreviousHighestChunkPos.x-2 || chunkPos.y > Frustrum.PreviousHighestChunkPos.y-2)
                {
                    if (!chunks.Contains(chunkPos))
                    {
                        if (!isNotFirstRequest)
                            chunksCreated.Add(NewChunk(chunkPos));
                        if (isNotFirstRequest)
                            chunksCreated.Add(NewChunk(chunkPos));

                        count++;
                    }
                }

            }
        foreach (PositionInfo info in chunksCreated)
            chunks.Add(info.key, info);
        isNotFirstRequest = true;
    }


    private static void ReLODChunks()
    {
        int count = 0;
        //If something is rendered unnecesary it is asked to quit. After the thread reacts and stops itself the gameobject is deleted.
        foreach (PositionInfo info in chunks.AsList)
        {
            int lod = LODLevelForDist(Vector3.Distance(Frustrum.cam.transform.position, new Vector3(info.key.x * 8, 0, info.key.y * 8)));
            if (info.LODLevel != lod||Settings.chunkSize != info.chunkSize)
            {
                if(info.thread!=null)
                    info.thread.quitThread=true;
                info.LODLevel = lod;
                info.chunkSize = Settings.chunkSize;
                info.thread = BuildChunk(info.key.x, info.key.y, info);
                count++;
            }
            if (count == 32)
                break;
        }
            
    }


    public static int LODLevelForDist(float dist) {
        if (dist < 100)
            return 1;

        if ((dist < 220))
            return 2;
            
        if ((dist < 500))
            return 4;

        if ((dist < 700))
            return 8;

        if ((dist < 1000))
            return 16;

        return 32;
    }



    private static void RemoveChunksNotInFrustrum()
    {
        List<PositionInfo> deletables = new List<PositionInfo>();
        //If something is rendered unnecesary it is asked to quit. After the thread reacts and stops itself the gameobject is deleted.

        foreach (PositionInfo info in chunks.AsList) {
            if (info.key.x < Frustrum.currentMinChunkPosXInView || info.key.y < Frustrum.currentMinChunkPosZInView || info.key.y > Frustrum.currentMaxChunkPosZInView || info.key.x > Frustrum.currentMaxChunkPosXInView)
            {//If it is not finished let it be deleted after finishing
                deletables.Add(info);
            }
            else if (info.key.x*8 % (Settings.chunkSize/8) != 0 || info.key.y*8 % (Settings.chunkSize / 8) != 0)
            {
                deletables.Add(info);
            }
        }

        chunks.StartDeletionActivity();
        foreach (PositionInfo info in deletables)
        {
            GameObjectHandler.SetAvailable(info.chunk);
            chunks.Remove(info.key);
        }
        chunks.FinishDeletionActivity();
    }

    #region
    public static PositionInfo NewChunk(Vector2Int chunkPos)
    {
        try
        {
            PositionInfo info = new PositionInfo();
            float distance = Vector3.Distance(Frustrum.cam.transform.position, new Vector3(chunkPos.x * 8, 0, chunkPos.y * 8));
            info.LODLevel = LODLevelForDist(distance);
            info.key = chunkPos;
            info.chunkSize = Settings.chunkSize;
            info.thread = BuildChunk(chunkPos.x, chunkPos.y, info);
            return info;
        }
        catch (System.Exception e) { Debug.Log(e); return null; }
    }



    public static void FinalizeChunkCreation(PositionInfo info)
    {
        bool needsGO = true;
        //Destroy placeholder GO
        if (info.chunk != null)
        {
            //GameObject.Destroy(info.chunk);
            needsGO = false;
        }


        //Extract Meshdata from task
        MeshData chunkMeshData = info.thread.chunkMeshData;
        MeshData natureMesh = info.thread.natureMeshData;

        //Request finalization and add to list of renders
        info.chunk = AddMeshToGO(chunkMeshData, natureMesh, info.thread.chunkX, info.thread.chunkZ, needsGO, info);

        info.thread = null;
    }

    public static void StopThreadRemoveChunk(List<PositionInfo> deletables, PositionInfo info)
    {
        if (info.chunk != null)
            GameObjectHandler.SetAvailable(info.chunk);
        deletables.Add(info);
    }

    public static GameObject AddMeshToGO(MeshData chunkMeshData, MeshData natureMeshData, int chunkX, int chunkZ, bool needsGO, PositionInfo info)
    {
        //Mesh
        Mesh mesh = MeshUtil.TransferMeshData(chunkMeshData);

        //Set up GO
        //GameObject chunkGO = GameObject.Instantiate(chunkPrfb);
        GameObject chunkGO = info.chunk;
        if(needsGO)
            chunkGO = GameObjectHandler.GetGameObject();
        chunkGO.GetComponent<MeshRenderer>().sharedMaterial = blockMat;
        chunkGO.GetComponent<MeshFilter>().mesh = mesh;

        //Naturr mesh
        //Mesh meshn = MeshUtil.TransferMeshData(natureMeshData);

        //Set up nature
        //GameObject nature = GameObject.Instantiate(chunkPrfb);
        //GameObject nature = GameObjectHandler.GetGameObject();
        //nature.transform.SetParent(chunkGO.transform);
        //nature.GetComponent<MeshRenderer>().sharedMaterial = blockMat;
        //nature.GetComponent<MeshFilter>().mesh = meshn;

        //Collider
        //chunkGO.GetComponent<MeshCollider>().sharedMesh = meshn;

        return chunkGO;
    }



    public static RenderThreadTask BuildChunk(int chunkX, int chunkZ, PositionInfo info)
    {
        //Initialize the thread data
        RenderThreadTask task = new RenderThreadTask();
        task.chunkX = chunkX;
        task.chunkZ = chunkZ;
        task.LODLevel = info.LODLevel;
        task.regions = regions;
        task.regionSize = regionSize;
        task.chunkSize = info.chunkSize;

        //Check if its already a file
        Chunk chunk = null;
        if (Serialize.ChunkIsOnDisk(chunkX, chunkZ))
            chunk = Serialize.LoadChunk(chunkX, chunkZ);
        task.chunk = chunk;

        //Start thread
        ThreadPool.QueueUserWorkItem(task.CreateMeshData);
        return task;
    }
    #endregion
}
