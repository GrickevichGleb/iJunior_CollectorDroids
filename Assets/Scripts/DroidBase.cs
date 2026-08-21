using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DroidBase : MonoBehaviour
{
    [SerializeField] private float _scanInterval = 3f;
    
    [SerializeField] private List<Droid> _droids;
    [SerializeField] private MineralSpawner _mineralSpawner;

    [SerializeField] private Vector3 _scanAreaRadius = new Vector3(20f, 2f, 20f);

    [SerializeField] private Transform _unloadPoint;

    private MineralCounter _mineralCounter;
    
    private bool _isScanning = true;

    private List<Mineral> _allMinerals = new List<Mineral>();
    private List<Mineral> _mineralsInProcess = new List<Mineral>();
    
    private Queue<Mineral> _mineralsAwaitProcess = new Queue<Mineral>();

    public Transform UnloadPoint => _unloadPoint;
    
    private void Start()
    {
        _mineralCounter = GetComponent<MineralCounter>();
        
        SetupAvailableDroids();
        StartCoroutine(ScanAvailableResourcesCoroutine());
    }

    public void CollectMineral(Mineral mineral)
    {
        mineral.Pick(_unloadPoint);
        mineral.Collect();
        
        _mineralCounter.AddMinerals();
    }

    private IEnumerator ScanAvailableResourcesCoroutine()
    {
        var delay = new WaitForSeconds(_scanInterval);

        while (_isScanning)
        {
            yield return delay;

            _allMinerals = _mineralSpawner.GetActiveObjectsList();

            _mineralsAwaitProcess.Clear();
            
            foreach (var mineral in _allMinerals)
            {
                if(_mineralsInProcess.Contains(mineral))
                    continue;
                
                _mineralsAwaitProcess.Enqueue(mineral);
            }
            
            AssignTasks();
        }
    }

    private void SetupAvailableDroids()
    {
        foreach (var droid in _droids)
        {
            droid.AssignBase(this);
        }
    }

    private void AssignTasks()
    {
        if(_mineralsAwaitProcess.Count == 0)
            return;

        foreach (var droid in _droids)
        {
            if(droid.HasTask == true)
                continue;

            Mineral mineral = _mineralsAwaitProcess.Dequeue();
            _mineralsInProcess.Add(mineral);
            
            droid.SetPickTarget(mineral);
            
            if (_mineralsAwaitProcess.Count == 0)
                return;
        }
    }
}
