using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombingController : MonoBehaviour
{
    [Header("Indicator")]
    public Transform indicator;
    public int maxScale = 20;

    [Header("Bombing")]
    public GameObject bomb;
    public Transform bombSpawn;
    public CoolDownTimer bombingTimer;

    private Rigidbody helicopter;
    // Start is called before the first frame update
    void Start()
    {
        bombingTimer = new CoolDownTimer( Bombing,  () =>  Input.GetKeyDown( KeyCode.Z ) );
        helicopter = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bombingTimer.Update();
    }

    void FixedUpdate()
    {
        SimulateTrajectory();
    }
    // 預判炸彈烙下的位置，然後設定標記點
    void SimulateTrajectory()
    {
        Vector3 point1 = bombSpawn.position;
        Vector3 predictedBulletVelocity =  new Vector3( helicopter.velocity.x , 0, helicopter.velocity.z );;
        float stepSize = 0.1f / 12;
        for(float step = 0; step < 10; step += stepSize)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + (predictedBulletVelocity * stepSize );
            Ray ray = new Ray(point1, point2 - point1);
            if( Physics.Raycast(ray, (point2 - point1).magnitude ) )
            {
                //Debug.Log("Hit");
               // OnHitTheGround();
                // indicator.position = point2;
                if( Mathf.Abs(helicopter.position.y - point2.y) < maxScale )
                {
                    indicator.position = new Vector3(point2.x, helicopter.position.y, point2.z);
                }
                else
                {
                    indicator.position = new Vector3(point2.x, maxScale, point2.z);
                }
            }
            
            point1 = point2;
        }
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Vector3 point1 = this.transform.position;
    //     Vector3 predictedBulletVelocity = new Vector3( helicopter.velocity.x, 0, helicopter.velocity.z );;
    //      float stepSize = 0.1f / 12;
    //     for(float step = 0; step < 10; step += stepSize)
    //     {
    //         predictedBulletVelocity += Physics.gravity * stepSize;
    //         Vector3 point2 = point1 + (predictedBulletVelocity * stepSize );
    //         Gizmos.DrawLine(point1, point2);
    //         point1 = point2;
    //     }
    // }


    private void Bombing()
    {
        GameObject newBomb = Instantiate(bomb, bombSpawn.position, bombSpawn.rotation );
        Vector3 velocity = new Vector3( helicopter.velocity.x, 0, helicopter.velocity.z );
        newBomb.GetComponent<BombController>().bombVelocity = velocity;
        newBomb.transform.Rotate(new Vector3(0, newBomb.GetComponent<Rigidbody>().velocity.magnitude, 0));
    }
}
