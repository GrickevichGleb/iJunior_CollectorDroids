using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineral : Spawnable
{
    [SerializeField] private BoxCollider _boxCollider;

    public bool IsPickable { get; private set; } = true;
    public SpawnArea SpawnArea { get; private set; }
    public int SpawnPositionIndex { get; private set; }

    private Transform _initialParent;
    
    public event Action<Mineral> Picked; 

    private void Start()
    {
        _initialParent = transform.parent;
    }

    public void Initialize(SpawnArea spawnArea, int spawnPositionIndex, Vector3 spawnPosition)
    {
        SpawnArea = spawnArea;
        SpawnPositionIndex = spawnPositionIndex;
        
        transform.position = spawnPosition;
    }

    public Mineral Pick(Transform carryPosition)
    {
        if(IsPickable)
            Picked?.Invoke(this);
        
        IsPickable = false;
        
        gameObject.transform.SetParent(carryPosition);
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        return this;
    }

    public void Put(Transform putPosition)
    {
        gameObject.transform.SetParent(putPosition);
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public override void Reset()
    {
        base.Reset();

        IsPickable = true;
    }

    public void Collect()
    {
        Release();
    }
}
