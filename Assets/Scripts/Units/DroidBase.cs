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
    [SerializeField] private MineralManager _mineralManager;
    
    [SerializeField] private float _scanAreaSize = 30f;

    [SerializeField] private Transform _unloadPoint;

    private IMineralProvider _mineralProvider;
    private MineralCounter _mineralCounter;
    
    private bool _isScanning = true;
    
    public Transform UnloadPoint => _unloadPoint;
    
    private void Start()
    {
        _mineralProvider = _mineralSpawner.GetComponent<IMineralProvider>();
        _mineralCounter = GetComponent<MineralCounter>();
        
        SetupAvailableDroids();
        StartCoroutine(ScanAvailableResourcesCoroutine());
    }

    public void CollectMineral(Mineral mineral)
    {
        mineral.Pick(_unloadPoint);
        mineral.Collect();
        
        _mineralCounter.AddMinerals();
        _mineralManager.ReportCollectedMineral(mineral);
    }

    private IEnumerator ScanAvailableResourcesCoroutine()
    {
        var delay = new WaitForSeconds(_scanInterval);

        while (_isScanning)
        {
            yield return delay;

            List<Mineral> allMinerals = _mineralProvider.GetAvailableMinerals(transform.position, _scanAreaSize);
            _mineralManager.ReportAvailableMinerals(allMinerals);
            
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
        foreach (var droid in _droids)
        {
            if(droid.HasTask == true)
                continue;

            if (_mineralManager.TryGetUnprocessedMinerals(out Mineral mineral))
                droid.SetPickTarget(mineral);
            else
                return;
        }
    }
}
