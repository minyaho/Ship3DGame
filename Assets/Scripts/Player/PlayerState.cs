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

    [Header("UI")]
    [SerializeField] private Transform _mainUI;
    [SerializeField] private Canvas _gameOverUI;
    [SerializeField] private GameObject _gameOverObject;

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
        {
            if(setting_stat.difficulty != null)
            {
                currentHealth -= attackValue*(1 + setting_stat.difficulty);
            }
            else
            {
                currentHealth -= attackValue;
            }
        }
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

    public void OnDestory()
    {   
        _mainUI.gameObject.SetActive(false);
        // Show GameOverUI
        if(_gameOverUI != null)
            _gameOverUI.gameObject.SetActive(true);
        if(_gameOverObject != null)
            _gameOverObject.SetActive(true);
    }

    private void HealthHandler()
    {   
        // Debug.Log("Here");
        if(currentHealth <= 0) // If stats class' health var <= 0, destroy enemy object
        {     
            _helicopterController.SetCrash();
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
