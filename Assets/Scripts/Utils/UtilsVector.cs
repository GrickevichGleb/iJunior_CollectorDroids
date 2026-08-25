using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsVector
{
    private const float Tolerance = 0.1f;
    
    public static bool IsEEqual(Vector3 v1, Vector3 v2)
    {
        float sqrMagnitude = (v1 - v2).sqrMagnitude;

        if (sqrMagnitude < Mathf.Pow(Tolerance, 2f))
            return true;

        return false;
    }
}
