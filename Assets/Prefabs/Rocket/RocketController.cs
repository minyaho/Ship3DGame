using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("REFERENCES")] 
    [SerializeField] private Rigidbody _rb;
    [HideInInspector] private GameObject _target;
    private Rigidbody _targetRB;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("BASIC")] 
    [SerializeField] private float _damage = 25.0f;

    [Header("MOVEMENT")] 
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("PREDICTION")] 
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")] 
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    [Header("LIFE TIME")]
    [SerializeField] private float _maxLifeTime = 10; 
    [SerializeField] private float _explosionLifeTime = 5;

    private void Start()
    {
        if( _target )
        {
            _targetRB = _target.GetComponent<Rigidbody>();
        }
        StartCoroutine(RocketLifeTimer());
    }
    private void FixedUpdate() {
        _rb.velocity = transform.forward * _speed;

        if( _target == null )
        {
            return;
        }
        float leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));
     
        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage) {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _targetRB.position + _targetRB.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage) {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        
        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket() {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject collider = collision.gameObject;

        
        if( collider.CompareTag("Player") )
        {
            return;
        }
        else if( collider.CompareTag("PlayerProjectile") )
        {
            return;
        }
        else if( collider.CompareTag("Enemy") )
        {
            EnemyStats enemy = collider.transform.parent.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.OnDamage(_damage);
            }
        }
        ExplodeEffect(transform);
        Destroy(gameObject);
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _standardPrediction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
    }

    public void SetTarget(GameObject gameObject)
    {
        if( gameObject != null )
        {
            _target = gameObject;
            _targetRB = _target.GetComponent<Rigidbody>();
        }
    }
    IEnumerator RocketLifeTimer()
    {
        yield return new WaitForSeconds(_maxLifeTime);
        Destroy( this.gameObject );
    }

    private void ExplodeEffect(Transform transform)
    {
        if(_explosionPrefab) {
            GameObject exlosionEffect = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(exlosionEffect, _explosionLifeTime);
        }
        if (transform.TryGetComponent<IExplode>(out var ex)) ex.Explode();
    }
}
