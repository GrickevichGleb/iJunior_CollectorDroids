using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBaseSpawner : Spawner<DroidBase>
{
    private Vector3 _spawnPoint;

    private Transform _initialTransform;
    private ColonyCenter _colonyCenter;

    public void SpawnBase(Droid droid, Transform initialTransform, ColonyCenter colonyCenter)
    {
        _initialTransform = initialTransform;
        _colonyCenter = colonyCenter;
        
        DroidBase spawnedBase = Pool.Get();
        spawnedBase.RegisterDroid(droid);
    }

    protected override void ActionOnGet(DroidBase droidBase)
    {
        base.ActionOnGet(droidBase);
        droidBase.Initialize(_initialTransform, _colonyCenter);
    }
}
