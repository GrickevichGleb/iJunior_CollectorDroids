using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : Spawnable
{
    private Mover _mover;
    private MineralPicker _mineralPicker;
    private Coroutine _currentTaskCoroutine;
    
    private ColonyManager _colonyManager;

    public DroidBase DroidBase {get; private set;}
    public Vector3 IdlePosition { get; private set; }
    public bool HasTask { get; private set; }
    public bool IsLoaded { get; private set; }

    public Mineral PickTarget; // { get; private set; }

    public Transform FlagTransform; // { get; private set; }

    public event Action<Droid> BaseBuilt;
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _mineralPicker = GetComponent<MineralPicker>();

        IdlePosition = transform.position;
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }
    
    public void AssignBase(DroidBase droidBase)
    {
        DroidBase = droidBase;
        _colonyManager = DroidBase.ColonyManager;
    }

    public void SetTask(Mineral mineral)
    {
        if(_currentTaskCoroutine != null)
            StopCoroutine(_currentTaskCoroutine);
        
        HasTask = true;
        PickTarget = mineral;

        _currentTaskCoroutine = StartCoroutine(CollectMineral());
    }

    public void SetTaskBuildBase(Transform flagTransform)
    {
        if(_currentTaskCoroutine != null)
            StopCoroutine(_currentTaskCoroutine);

        FlagTransform = flagTransform;
        HasTask = true;

        _currentTaskCoroutine = StartCoroutine(BuildBase());
    }

    private IEnumerator CollectMineral()
    {
        yield return _mover.MoveToPosition(PickTarget.transform.position);
        yield return _mineralPicker.Pick(PickTarget);

        IsLoaded = true;

        yield return _mover.MoveToPosition(DroidBase.UnloadPoint.position);
        yield return _mineralPicker.Unload(PickTarget, DroidBase);

        HasTask = false;
        IsLoaded = false;
        PickTarget = null;
        
        yield return _mover.MoveToPosition(IdlePosition);
    }

    private IEnumerator BuildBase()
    {
        yield return _mover.MoveToPosition(FlagTransform.position);
        
        DroidBase.ColonyManager.DroidBaseSpawner.SpawnBase(this, FlagTransform, DroidBase.ColonyManager);
        IdlePosition = DroidBase.UnloadPoint.position;
        
        BaseBuilt?.Invoke(this);
        
        HasTask = false;
        IsLoaded = false;
        PickTarget = null;
        
        yield return _mover.MoveToPosition(IdlePosition);
    }

    public override void Reset()
    {
        if(_currentTaskCoroutine != null)
            StopCoroutine(_currentTaskCoroutine);
        
        DroidBase = null;
        IdlePosition = Vector3.zero;

        HasTask = false;
        IsLoaded = false;
        
        PickTarget = null;
    }
}
