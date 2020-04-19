using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Collection
{
    public List<object> AsList = new List<object>();
    public Dictionary<object, object> AsDict = new Dictionary<object, object>();
    public HashSet<object> AsHash = new HashSet<object>();
    Dictionary<object, object> deletionRequest;
    public int Count;

    public void Add(object key, object item)
    {
        AsList.Add(item);
        AsDict.Add(key, item);
        AsHash.Add(key);
        Count++;
    }

    public void Remove(object key)
    {
        deletionRequest.Remove(key);
        AsDict.Remove(key);
        AsHash.Remove(key);
        Count--;
    }

    public void StartDeletionActivity()
    {
       deletionRequest = AsList.ToDictionary(i => i);
    }

    public void FinishDeletionActivity() {
        AsList = deletionRequest.Select(i => i.Key).ToList();
    }



    public bool Contains(object key)
    {
        return AsHash.Contains(key);
    }

    public object Get(object key) {
        return AsDict[key];
    }

    public void Clear() {
        AsList.Clear();
        AsDict.Clear();
        AsHash.Clear();
    }


}
