using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public event Action<Spawnable> RequestRelease; 

    public virtual void Reset()
    {
        
    }

    public virtual void RemoteRelease()
    {
        Release();
    }

    protected virtual void Release()
    {
        RequestRelease?.Invoke(this);
    }
}
