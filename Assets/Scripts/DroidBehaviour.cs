using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBehaviour : MonoBehaviour
{
    private Droid _droid;
    private Mover _mover;
    private MineralPicker _mineralPicker;
    
    private bool _isOnTheWay;

    private void Awake()
    {
        _droid = GetComponent<Droid>();
        _mover = GetComponent<Mover>();
        _mineralPicker = GetComponent<MineralPicker>();
    }

    private void Update()
    {
        if (_droid.HasTask == false)
            return;

        if (_droid.PickTarget != null && _droid.IsLoaded == false) 
        {
            MoveTo(_droid.PickTarget.transform.position);
        }

        if (_droid.IsLoaded == true)
        {
            Debug.Log("Move to unload");
            MoveTo(_droid.DroidBase.UnloadPoint.position);
        }
    }

    private void MoveTo(Vector3 position)
    {
        _mover.MoveToPosition(position);
        _mover.ReachedDestination += OnReachedDestination;

        _isOnTheWay = true;
    }

    private void OnReachedDestination()
    {
        _mover.ReachedDestination -= OnReachedDestination;
        _isOnTheWay = false;

        if (_droid.IsLoaded == false && _droid.PickTarget != null)
            _mineralPicker.TryPickMineral(_droid.PickTarget);
        else if (_droid.IsLoaded == true && _droid.PickTarget == null)
            if(_mineralPicker.TryUnloadMineral(_droid.DroidBase))
                _mover.MoveToPosition(_droid.IdlePosition);
    }
}
