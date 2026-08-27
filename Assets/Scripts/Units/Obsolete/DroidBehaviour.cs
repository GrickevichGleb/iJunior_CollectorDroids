using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBehaviour : MonoBehaviour
{
    private DroidObs _droidObs;
    private MoverObs _moverObs;
    private MineralPickerObs _mineralPickerObs;
    
    private bool _isOnTheWay;

    private void Awake()
    {
        _droidObs = GetComponent<DroidObs>();
        _moverObs = GetComponent<MoverObs>();
        _mineralPickerObs = GetComponent<MineralPickerObs>();
    }

    private void Update()
    {
        if (_droidObs.HasTask == false)
            return;

        if (_droidObs.PickTarget != null && _droidObs.IsLoaded == false) 
        {
            MoveTo(_droidObs.PickTarget.transform.position);
        }

        if (_droidObs.IsLoaded == true)
        {
            MoveTo(_droidObs.DroidBase.UnloadPoint.position);
        }
    }

    private void MoveTo(Vector3 position)
    {
        _moverObs.MoveToPosition(position);
        _moverObs.ReachedDestination += OnReachedDestination;

        _isOnTheWay = true;
    }

    private void OnReachedDestination()
    {
        _moverObs.ReachedDestination -= OnReachedDestination;
        _isOnTheWay = false;

        if (_droidObs.IsLoaded == false && _droidObs.PickTarget != null)
            _mineralPickerObs.TryPickMineral(_droidObs.PickTarget);
        else if (_droidObs.IsLoaded == true && _droidObs.PickTarget == null)
            if(_mineralPickerObs.TryUnloadMineral(_droidObs.DroidBase))
                _moverObs.MoveToPosition(_droidObs.IdlePosition);
    }
}
