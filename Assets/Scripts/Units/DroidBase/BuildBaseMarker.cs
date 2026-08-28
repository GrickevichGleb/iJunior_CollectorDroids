using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBaseMarker : MonoBehaviour
{
    [SerializeField] private FlagMark _mark;

    public bool IsFlagSet { get; private set; } = false;

    public Transform MarkTransform => _mark.transform;
    
    public void SetMark(Vector3 position)
    {
        if(_mark.gameObject.activeInHierarchy == false)
            _mark.gameObject.SetActive(true);
        
        Vector3 towardsLevelCenter = Vector3.zero - position;
        Quaternion toLevelCenter = Quaternion.LookRotation(towardsLevelCenter, Vector3.up);
        
        _mark.transform.SetPositionAndRotation(position, toLevelCenter);

        IsFlagSet = true;
    }

    public void ResetMark()
    {
        _mark.gameObject.SetActive(false);
        _mark.transform.SetPositionAndRotation(transform.position, transform.rotation);
        
        IsFlagSet = false;
    }
}
