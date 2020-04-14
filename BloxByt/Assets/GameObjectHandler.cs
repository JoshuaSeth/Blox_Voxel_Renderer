using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameObjectHandler : MonoBehaviour
{
    public List<GameObject> pool = new List<GameObject>();
    static int startSize = 100;

    public static GameObjectHandler i;

    private void Awake()
    {
        i = this;
    }


    // Start is called before the first frame update
    public void Init()
    {
        //for (int i = 0; i < startSize; i++)
        //{
        //    GameObject go = GameObject.Instantiate(ChunksManager.chunkPrfb) as GameObject;
        //    go.SetActive(false);
        //    pool.Add(go);
        //}
    }

    public GameObject GetGameObject()
    {
        //foreach (GameObject gob in pool)
            //if (!gob.activeInHierarchy)
            //{
            //    gob.SetActive(true);
            //    return gob;
            //}
        GameObject go = GameObject.Instantiate(ChunksManager.chunkPrfb) as GameObject;
        //pool.Add(go);
        //Debug.Log(pool.Count);
        return go;
    }

    public void SetAvailable(GameObject go)
    {
        GameObject.Destroy(go);
    }

}
