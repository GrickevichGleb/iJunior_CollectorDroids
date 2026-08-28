using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControll : MonoBehaviour
{
    private const float MaxDistanceRaycast = 50f;
    private readonly Vector3 HightVectorRaycast = new Vector3(0f, 30f, 0f);
    
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private LayerMask _groundLM;
    
    private bool _isBaseSelected = false;

    private DroidBase _selectedBase;
    
    private void Start()
    {
        _inputReader.Clicked += OnClicked;
    }

    private void OnClicked(RaycastHit hit)
    {
        if (_isBaseSelected == false)
        {
            if(hit.collider.TryGetComponent(out _selectedBase))
            {
                _isBaseSelected = true;
                Debug.Log("Selected base: " + hit.collider.name);

                _selectedBase.NewBaseConstructionStarted += OnNewBaseConstructionStarted;
            }
                
        }
        else
        {
            Vector3 origin = hit.point + HightVectorRaycast;
            
            if(Physics.Raycast(origin, Vector3.down, MaxDistanceRaycast, _groundLM))
                _selectedBase.gameObject.GetComponent<BuildBaseMarker>().SetMark(hit.point);
        }
            
    }

    private void OnNewBaseConstructionStarted()
    {
        _selectedBase.NewBaseConstructionStarted -= OnNewBaseConstructionStarted;

        _isBaseSelected = false;
        _selectedBase = null;
    }
}
