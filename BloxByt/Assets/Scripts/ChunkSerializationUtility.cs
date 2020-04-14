using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkSerializationUtility
{
    public static string PathForChunk(int chunkX, int chunkY)
    {
        return GetPath(chunkX) + GetPath(chunkY);
    }

    private static string GetPath(int i)
    {
        int remainderInThousandsX = i % 1000;
        int chunkXInThousands = (i - remainderInThousandsX);
        int chunkXThousandsCount = chunkXInThousands / 1000;
        string thousandPathPart = chunkXThousandsCount.ToString() + "/";

        int chunkXMinusThousands = i - chunkXInThousands;

        int remainderInHundredsX = chunkXMinusThousands % 100;
        int ChunkXInHundreds = (chunkXMinusThousands - remainderInHundredsX);
        int ChunkXHundredsCount = ChunkXInHundreds / 100;
        string hundredPathPart = ChunkXHundredsCount.ToString() + "/";

        int chunkXMinusHundreds = i - chunkXInThousands - ChunkXInHundreds;

        int remainderInTensX = chunkXMinusHundreds % 10;
        int ChunkXInTens = (chunkXMinusHundreds - remainderInTensX);
        int ChunkXTensCount = ChunkXInTens / 10;
        string tensPartPath = ChunkXTensCount.ToString() + "/";

        return thousandPathPart + hundredPathPart + tensPartPath;
    }
}
