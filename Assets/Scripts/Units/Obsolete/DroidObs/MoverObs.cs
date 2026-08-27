using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoverObs : MonoBehaviour
{
    private const float ReachDistance = 0.3f;
    private const float RaycastDistance = 15f;
    
    private static readonly Vector3 RaycastHeightVector = new Vector3(0f, 10f, 0f);

    [SerializeField] private LayerMask _groundMask;

    private NavMeshAgent _navMeshAgent;
    
    private Vector3 _destination;
    private Vector3 _initialPosition;

    public bool IsMoving { get; private set; } = false;
    
    public event Action ReachedDestination; 

    private void Awake()
    {
        _initialPosition = GetPositionOnGround(transform.position);
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (IsMoving == false)
            return;
        
        if(HasReachedDestination())
            FinishMovement();
    }
    
    public void MoveToObject(GameObject gameObject)
    {
        //_destinationObject = gameObject;
        _destination = GetPositionOnGround(gameObject.transform.position);

        _navMeshAgent.destination = _destination;
        _navMeshAgent.isStopped = false;

        IsMoving = true;
    }

    public void MoveToPosition(Vector3 position)
    {
        _destination = GetPositionOnGround(position);
        
        _navMeshAgent.destination = _destination;
        _navMeshAgent.isStopped = false;

        IsMoving = true;
    }

    public void FinishMovement()
    {
        _navMeshAgent.isStopped = true;
        
        ReachedDestination?.Invoke();

        IsMoving = false;
    }

    private Vector3 GetPositionOnGround(Vector3 position)
    {
        if(Physics.Raycast(position + RaycastHeightVector, Vector3.down, 
               out RaycastHit hit, RaycastDistance, _groundMask))
        {
            return hit.point;
        }
        else
        {
            Debug.Log("Can't get destination point");
        }

        return _initialPosition;
    }
    
    private bool HasReachedDestination()
    {
        if (_navMeshAgent.remainingDistance <= ReachDistance)
            return true;

        return false;
    }
    
    
}
