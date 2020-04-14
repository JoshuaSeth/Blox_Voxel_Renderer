using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Perlinlayer
{
    public float stretch = 0.1f;
    /// <summary>
    /// The max height of the layer. For 
    /// </summary>
    public int layerMaxHeight = 4;

    public Perlinlayer(float s, int lmh)
    {
        stretch = s;
        layerMaxHeight = lmh;
    }
}
