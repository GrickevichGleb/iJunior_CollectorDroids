using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsRandom
{
    private static System.Random s_random = new System.Random();
    
    public static int GetRandomNumber(int min, int max)
    {
        return s_random.Next(min, max + 1);
    }

}
