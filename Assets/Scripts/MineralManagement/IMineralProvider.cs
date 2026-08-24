using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMineralProvider
{
    public List<Mineral> GetAvailableMinerals(Vector3 areaCenter, float areaSize);
}
