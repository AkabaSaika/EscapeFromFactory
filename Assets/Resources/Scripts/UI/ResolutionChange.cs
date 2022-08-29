using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionChange : ScriptableObject
{
    public static void SetResolution(int index)
    {
        Debug.Log(index);
        switch(index)
        {
            case 0:
                Screen.SetResolution(800, 600, false);
                break;
            case 1:
                Screen.SetResolution(1024, 768, false);
                break;
            case 2:
                Screen.SetResolution(1600, 900, false);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, false);
                break;
        }
    }
}
