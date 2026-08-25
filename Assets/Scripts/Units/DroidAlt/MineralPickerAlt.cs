using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralPickerAlt : MonoBehaviour
{
    [SerializeField] private Transform _carryPoint;
    
    private DroidAlt _droid;
    private DroidAnimator _droidAnimator;

    private void Awake()
    {
        _droid = GetComponent<DroidAlt>();
        _droidAnimator = GetComponent<DroidAnimator>();
    }

    public IEnumerator Pick(Mineral mineral)
    {
        _droidAnimator.PlayLift(mineral.transform, _carryPoint);
        yield return new WaitWhile(() => _droidAnimator.IsLifting);
        mineral.Pick(_carryPoint);
    }

    public IEnumerator Unload(Mineral mineral, DroidBase droidBase)
    {
        if(_droid.IsLoaded == false)
            yield break;
        
        _droidAnimator.PlayLift(mineral.transform, droidBase.UnloadPoint);
        yield return new WaitWhile(() => _droidAnimator.IsLifting);

        droidBase.CollectMineral(mineral);
    }
}
