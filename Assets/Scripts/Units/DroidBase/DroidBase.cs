using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DroidBase : Spawnable
{
    [SerializeField] private float _scanInterval = 3f;
    [SerializeField] private int _droidCost = 3;
    [SerializeField] private int _droidBaseCost = 5;

    [SerializeField] private List<Droid> _droids;

    [SerializeField] private ColonyCenter colonyCenter;
    
    [SerializeField] private DroidSpawner _droidSpawner;
 
    [SerializeField] private Transform _unloadPoint;
    
    private MineralTracker _mineralTracker;
    private MineralCounter _mineralCounter;
    private MineralScanner _mineralScanner;

    private BuildBaseMarker _buildBaseMarker;
    
    private bool _isScanning = true;

    private Coroutine _scanning;
    
    public Transform UnloadPoint => _unloadPoint;

    public ColonyCenter ColonyCenter => colonyCenter;

    public event Action NewBaseConstructionStarted;

    private void Awake()
    {
        _mineralScanner = GetComponent<MineralScanner>();
        _mineralCounter = GetComponent<MineralCounter>();

        _buildBaseMarker = GetComponent<BuildBaseMarker>();
    }

    private void Start()
    {
        StartWork();
    }

    public void Initialize(Transform initialTransform, ColonyCenter colonyCenter)
    {
        transform.SetPositionAndRotation(initialTransform.position, initialTransform.rotation);

        this.colonyCenter = colonyCenter;
        
        StartWork();
    }

    public void RegisterDroid(Droid droid)
    {
        _droids.Add(droid);
        droid.AssignBase(this);
    }

    public void CollectMineral(Mineral mineral)
    {
        mineral.Pick(_unloadPoint);
        mineral.Collect();
        
        _mineralCounter.AddMinerals();
        _mineralTracker.ReportCollectedMineral(mineral);
        
        if(_buildBaseMarker.IsFlagSet && _droids.Count > 1)
            return;
        
        if(_mineralCounter.TrySpend(_droidCost))
            CreateDroid();
    }

    public override void Reset()
    {
        if(_scanning != null)
            StopCoroutine(ScanAvailableResourcesCoroutine());
        
        _buildBaseMarker.ResetMark();
        
        _droids.Clear();
    }

    private void StartWork()
    {
        _mineralTracker = colonyCenter.MineralTracker;
        _droidSpawner = colonyCenter.DroidSpawner;

        SetupAvailableDroids();

        _scanning = StartCoroutine(ScanAvailableResourcesCoroutine());
    }

    private void CreateDroid()
    {
        if (_droidSpawner.TrySpawnDroid(this, out Droid droid))
            RegisterDroid(droid);
    }

    private void BuildNewBase(Droid droid)
    {
        NewBaseConstructionStarted?.Invoke();
        
        droid.SetTaskBuildBase(_buildBaseMarker.MarkTransform);

        droid.BaseBuilt += OnBaseBuilt;
    }

    private IEnumerator ScanAvailableResourcesCoroutine()
    {
        var delay = new WaitForSeconds(_scanInterval);

        while (_isScanning)
        {
            yield return delay;
            
            List<Mineral> allMinerals = _mineralScanner.GetAvailableMinerals();
            _mineralTracker.ReportAvailableMinerals(allMinerals);
            
            AssignTasks();
        }
    }

    private void SetupAvailableDroids()
    {
        _droidSpawner.RegisterDroids(_droids);
        
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
            
            if (_buildBaseMarker.IsFlagSet && _droids.Count > 1)
            {
                if (_mineralCounter.TrySpend(_droidBaseCost))
                {
                    BuildNewBase(droid);
                    continue;
                }
            }
            
            if (_mineralTracker.TryGetUnprocessedMinerals(out Mineral mineral))
                droid.SetTaskCollectMineral(mineral);
            else
                return;
        }
    }

    private void OnBaseBuilt(Droid droid)
    {
        droid.BaseBuilt -= OnBaseBuilt;

        _droids.Remove(droid);
        _buildBaseMarker.ResetMark();
    }
}
