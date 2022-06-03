using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShootController : MonoBehaviour
{
    [SerializeField] private List<RocketSpawner> _rocketSpawnList = new List<RocketSpawner>();
    // Start is called before the first frame update
    void Start()
    {
 
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
                    spawner.Launch();
                    break;
                }
            }
        }
    }
}
