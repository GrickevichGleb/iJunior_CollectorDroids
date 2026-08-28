using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralTracker : MonoBehaviour
{
    [SerializeField] private MineralSpawner _mineralSpawner;

    private List<Mineral> _allMinerals = new List<Mineral>();
    private List<Mineral> _mineralsInProcess = new List<Mineral>();
    
    private Queue<Mineral> _mineralsAwaitProcess = new Queue<Mineral>();
    
    public void ReportAvailableMinerals(List<Mineral> availableMinerals)
    {
        _allMinerals = availableMinerals;

        foreach (var mineral in _allMinerals)
        {
            if(_mineralsInProcess.Contains(mineral) == false && _mineralsAwaitProcess.Contains(mineral) == false)
                _mineralsAwaitProcess.Enqueue(mineral);
        }
    }

    public void ReportCollectedMineral(Mineral mineral)
    {
        _mineralsInProcess.Remove(mineral);
    }

    public bool TryGetUnprocessedMinerals(out Mineral mineral)
    {
        mineral = null;
        
        if (_mineralsAwaitProcess.Count == 0)
            return false;

        mineral = _mineralsAwaitProcess.Dequeue();
        _mineralsInProcess.Add(mineral);
        
        return true;
    }
}
