using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Geography : ScriptableObject
{
    public List<TypeIntFloatPair> cubetypeAtHeights = new List<TypeIntFloatPair>();
    [HideInInspector]public TypeIntFloatPair[] types;

    private void OnEnable()
    {
        SetTypes();
    }

    public void SetTypes()
    {
        types = new TypeIntFloatPair[ChunksManager.minMapHeight*-1 + ChunksManager.MapHeight];
        foreach(TypeIntFloatPair tp in cubetypeAtHeights)
        {
            for (int i = 0; i < ChunksManager.minMapHeight * -1 + ChunksManager.MapHeight; i++)
            {
                if(i > tp.height && !IsAvailableForOtherBlocks(i, tp.height))
                    types[i] = tp;
            }
        }
        //Debug.Log("Types set for geography " + this.name);
    }

    private bool IsAvailableForOtherBlocks(int y, int height)
    {
        foreach(TypeIntFloatPair tp in cubetypeAtHeights)
        {
            if (tp.height < y && height<tp.height)
                return true;
        }
        return false;
    }

    public Cube.Type GetType(int y, float steepness)
    {
        if (types[y + ChunksManager.minMapHeight * -1].maxSteepness > steepness)
        {
            //Debug.Log("steepness: " + steepness.ToString() + ", height: " + y.ToString() + ", Cube: " + types[y + 45].type.ToString());
            return types[y + ChunksManager.minMapHeight * -1].type;
        }
        else
            return types[y + ChunksManager.minMapHeight * -1].typeIfToSteep;
    }

}
