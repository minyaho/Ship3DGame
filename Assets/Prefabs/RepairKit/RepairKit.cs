using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _comsumeEffect;
    [Header("Parameters")] 
    [SerializeField] public float HealValue = 50.0f;
    [SerializeField] private float _effectTime = 2.0f;
    [SerializeField] private float _rotateSpeed = 1.0f;
    [SerializeField] private float _positonY;
    // Start is called before the first frame update

    private Animator _animatior;
    private bool onDestory = false;
    void Start()
    {
        _animatior = transform.GetChild(0).GetComponent<Animator>();
        _positonY = transform.position.y + 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( onDestory )
        {
            _rotateSpeed = Mathf.Lerp(_rotateSpeed, 6.0f, Time.fixedDeltaTime * 0.5f);
            transform.position = new Vector3( transform.position.x,  Mathf.Lerp(transform.position.y, _positonY,  Time.fixedDeltaTime * (_rotateSpeed / 30)), transform.position.z );
            _animatior.SetFloat("RotationSpeed", _rotateSpeed);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if( collider.tag.Equals("Player") )
        {
            collider.transform.GetComponent<PlayerState>().OnHealing( HealValue );
            GameObject effect = Instantiate(_comsumeEffect, transform.position, Quaternion.identity);
            Destroy(effect, _effectTime);
            Destroy(gameObject, 3);
            onDestory = true;
        }
    }
}
