using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralCounter : MonoBehaviour
{
    private int _mineralCount = 0;

    public event Action<int> CounterUpdated;
    
    public void AddMinerals(int quantity = 1)
    {
        _mineralCount += quantity;
        
        CounterUpdated?.Invoke(_mineralCount);
    }

    public bool TrySpend(int amount)
    {
        if (_mineralCount < amount)
            return false;

        _mineralCount -= amount;
        CounterUpdated?.Invoke(_mineralCount);
        
        return true;
    }
}
