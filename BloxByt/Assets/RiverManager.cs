using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    //Spawn some rivers based on biome river properties
    //Create start point for river
    //Keep track of tiles in river
    //Try to extend at active tiles of river
    //Extend of height is equal or lower
    //Add tiles to active tile
    //Getheight might me a probem for the blending biomes


    public List<Vector3> riverSpots = new List<Vector3>();
    public List<Vector3> riverSpotsPending = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartCo()
    {
        yield return new WaitForSeconds(1);
        ExtendRiver();
        StartCoroutine(StartCo());
    }

    public void ExtendRiver()
    {
        Debug.Log("try extend river");
        List<Vector3> addables = new List<Vector3>();
        List<Vector3> dels = new List<Vector3>();
        //Select random spot in current frustrum
        if (riverSpots.Count==0)
        {
            Vector2Int startSpot = new Vector2Int(Random.Range(Frustrum.currentMinChunkPosXInView * 8, Frustrum.currentMaxChunkPosXInView * 8), Random.Range(Frustrum.currentMinChunkPosZInView * 8, Frustrum.currentMaxChunkPosZInView * 8));
            AddIfNotInRiver(addables, new Vector3(startSpot.x,999,startSpot.y), 0, 0);
        }
        else
        {
            if (riverSpots.Count < 1000)
            {
                int count = 0;
                foreach (Vector3 pendingSpot in riverSpotsPending)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            if (!(x == 0 && z == 0))
                            {
                                AddIfNotInRiver(addables, pendingSpot, x, z);
                                dels.Add(pendingSpot);
                            }
                        }
                    }
                    if (count == 10)
                        break;
                }

            }

        }
        foreach (Vector3 del in dels)
            riverSpotsPending.Remove(del);
        foreach (Vector3 add in addables)
            riverSpotsPending.Add(add);

    }

    private void AddIfNotInRiver(List<Vector3> addables, Vector3 pendingSpot, int x, int z)
    {
        Vector2Int newSpot = new Vector2Int(Mathf.RoundToInt(pendingSpot.x) + x, Mathf.RoundToInt(pendingSpot.z) + z);
        Vector2Int chunkPos = GetChunkPos(newSpot);
        int height = Chance.GetHeightPrecise(newSpot.x, newSpot.y, chunkPos.x, chunkPos.y);
        Vector3 realPos = new Vector3(newSpot.x, height, newSpot.y);
        if (!riverSpots.Contains(realPos) && height<pendingSpot.y)
        {
            riverSpots.Add(realPos);
            PrimSpawn.inst.RequestRender(realPos, Color.red);
            addables.Add(realPos);
        }
    }

    private static Vector2Int GetChunkPos(Vector2Int startSpot)
    {
        float xRemain = startSpot.x % 8;
        int chunkPosX = Mathf.RoundToInt(startSpot.x - (8 - xRemain)) / 8;
        float zRemain = startSpot.x % 8;
        int chunkPosZ = Mathf.RoundToInt(startSpot.y - (8 - zRemain)) / 8;
        return new Vector2Int(chunkPosX, chunkPosZ);
    }
}
