using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroidObs : MonoBehaviour
{
    private MoverObs _moverObs;
    private MineralPickerObs _mineralPickerObs;
    
    public DroidBase DroidBase {get; private set;}

    public Vector3 IdlePosition { get; private set; }
    public bool HasTask { get; private set; }
    public bool IsLoaded { get; private set; }

    public Mineral PickTarget { get; private set; }
    
    private void Awake()
    {
        _moverObs = GetComponent<MoverObs>();
        _mineralPickerObs = GetComponent<MineralPickerObs>();

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
        
        _mineralPickerObs.PickedMineral += OnPickedMineral;
    }

    private void OnPickedMineral(Mineral mineral)
    {
        _mineralPickerObs.PickedMineral -= OnPickedMineral;
        _mineralPickerObs.UnloadedMineral += OnUnloadedMineral;

        PickTarget = null;
        IsLoaded = true;
    }

    private void OnUnloadedMineral()
    {
        _mineralPickerObs.UnloadedMineral -= OnUnloadedMineral;

        IsLoaded = false;
        HasTask = false;
    }
}
