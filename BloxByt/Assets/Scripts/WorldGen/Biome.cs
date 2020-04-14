using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Biome : ScriptableObject
{
    public bool useInvertedABS;
    public int baseHeight;
    public int maxHeight;
    public int minHeight;
    public List<Perlinlayer> perlinLayers = new List<Perlinlayer>();
    public Geography geography;

    public int beakWidth;
    public int beakDepth;
    public int riverWidth;
    public int riverDepth;
    public int bigRiverWidth;
    public int bigRiverDepth;


    public List<VegetationLayer> vegetationLayers = new List<VegetationLayer>();
    public List<TypeLayer> typeOverlays = new List<TypeLayer>();

}
