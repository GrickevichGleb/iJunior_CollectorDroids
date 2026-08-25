using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralPicker : MonoBehaviour
{
    private static readonly Vector3 PickRange = new Vector3(1f, 1f, 1f);

    [SerializeField] private Transform _carryPosition;

    private DroidAnimator _droidAnimator;
    private Mineral _pickedMineral;
    
    public event Action<Mineral> PickedMineral;
    public event Action UnloadedMineral;



    public bool TryPickMineral(Mineral pickTarget)
    {
        if (_pickedMineral != null)
            return false;
        
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, PickRange,
                                              Vector3.up, Quaternion.identity);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out Mineral mineral) && mineral.IsPickable && mineral == pickTarget)
            {
                _pickedMineral = mineral.Pick(_carryPosition);
                PickedMineral?.Invoke(_pickedMineral);
                
                return true;
            }
        }

        return false;
    }
    
    public bool TryUnloadMineral(DroidBase receiverDroidBase)
    {
        RaycastHit[] hits;

        hits = Physics.BoxCastAll(transform.position, PickRange,
            Vector3.up, Quaternion.identity);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out DroidBase droidBase) && droidBase == receiverDroidBase)
            {
                droidBase.CollectMineral(_pickedMineral);
                _pickedMineral = null;
                UnloadedMineral?.Invoke();
                
                return true;
            }
        }

        return false;
    }

}
