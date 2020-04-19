using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class Serialize : MonoBehaviour
{

    static IFormatter formatter;

    private void Awake()
    {
        formatter = new BinaryFormatter();
    }

    public static string FileName(string name, string path)
    {
        string fileName = Application.persistentDataPath +"/" + path +name+".bin";
        return fileName;
    }

    public static bool FileExists(string name, string path)
    {
        string saveFile = FileName(name, path);

        if (!File.Exists(saveFile))
            return false;
        return true;
    }

    public static bool PathExists(string path)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" +path))
            return false;
        return true;
    }

    public static bool ChunkIsOnDisk(int chunkX, int chunkY)
    {
        string path = ChunkSerializationUtility.PathForChunk(chunkX, chunkY);
        string name = chunkX.ToString() + "," + chunkY.ToString();
        if (!PathExists(path))
            return false;
        else if (FileExists(name, path))
            return true;
        return false;
    }

    public static void SaveObject(object obj, string name, string path)
    {
        if(!Directory.Exists(Application.persistentDataPath + "/" + path))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + path);
        Stream stream = new FileStream(FileName(name, path), FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, obj);
        stream.Close();
    }

    public static Chunk LoadChunk(int chunkX, int chunkY)
    {
        string chunkName = chunkX.ToString() + "," + chunkY.ToString();
        string chunkPath = ChunkSerializationUtility.PathForChunk(chunkX, chunkY);
        string saveFileLocation = FileName(chunkName, chunkPath);

        if (!File.Exists(saveFileLocation))
            Debug.LogError("File at " + saveFileLocation + " does not exist");
        FileStream stream = new FileStream(saveFileLocation, FileMode.Open);

        object obj = formatter.Deserialize(stream);

        stream.Close();
        return (Chunk)obj;
    }

    public static object Load(string name, string path)
    {
        string saveFileLocation = FileName(name, path);

        if (!File.Exists(saveFileLocation))
            Debug.LogError("File at " + saveFileLocation + " does not exist");
        FileStream stream = new FileStream(saveFileLocation, FileMode.Open);

        object obj = formatter.Deserialize(stream);

        stream.Close();
        return obj;
    }


}