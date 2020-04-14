using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TypeIntFloatPair
{
    public Cube.Type type;
    public Cube.Type typeIfToSteep;
    public int height;
    public float maxSteepness;
}
