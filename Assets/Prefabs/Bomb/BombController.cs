using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Explosion")]
    public GameObject explosionPrefab;
    public int explosionLifeTime = 5;

    [Header("Parmaters")]
    public int predictStepPerFrame = 12;
    public int bomblifeTime = 30;
    public float attack = 100;

    [Header("Debug")]
    [SerializeField] public Vector3 bombVelocity;

    private AudioSource fallingSound;

    // Start is called before the first frame update
    void Start()
    {
        fallingSound = GetComponent<AudioSource>();
        StartCoroutine( BombLifeTimer() );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 point1 = this.transform.position;
        float stepSize = 0.1f / predictStepPerFrame;
        for(float step = 0; step < 1; step += stepSize)
        {
            bombVelocity += Physics.gravity * stepSize * Time.deltaTime;
            Vector3 point2 = point1 + (bombVelocity * stepSize * Time.deltaTime);
            point1 = point2;
        }
        this.transform.position = point1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 point1 = this.transform.position;
        Vector3 predictedBulletVelocity = bombVelocity;
        float stepSize = 1.0f / predictStepPerFrame;
        for(float step = 0; step < 10; step += stepSize)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + (predictedBulletVelocity * stepSize);
            Gizmos.DrawLine(point1, point2);
            point1 = point2;
        }
    }


    IEnumerator BombLifeTimer()
    {
        yield return new WaitForSeconds(bomblifeTime);
        Destroy( this.gameObject );
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnHitTheGround()
    {
        Destroy( this.gameObject );
        GameObject exlosionEffect = Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(exlosionEffect, explosionLifeTime);
    }



    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;
        // Debug.Log(collider.name);
        if( collider.CompareTag("Player") )
        {
            return;
        }
        if( collider.CompareTag("PlayerProjectile") )
        {
            return;
        }
        if( collider.CompareTag("Enemy") )
        {
           if (collider.transform.parent != null)
            {
                EnemyStats enemy = collider.transform.parent.GetComponent<EnemyStats>();
                enemy.OnDamageByPlayer(attack);
            }
            else
            {
                EnemyStats enemy = collider.transform.GetComponent<EnemyStats>();
                enemy.OnDamageByPlayer(attack);
            }
        }
        OnHitTheGround();
    }

}

