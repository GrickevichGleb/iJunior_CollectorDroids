using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawner : Spawner<Mineral>
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private List<SpawnArea> _spawnAreas;
    
    private bool _isSpawning = true;
    private Coroutine _spawningMineralsCoroutine;

    private Vector3 _spawnPosition;
    
    private void Start()
    {
        _spawningMineralsCoroutine = StartCoroutine(SpawnMineralsCoroutine(_spawnInterval));
    }

    protected override void ActionOnGet(Spawnable spawnable)
    {
        base.ActionOnGet(spawnable);

        spawnable.transform.position = _spawnPosition;
    }

    private IEnumerator SpawnMineralsCoroutine(float interval)
    {
        var delay = new WaitForSeconds(interval);

        while (_isSpawning)
        {
            yield return delay;

            SpawnMineral();
        }
    }
    
    private void SpawnMineral()
    {
        if (GetRandomSpawnArea().TryGetPoint(out _spawnPosition))
            Pool.Get();
        else
            Debug.Log("No spawn points available");
    }

    private SpawnArea GetRandomSpawnArea()
    {
        int index = UtilsRandom.GetRandomNumber(0, _spawnAreas.Count-1);

        return _spawnAreas[index];
    }
}
