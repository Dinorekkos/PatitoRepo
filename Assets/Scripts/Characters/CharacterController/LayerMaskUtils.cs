using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskUtils
{
    public static bool CompareToInt(this LayerMask layerMask, int value)
    {
        if (((1 << value) & layerMask) != 0)
        {
            return true;
        }
        return false;
    }
}
