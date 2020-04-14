using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Variation : ScriptableObject
{
    public List<Cube.Type> variations = new List<Cube.Type>();
}
