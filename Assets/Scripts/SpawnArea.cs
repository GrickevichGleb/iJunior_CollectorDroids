using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class SpawnArea : MonoBehaviour
{
    private const int PositionAvailable = 0;
    private const int PositionTaken = 1;
    
    private List<Vector3> _allSpawnPositions = new List<Vector3>();
    private int[] _positionsStatus;
    
    private List<Vector3> _availableSpawnPoints;

    private HashSet<int> _allPointsIndexes = new HashSet<int>();
    private HashSet<int> _takenPointsIndexes = new HashSet<int>();

    private BoxCollider _boxCollider;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        
        GetAllSpawnPositions();
        InitializePositionsStatus();
    }
    
    public bool TryGetPoint(out Vector3 spawnPoint, out int spawnPointIndex)
    {
        spawnPoint = Vector3.zero;
        spawnPointIndex = 0;

        int[] availablePoints = GetAvailablePositionsIndexes();

        if (availablePoints.Length == 0)
            return false;

        int randomIndex = UtilsRandom.GetRandomNumber(0, availablePoints.Length - 1);
        int positionIndex = availablePoints[randomIndex];

        spawnPoint = _allSpawnPositions[positionIndex];
        spawnPointIndex = positionIndex;
        _positionsStatus[positionIndex] = PositionTaken;
        
        return true;
    }

    public void SetPositionAsAvailable(int spawnPositionIndex)
    {
        _positionsStatus[spawnPositionIndex] = PositionAvailable;
    }

    private void GetAllSpawnPositions()
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

                _allSpawnPositions.Add(spawnPoint);
            }
        }

        for (int i = 0; i < _allSpawnPositions.Count; i++)
        {
            _allPointsIndexes.Add(i);
        }
    }

    private void InitializePositionsStatus()
    {
        _positionsStatus = new int[_allSpawnPositions.Count];

        for (int index = 0; index < _allSpawnPositions.Count; index++)
        {
            _positionsStatus[index] = PositionAvailable;
        }
    }

    private int[] GetAvailablePositionsIndexes()
    {
        List<int> availableIndexes = new List<int>();

        for (int index = 0; index < _positionsStatus.Length; index++)
        {
            if(_positionsStatus[index] == PositionAvailable)
                availableIndexes.Add(index);
        }

        return availableIndexes.ToArray();
    }
    
}
