using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float speed = 60.0f;    // Speed of projectile
    [SerializeField]
    private float attack = 10.0f;   // Hurt of projectile

    [SerializeField]
    public GameObject explosionPrefab;

    public Vector3 Direction { get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction * speed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {   
        GameObject collider = collision.gameObject;
        
        if( collider.CompareTag("Player") )
        {
            Destroy(gameObject);
            if (explosionPrefab != null){
                GameObject exlosionEffect = Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
                Destroy(exlosionEffect, 3);
            }

            PlayerState player = collider.GetComponent<PlayerState>();
            if(player != null){
                player.OnDamage(attack);
            }
            else{
                Debug.Log("Can't find player!");
            }
        }
    }
}
