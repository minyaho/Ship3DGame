using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 420.0f;
    public int predictStepPerFrame = 6;
    public int lifeTime = 5;
    public float attack = 10;

    public Vector3 bulletVelocity;
    // Start is called before the first frame update
    void Start()
    {
        bulletVelocity = this.transform.forward * speed;
        StartCoroutine( LifeTimer() );
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point1 = this.transform.position;
        float stepSize = 1.0f / predictStepPerFrame;
        for(float step = 0; step < 1; step += stepSize)
        {
            bulletVelocity += Physics.gravity * stepSize * Time.deltaTime;
            Vector3 point2 = point1 + (bulletVelocity * stepSize  * Time.deltaTime);
            
            Ray ray = new Ray(point1, point2 - point1);
            if( Physics.Raycast(ray, (point2 - point1).magnitude ) )
            {
                // Debug.Log("Hit");
            }
            point1 = point2;
        }
        this.transform.position = point1;
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;

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
            EnemyStats enemy = collider.transform.parent.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.OnDamage(attack);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 point1 = this.transform.position;
        Vector3 predictedBulletVelocity = bulletVelocity;
        float stepSize = 0.01f;
        for(float step = 0; step < 1; step += stepSize)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + (predictedBulletVelocity * stepSize);
            Gizmos.DrawLine(point1, point2);
            point1 = point2;
        }
    }


    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy( this.gameObject );
    }
}
