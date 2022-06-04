using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public event Action<float, float> UpdateHealthBarOnAttack; // HeathBarUI will use it.

    public float maxHealth; // Please don't set the value, it will auto set.

    [Header("Enemy Parmaters")]
    [SerializeField]
    public float currentHealth = 100.0f;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public int explosionLifeTime = 3;

    // Provide to UI use
    public static int enemyLiftNumber = 0;
    public static int enemyPlayerDestoryNumer = 0;

    public void Start()
    {
        enemyLiftNumber += 1;
        maxHealth = currentHealth;
    }

    public void Update()
    {
        HealthHandler(); // Handle the Health
    }

    public void OnDamageByPlayer(float attackValue) // If damage by player, please use it.

    {
        currentHealth -= attackValue;
        UpdateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);

        if(currentHealth <= 0){
            enemyPlayerDestoryNumer += 1;
        }
    }

    public void OnDamageBySelf(float attackValue) // If you want to destory the GameObject, please use it.
    {
        currentHealth -= attackValue;
        UpdateHealthBarOnAttack?.Invoke(currentHealth, maxHealth);
    }

    public void OnDestory()
    {
        if(explosionPrefab){
            GameObject exlosionEffect = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(exlosionEffect, explosionLifeTime);
        }
        Destroy(gameObject);

        enemyLiftNumber -= 1;
    }

    private void HealthHandler()
    {   
        if(currentHealth <= 0) // If stats class' health var <= 0, destroy enemy object
        {     
            OnDestory();
        }
    }
}


// If you want to create an new enemy, please inherit this class.
// In new enemy, it will like this template.
//
// public class EnemyName : MonoBehaviour
// {
//     public float attack = 10;
//     new void Start()
//     {
//         base.Start();
//         // Code below here
//     }

//     new void Update()
//     {
//         base.Update();
//         // Code below here
//     }

//     private void OnCollisionEnter(Collision collision)
//     {
//         GameObject collider = collision.gameObject;
//         if( collider.CompareTag("Player") )
//         {
//             PlayerState player = collider.GetComponent<PlayerState>();
//             if(player != null){
//                     player.OnDamage(attack);
//             }
//         }
//     }
// }
//
// If want to damage by player, please add "box collider" and "rigidbody" in your enemy object