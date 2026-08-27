using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : Spawnable
{
    [SerializeField] protected T SpawnablePref;
    [Space] 
    [SerializeField] protected int PoolCapacity = 10;
    [SerializeField] protected int PoolMaxSize = 200;

    protected ObjectPool<T> Pool;
    protected List<T> ActiveObjects = new List<T>();

    public event Action<T> Spawned;
    
    private void Awake()
    {
        Pool = new ObjectPool<T>(
            () => Instantiate(SpawnablePref),
            actionOnGet: (spawnable) => ActionOnGet(spawnable),
            actionOnRelease: (spawnable) => spawnable.gameObject.SetActive(false),
            actionOnDestroy: (spawnable) => Destroy(spawnable.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize);
    }

    public virtual void ResetSpawner()
    {
        ReleaseAll();
    }

    protected virtual void ActionOnGet(T mineral)
    {
        mineral.Reset();
        mineral.RequestRelease += OnRequestRelease;
        
        ActiveObjects.Add((T)mineral);
        Spawned?.Invoke((T)mineral);
    }

    protected void OnRequestRelease(Spawnable spawnable)
    {
        ActiveObjects.Remove((T)spawnable);
        spawnable.RequestRelease -= OnRequestRelease;
        Pool.Release((T)spawnable);
    }
    
    private void ReleaseAll()
    {
        foreach (T spawnable in ActiveObjects)
        {
            spawnable.RequestRelease -= OnRequestRelease;
            Pool.Release(spawnable);
        }
        
        ActiveObjects.Clear();
    }
}
