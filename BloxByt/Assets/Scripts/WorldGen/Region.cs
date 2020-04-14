using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Region : ScriptableObject
{
    public Landscape[] landscapes;
    public Landscape.BiomeDistribution distribution;
    public float stretch= 1;
}
