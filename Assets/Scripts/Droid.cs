using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    private Mover _mover;
    private MineralPicker _mineralPicker;
    
    public DroidBase DroidBase {get; private set;}

    public Vector3 IdlePosition { get; private set; }
    public bool HasTask { get; private set; }
    public bool IsLoaded { get; private set; }

    public Mineral PickTarget { get; private set; }
    
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _mineralPicker = GetComponent<MineralPicker>();

        IdlePosition = transform.position;
    }

    public void AssignBase(DroidBase droidBase)
    {
        DroidBase = droidBase;
    }

    public void SetPickTarget(Mineral pickTarget)
    {
        HasTask = true;
        PickTarget = pickTarget;
        
        _mineralPicker.PickedMineral += OnPickedMineral;
    }

    private void OnPickedMineral(Mineral mineral)
    {
        _mineralPicker.PickedMineral -= OnPickedMineral;
        _mineralPicker.UnloadedMineral += OnUnloadedMineral;

        PickTarget = null;
        IsLoaded = true;
    }

    private void OnUnloadedMineral()
    {
        _mineralPicker.UnloadedMineral -= OnUnloadedMineral;

        IsLoaded = false;
        HasTask = false;
    }
}
