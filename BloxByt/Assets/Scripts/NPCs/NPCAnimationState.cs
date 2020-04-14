using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCAnimationState
{
    public float timeSincePreviousFrame = 1;
    public float bodyXRot;
    public float bodyYRot;
    public float bodyZRot;
    public float rLegXRot;
    public float rLegYRot;
    public float rLegZRot;
    public float lLegXRot;
    public float lLegYRot;
    public float lLegZRot;
    public float rArmXRot;
    public float rArmYRot;
    public float rArmZRot;
    public float lArmXRot;
    public float lArmYRot;
    public float lArmZRot;
}
