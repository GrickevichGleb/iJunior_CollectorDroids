using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBaseSpawner : Spawner<DroidBase>
{
    private Vector3 _spawnPoint;

    private Transform _initialTransform;
    private ColonyManager _colonyManager;
    private DroidBase _spawnedBase;

    public void SpawnBase(Droid droid, Transform initialTransform, ColonyManager colonyManager)
    {
        _initialTransform = initialTransform;
        _colonyManager = colonyManager;
        
        Pool.Get();
        _spawnedBase.RegisterDroid(droid);
    }

    protected override void ActionOnGet(DroidBase droidBase)
    {
        base.ActionOnGet(droidBase);
        droidBase.Initialize(_initialTransform, _colonyManager);

        _spawnedBase = droidBase;
    }
}
