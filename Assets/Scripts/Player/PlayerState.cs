using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : MonoBehaviour
{
    [Header("Player Parmaters")]
    [SerializeField] public float currentHealth = 200.0f;
    [SerializeField] public float maxHealth = 200.0f;
    [SerializeField] public bool canDamage = true;
    [SerializeField] public float gameEndTimer = 600.0f;
    [SerializeField] public bool timerUpdate = true;

    [Header("UI")]
    [SerializeField] private Transform _mainUI;
    [SerializeField] private Canvas _gameOverUI;
    [SerializeField] private GameObject _gameOverObject;

    public static float _gameEndTimer = 0.0f;

    private HelicopterController _helicopterController;
    // Start is called before the first frame update
    void Start()
    {
        _helicopterController = GetComponent<HelicopterController>();
        maxHealth = currentHealth;
        _gameEndTimer = gameEndTimer;
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
        TimerHandler();

        if(timerUpdate)
            _gameEndTimer -= Time.deltaTime;
    }

    public void OnDestory()
    {   
        // setEnemyInfoZero();
        _mainUI.gameObject.SetActive(false);
        // Show GameOverUI
        StartCoroutine(DelayDestory());
    }

    private IEnumerator DelayDestory()
    {
        yield return new WaitForSeconds(3);
        if(_gameOverUI != null)
            _gameOverUI.gameObject.SetActive(true);
        if(_gameOverObject != null)
            _gameOverObject.SetActive(true);
        Destroy(gameObject);
    }

    private void setEnemyInfoZero()
    {
        EnemyInfo.canUpdate = false;
        EnemyInfo.enemyLifeNumber = 0;
        EnemyInfo.enemyPlayerDestoryNumer = 0;
        EnemyInfo.turretLifeNumber = 0;

        EnemyInfo.destoryFlyBombNumber = 0;
        EnemyInfo.destoryWarBalloonNumber = 0;
        EnemyInfo.destoryTurretNumber = 0;
    }

    private void HealthHandler()
    {   
        // Debug.Log("Here");
        if(currentHealth <= 0) // If stats class' health var <= 0, destroy enemy object
        {     
            _helicopterController.SetCrash();
            timerUpdate = false;
        }
    }

    private void TimerHandler()
    {
        if(_gameEndTimer <= 0) // If stats class' health var <= 0, destroy enemy object
        {   
            OnDestory();
            gameObject.SetActive(false);
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
