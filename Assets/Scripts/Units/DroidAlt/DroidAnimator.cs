using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidAnimator : MonoBehaviour
{
    [SerializeField] private float _pickSpeed = 0.2f;

    public bool IsLifting { get; private set; } = false;

    private Transform _pickable;
    private Transform _carryPoint;
    
    public void PlayLift(Transform pickable, Transform carryPoint)
    {
        _pickable = pickable;
        _carryPoint = carryPoint;

        IsLifting = true;

        StartCoroutine(PickAnimation());
        
    }

    private IEnumerator PickAnimation()
    {
        float step;
        
        while (UtilsVector.IsEEqual(_pickable.position, _carryPoint.position) == false)
        {
            step = _pickSpeed * Time.deltaTime;
            _pickable.position = Vector3.MoveTowards(_pickable.position, _carryPoint.position, step);

            yield return null;
        }

        IsLifting = false;
    }
}
