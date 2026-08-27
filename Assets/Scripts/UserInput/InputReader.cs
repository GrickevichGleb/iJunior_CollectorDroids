using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const float MaxDistanceRaycast = 100f;
    
    private Camera _camera;

    public event Action<RaycastHit> Clicked;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            MouseClick0();
    }

    private void MouseClick0()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, MaxDistanceRaycast))
        {
            Clicked?.Invoke(hit);
        }
            
    }
}
