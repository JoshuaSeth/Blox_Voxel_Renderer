using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameObjectHandler : MonoBehaviour
{
    public static List<GameObject> pool = new List<GameObject>();
    static int startSize = 1000;
   

    // Start is called before the first frame update
    public static void Init()
    {
        for(int i = 0; i < startSize; i++)
        {
            GameObject go = GameObject.Instantiate(ChunksManager.chunkPrfb) as GameObject;
            go.SetActive(false);
            pool.Add(go);
        }
    }

    public static GameObject GetGameObject()
    {
        Debug.Log("Giving GO");
        foreach (GameObject gob in pool)
            if (!gob.active)
            {
                gob.SetActive(true);
                return gob;
            }
        GameObject go = GameObject.Instantiate(ChunksManager.chunkPrfb) as GameObject;
        pool.Add(go);
        Debug.Log(pool.Count);
        return go;
    }

    public static void SetAvailable(GameObject go)
    {
        go.SetActive(false);
    }

}
