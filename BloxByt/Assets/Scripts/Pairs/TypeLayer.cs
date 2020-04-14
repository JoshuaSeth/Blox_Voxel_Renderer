using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class TypeLayer : ScriptableObject
{
    public int minHeight;
    public int maxHeight;
    public float thresHold;
    public Cube.Type type;
    public List<Perlinlayer> perlinLayers = new List<Perlinlayer>();
}
