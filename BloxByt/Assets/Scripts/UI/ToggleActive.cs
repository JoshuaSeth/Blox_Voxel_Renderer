using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public MonoBehaviour toBeToggled;

    public void Toggle()
    {
        if (toBeToggled.enabled)
            toBeToggled.enabled = false;
        else
            toBeToggled.enabled = true;
    }
}
