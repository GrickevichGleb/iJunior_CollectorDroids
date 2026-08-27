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

    [SerializeField] private ColonyManager _colonyManager;
    
    [SerializeField] private MineralManager _mineralManager;
    [SerializeField] private DroidSpawner _droidSpawner;
    [SerializeField] private DroidBaseSpawner _droidBaseSpawner;
    
    [SerializeField] private Transform _unloadPoint;
    [SerializeField] private FlagMark _flag;
    
    private MineralCounter _mineralCounter;
    private MineralScanner _mineralScanner;
    
    private bool _isScanning = true;
    private bool _isFlagSet = false;

    private Coroutine _scanning;
    
    public Transform UnloadPoint => _unloadPoint;

    public ColonyManager ColonyManager => _colonyManager;

    public event Action NewBaseConstructionStarted;
    
    private void Start()
    {
        _mineralScanner = GetComponent<MineralScanner>();
        _mineralCounter = GetComponent<MineralCounter>();
        
        StartWork();
    }

    public void Initialize(Transform initialTransform, ColonyManager colonyManager)
    {
        transform.SetPositionAndRotation(initialTransform.position, initialTransform.rotation);

        _colonyManager = colonyManager;
        
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
        _mineralManager.ReportCollectedMineral(mineral);
        
        if(_isFlagSet)
            return;
        
        if(_mineralCounter.TrySpend(_droidCost))
            CreateDroid();
    }

    public void SetFlag(Vector3 position)
    {
        if(_flag.gameObject.activeInHierarchy == false)
            _flag.gameObject.SetActive(true);
        
        Vector3 towardsLevelCenter = Vector3.zero - position;
        Quaternion toLevelCenter = quaternion.LookRotation(towardsLevelCenter, Vector3.up);
        
        _flag.transform.SetPositionAndRotation(position, toLevelCenter);

        _isFlagSet = true;
    }

    private void ResetFlag()
    {
        _flag.gameObject.SetActive(false);
        
        _flag.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _isFlagSet = false;
    }
    
    public override void Reset()
    {
        if(_scanning != null)
            StopCoroutine(ScanAvailableResourcesCoroutine());
        
        _flag.gameObject.SetActive(false);
        
        _droids.Clear();
    }

    private void StartWork()
    {
        _mineralManager = _colonyManager.MineralManager;
        _droidSpawner = _colonyManager.DroidSpawner;
        _droidBaseSpawner = _colonyManager.DroidBaseSpawner;
        
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
        _isFlagSet = false;
        
        NewBaseConstructionStarted?.Invoke();
        
        droid.SetTaskBuildBase(_flag.transform);

        droid.BaseBuilt += OnBaseBuilt;
    }

    private IEnumerator ScanAvailableResourcesCoroutine()
    {
        var delay = new WaitForSeconds(_scanInterval);

        while (_isScanning)
        {
            yield return delay;
            
            List<Mineral> allMinerals = _mineralScanner.GetAvailableMinerals();
            _mineralManager.ReportAvailableMinerals(allMinerals);
            
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

            if (_isFlagSet)
            {
                if (_mineralCounter.TrySpend(_droidBaseCost))
                {
                    BuildNewBase(droid);
                    continue;
                }
            }
            
            if (_mineralManager.TryGetUnprocessedMinerals(out Mineral mineral))
                droid.SetTask(mineral);
            else
                return;
        }
    }

    private void OnBaseBuilt(Droid droid)
    {
        droid.BaseBuilt -= OnBaseBuilt;

        _droids.Remove(droid);
        ResetFlag();
    }
}
