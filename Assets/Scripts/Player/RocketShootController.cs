using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShootController : MonoBehaviour
{
    
    [SerializeField] private GameObject _rocketSpawnerParent;

    [SerializeField] private LockSystem _lockSystem;
    private RocketSpawner[] _rocketSpawnList;

    void Start()
    {
        _rocketSpawnList = _rocketSpawnerParent.GetComponentsInChildren<RocketSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.X ) )
        {
            foreach( RocketSpawner spawner in _rocketSpawnList )
            {
                if( spawner.ready )
                {
                    spawner.SetTarget( _lockSystem.GetTargetObject() );
                    spawner.Launch();
                    break;
                }
            }
        }
    }
}
