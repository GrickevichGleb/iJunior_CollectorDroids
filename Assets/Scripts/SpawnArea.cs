using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class SpawnArea : MonoBehaviour
{
    private List<Vector3> _possibleSpawnPoints = new List<Vector3>();
    private List<Vector3> _availableSpawnPoints = new List<Vector3>();
    
    private BoxCollider _boxCollider;
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        
        GetAllPossibleSpawnPoints();
    }

    public bool HasAvailableSpawnPoints()
    {
        UpdateAvailableSpawnPoints();

        if (_availableSpawnPoints.Count == 0)
            return false;

        return true;
    }

    public bool TryGetPoint(out Vector3 spawnPoint)
    {
        spawnPoint = Vector3.zero;
        UpdateAvailableSpawnPoints();

        if (_availableSpawnPoints.Count == 0)
            return false;

        int index = UtilsRandom.GetRandomNumber(0, _availableSpawnPoints.Count - 1);
        spawnPoint = _availableSpawnPoints[index];
        
        return true;
    }

    private void UpdateAvailableSpawnPoints()
    {
        RaycastHit[] hits;

        _availableSpawnPoints.Clear();
        
        foreach (Vector3 spawnPoint in _possibleSpawnPoints)
        {
            bool spawnPointOccupied = false;
            hits = Physics.RaycastAll(spawnPoint + new Vector3(0f, -0.5f, 0f), Vector3.up, 2f);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out Spawnable spawnable))
                    spawnPointOccupied = true;
            }
            
            if(spawnPointOccupied == false)
                _availableSpawnPoints.Add(spawnPoint);
        }

        Debug.Log("Available points");
        foreach (var point in _availableSpawnPoints)
        {
            Debug.Log(point);
        }
    }

    private void GetAllPossibleSpawnPoints()
    {
        float sizeX = Convert.ToInt32(_boxCollider.size.x);
        float sizeZ = Convert.ToInt32(_boxCollider.size.z);
        
        Vector3 startPoint = _boxCollider.center + new Vector3(-(sizeX / 2), 0f, -(sizeZ / 2));
        startPoint += new Vector3(0.5f, 0f, 0.5f);
        
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                Vector3 position = new Vector3( startPoint.x + i, startPoint.y, startPoint.z + j);
                Vector3 spawnPoint = transform.TransformPoint(position);

                _possibleSpawnPoints.Add(spawnPoint);
            }
        }
    }
}
