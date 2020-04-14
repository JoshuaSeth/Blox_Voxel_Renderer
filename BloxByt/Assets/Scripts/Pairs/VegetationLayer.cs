using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class VegetationLayer : ScriptableObject
{
    public int minHeight;
    public int maxHeight;
    public float thresHold;
    public Item item;
    public bool useWhiteNoiseFiler;
    public List<Perlinlayer> perlinLayers = new List<Perlinlayer>();
}
