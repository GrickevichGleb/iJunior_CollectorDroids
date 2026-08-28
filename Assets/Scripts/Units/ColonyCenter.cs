using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyCenter : MonoBehaviour
{
    [SerializeField] private MineralTracker mineralTracker;
    [SerializeField] private DroidSpawner _droidSpawner;
    [SerializeField] private DroidBaseSpawner _droidBaseSpawner;

    public MineralTracker MineralTracker => mineralTracker;
    public DroidSpawner DroidSpawner => _droidSpawner;
    public DroidBaseSpawner DroidBaseSpawner => _droidBaseSpawner;
}
