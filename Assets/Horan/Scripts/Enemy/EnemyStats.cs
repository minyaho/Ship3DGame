using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public event Action<float, float> UpdateHealthBarOnAttack;

    [Header("Enemy Parmaters")]
    [SerializeField]
    public float currentHealth = 100.0f;
    public float maxHealth;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public int explosionLifeTime = 3;

    // Provide to UI use
    public static int enemyLiftNumber = 0;

    // Start is called before the first frame update
    public void Start()
    {
        enemyLiftNumber += 1;
        maxHealth = currentHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        HealthHandler();
    }

    public void OnDamage(float attackValue)
    {
        currentHealth -= attackValue;
        UpdateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);
    }

    private void OnDestory()
    {
        Destroy(this.gameObject);

        // Exlosion effect
        GameObject exlosionEffect = Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(exlosionEffect, explosionLifeTime);

        enemyLiftNumber -= 1;
    }

    private void HealthHandler()
    {   
        // Debug.Log("Here");
        if(currentHealth <= 0) // If stats class' health var <= 0, destroy enemy object
        {     
            OnDestory();
        }
    }
}
