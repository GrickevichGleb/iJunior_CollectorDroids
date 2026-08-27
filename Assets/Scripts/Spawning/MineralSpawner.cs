using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MineralSpawner : Spawner<Mineral>
{
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private List<SpawnArea> _spawnAreas;

    private bool _isSpawning = true;
    private Coroutine _spawningMineralsCoroutine;

    private SpawnArea _spawnArea;
    private Vector3 _spawnPosition;
    private int _spawnPositionIndex;
    
    private void Start()
    {
        _spawningMineralsCoroutine = StartCoroutine(SpawnMineralsCoroutine(_spawnInterval));
    }

    protected override void ActionOnGet(Mineral mineral)
    {
        base.ActionOnGet(mineral);

        mineral.Initialize( _spawnArea, _spawnPositionIndex, _spawnPosition);
        
        mineral.Picked += OnPicked;
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
        _spawnArea = GetRandomSpawnArea();
        
        if (_spawnArea.TryGetPoint(out _spawnPosition, out _spawnPositionIndex))
            Pool.Get();
    }

    private SpawnArea GetRandomSpawnArea()
    {
        int index = UtilsRandom.GetRandomNumber(0, _spawnAreas.Count-1);

        return _spawnAreas[index];
    }

    private void OnPicked(Mineral mineral)
    {
        mineral.Picked -= OnPicked;
        mineral.SpawnArea.SetPositionAsAvailable(mineral.SpawnPositionIndex);
    }
}
