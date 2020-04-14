using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Landscape : ScriptableObject
{
    public Biome[] biomes;
    public enum BiomeDistribution {Simplex, Cellular};
    public BiomeDistribution biomeDistribution;
    public float stretch = 1;
}
