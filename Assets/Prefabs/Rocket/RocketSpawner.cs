using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleTimer))]
public class RocketSpawner : MonoBehaviour
{   
    [Tooltip("Can be override by Rocket Prefab by himself")]
    [HideInInspector] private GameObject _target;
    [SerializeField] private GameObject _rocketObject;
    private SimpleTimer _spawnTimer;
    
    [SerializeField] public bool ready{get; set;} = false;

    private GameObject rocketModels;
    void Start()
    {   
        _spawnTimer = GetComponent<SimpleTimer>();
        _spawnTimer.Binding( ReadyToLanuch, () =>  ready == false );
        rocketModels = transform.Find("RocketModel").gameObject;
        rocketModels.SetActive(false);
    }

    void Update() {
    }

    public void SetTarget(GameObject gameObject)
    {
        _target = gameObject;
    }

    public void Launch()
    {
        if( ready )
        {
            ready = false;
            rocketModels.SetActive( ready );
            GameObject rocket = Instantiate(_rocketObject, transform.position, transform.rotation);
            RocketController rocketController = rocket.GetComponent<RocketController>();
            rocketController.SetTarget(_target);
        }
        else
        {

        }
    } 
    [SerializeField]
    private void ReadyToLanuch()
    {
        ready = true;
        rocketModels.SetActive( ready );
    }
}
