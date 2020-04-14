using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public enum Placement { Middle, Free, Wall};
    public Placement placement;

    public enum Category {None, Object, Food, Buildpart, Npc };
    public Category category;

    public int integrity;
    public int durability;
    public int weight;
    public int containerSize;
    public int foodValue;
    public int thirstValue;
    public int placementSpace;
    public int damageValue;
    public int nextStageAge;
    public Item nextStage;
    public bool canPickUp;

    public float yOffSet;
    public float xOffSet;
    public float zOffSet;
    public bool changeForRot;

    public Variation posibleForms;


    public List<MeshEntry> meshParts = new List<MeshEntry>();

    public enum Construction { Floor, Wall, Half_Wall, Ramp, Pillar, Pole, Fence, Gate, Wide_Gate, Window_Wall, Half_Ramp };
    public Construction construction;
}
