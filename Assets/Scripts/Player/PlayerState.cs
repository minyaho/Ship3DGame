using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HelicopterController))]
public class PlayerState : MonoBehaviour
{
    [Header("Player Parmaters")]
    [SerializeField] public float currentHealth = 200.0f;
    [SerializeField] public float maxHealth = 200.0f;

    [SerializeField] public bool canDamage = true;

    [Header("Explosion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _explosionLifeTime = 3;

    [Header("UI")]
    [SerializeField] private Transform _mainUI;
    [SerializeField] private Canvas _gameOverUI;

    private HelicopterController _helicopterController;
    // Start is called before the first frame update
    void Start()
    {
        _helicopterController = GetComponent<HelicopterController>();
        maxHealth = currentHealth;
    }

    public void OnDamage(float attackValue)
    {
        if(canDamage)
            currentHealth -= attackValue;
        // Upadate Player's Health Bar UI
        // Code Here
    }

    public void OnHealing(float value)
    {
        currentHealth = Mathf.Min(currentHealth + value, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        HealthHandler();
        DamageStateEffect();
    }

    private void OnDestory()
    {   
        _mainUI.gameObject.SetActive(false);
        // Show GameOverUI
        if(_gameOverUI != null)
            _gameOverUI.gameObject.SetActive(true);

        
        Destroy(this.gameObject);

        // Exlosion effect
        if (_explosionPrefab)
        {
            GameObject exlosionEffect = Instantiate(_explosionPrefab, this.transform.position, this.transform.rotation);
            Destroy(exlosionEffect, _explosionLifeTime);
        }
    }

    private void HealthHandler()
    {   
        // Debug.Log("Here");
        if(currentHealth <= 0) // If stats class' health var <= 0, destroy enemy object
        {     
            OnDestory();
        }
    }

    private void DamageStateEffect()
    {
        float half = (maxHealth / 2) ;
        if( currentHealth < half )
        {
            _helicopterController.SetSmokeRate( Mathf.RoundToInt((100 / half) * currentHealth) );
        }
    }
}
