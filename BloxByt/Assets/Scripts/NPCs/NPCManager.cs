using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager active;
    public List<NPC> npcs = new List<NPC>();

    private void Awake()
    {
        active = this;
    }
}
