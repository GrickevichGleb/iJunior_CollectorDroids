using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidAlt : MonoBehaviour
{
    private MoverAlt _mover;
    private MineralPickerAlt _mineralPicker;
    private Coroutine _currentTaskCoroutine;
    
    public DroidBase DroidBase {get; private set;}

    public Vector3 IdlePosition { get; private set; }
    public bool HasTask { get; private set; }
    public bool IsLoaded { get; private set; }

    public Mineral PickTarget { get; private set; }
    
    private void Awake()
    {
        _mover = GetComponent<MoverAlt>();
        _mineralPicker = GetComponent<MineralPickerAlt>();

        IdlePosition = transform.position;
    }
    
    public void AssignBase(DroidBase droidBase)
    {
        DroidBase = droidBase;
    }

    public void SetTask(Mineral mineral)
    {
        if(_currentTaskCoroutine != null)
            StopCoroutine(_currentTaskCoroutine);
        
        HasTask = true;
        PickTarget = mineral;

        _currentTaskCoroutine = StartCoroutine(CollectMineral());
    }

    public IEnumerator CollectMineral()
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
}
