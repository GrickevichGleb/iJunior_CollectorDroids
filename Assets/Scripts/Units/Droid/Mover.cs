using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private float _moveSpeed = 2f;
    
    private Rigidbody _rigidbody;

    public IEnumerator MoveToPosition(Vector3 position)
    {
        yield return LookAt(position);
        yield return MoveTo(position);
    }

    public IEnumerator LookAt(Vector3 position)
    {
        bool rotating = true;
        
        Vector3 targetDirection =  position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized, Vector3.up);

        while (rotating)
        {
            transform.rotation = 
                Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.3f)
                yield break;
            
            yield return null;
        }
    }

    public IEnumerator MoveTo(Vector3 position)
    {
        float step;
        
        while (UtilsVector.IsEEqual(transform.position, position) == false)
        {
            step = _moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, position, step);
            
            yield return null;
        }
    }

}
