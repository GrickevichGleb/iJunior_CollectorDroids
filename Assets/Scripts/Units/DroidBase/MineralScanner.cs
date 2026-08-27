using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _mineralsLM;
    [SerializeField] private float _scanAreaSize = 30f;

    public List<Mineral> GetAvailableMinerals()
    {
        List<Mineral> availableMinerals = new List<Mineral>();
        Vector3 halfExtents = new Vector3(_scanAreaSize, _scanAreaSize, _scanAreaSize) / 2f;

        Collider[] colliders = 
            Physics.OverlapBox(transform.position, halfExtents, Quaternion.identity, _mineralsLM);
  
        foreach (var mineralCollider in colliders)
        {
            if(mineralCollider.TryGetComponent(out Mineral mineral))
                availableMinerals.Add(mineral);
        }
        
        return availableMinerals;
    }
}
