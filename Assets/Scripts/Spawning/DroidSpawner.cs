using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidSpawner : Spawner<Droid>
{
    private DroidBase _spawnBase;
    private Transform _spawnPoint;

    protected override void ActionOnGet(Droid droid)
    {
        base.ActionOnGet(droid);
        droid.Initialize(_spawnPoint.position);
    }

    public bool TrySpawnDroid(DroidBase droidBase, out Droid droid)
    {
        _spawnPoint = droidBase.UnloadPoint;

        droid = Pool.Get();
        
        return true;
    }
    
    public void RegisterDroids(List<Droid> availableDroids)
    {
        foreach (var droid in availableDroids)
        {
            if (ActiveObjects.Contains(droid))
                continue;
            
            droid.RequestRelease += OnRequestRelease;
            ActiveObjects.Add(droid);
        }
    }
}
