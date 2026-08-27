using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{
    [SerializeField] private MineralManager _mineralManager;
    [SerializeField] private DroidSpawner _droidSpawner;
    [SerializeField] private DroidBaseSpawner _droidBaseSpawner;

    public MineralManager MineralManager => _mineralManager;
    public DroidSpawner DroidSpawner => _droidSpawner;
    public DroidBaseSpawner DroidBaseSpawner => _droidBaseSpawner;
}
