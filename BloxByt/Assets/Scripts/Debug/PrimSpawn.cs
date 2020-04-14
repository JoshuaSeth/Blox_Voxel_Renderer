using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimSpawn : MonoBehaviour
{

    public static PrimSpawn inst;

    public List<PosColorPair> primitives = new List<PosColorPair>();

    private void Awake()
    {
        inst = this;
    }

    private void Update()
    {
        foreach (PosColorPair pair in primitives)
        {
            if (pair != null)
            {
                if (!pair.spawned)
                    SpawnPrimite(pair.pos, pair.col);
                pair.spawned = true;
            }
        }
    }

    public void RequestRender(Vector3 pos, Color col)
    {
        PosColorPair pair = new PosColorPair();
        pair.pos = pos;
        pair.col = col;
        primitives.Add(pair);
    }

    public void SpawnPrimite(Vector3 pos, Color col)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder) as GameObject;
        go.transform.position = pos;
        go.GetComponent<MeshRenderer>().material.color = col;
    }
}

public class PosColorPair
{
    public Vector3 pos;
    public Color col;
    public bool spawned;
}
