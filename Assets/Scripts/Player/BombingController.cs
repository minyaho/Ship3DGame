using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HelicopterController))]
public class BombingController : MonoBehaviour
{
    [Header("Indicator")]
    public Transform indicator;
    public int maxScale = 20;
    [SerializeField] private LayerMask _layerMask;

    [Header("Bombing")]
    public GameObject bomb;
    public Transform bombSpawn;
    
    [SerializeField] private SimpleTimer bombingTimer;

    private bool bombReady = false;
    private Rigidbody helicopter;
    public bool AllowUserControl {get; set;} = true;
    private HelicopterController _helicopterController;
    // Start is called before the first frame update
    void Start()
    {
        bombingTimer.Binding( () => bombReady = true,  () => (bombReady == false) );
        helicopter = GetComponent<Rigidbody>();
        _helicopterController = GetComponent<HelicopterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if( bombReady && Input.GetKeyDown( KeyCode.Z ) && AllowUserControl )
        {
            Bombing();
        }
    }

    void FixedUpdate()
    {
        if( _helicopterController.IsOnGround )
        {
            indicator.gameObject.SetActive(false);
        }
        else
        {
            SimulateTrajectory();
        }
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
            if( Physics.Raycast(ray, (point2 - point1).magnitude, _layerMask ) )
            {
                //Debug.Log("Hit");
               // OnHitTheGround();
                // indicator.position = point2;
                float distance = (helicopter.position.y - point2.y);
                                    Debug.Log(distance);

                if( distance < maxScale && distance > 5.0f )
                {
                    indicator.position = new Vector3(point2.x, point2.y + (helicopter.position.y - point2.y), point2.z);
                    indicator.gameObject.SetActive(true);
                }
                else if( distance > 10.0f && distance < helicopter.position.y )
                {
                    indicator.position = new Vector3(point2.x, maxScale, point2.z);
                    indicator.gameObject.SetActive(true);
                }
                else
                {
                    indicator.gameObject.SetActive(false);
                }
                break;
            }
            
            point1 = point2;
        }
    }
    // void OnDrawGizmos()
    // {
        
    //     Rigidbody h = ( helicopter == null ) ? GetComponent<Rigidbody>() : helicopter;
    //     Gizmos.color = Color.red;
    //     Vector3 point1 = this.transform.position;
    //     Vector3 predictedBulletVelocity = new Vector3( h.velocity.x, 0, h.velocity.z );;
    //      float stepSize = 0.1f / 12;
    //     for(float step = 0; step < 10; step += stepSize)
    //     {
    //         predictedBulletVelocity += Physics.gravity * stepSize;
    //         Vector3 point2 = point1 + (predictedBulletVelocity * stepSize );
    //         Gizmos.DrawLine(point1, point2);
    //         Ray ray = new Ray(point1, point2 - point1);
    //         if( Physics.Raycast(ray, (point2 - point1).magnitude, _layerMask ) )
    //         {
    //             // Debug.Log("Hit");
    //             // OnHitTheGround();
    //             // indicator.position = point2;
    //             Gizmos.DrawCube(new Vector3(point2.x, point2.y, point2.z), new Vector3(6, 6, 6));
    //             // if( helicopter.position.y - point2.y < maxScale )
    //             // {
    //             //     indicator.position = new Vector3(point2.x, helicopter.position.y, point2.z);
    //             // }
    //             // else
    //             // {
    //             //     indicator.position = new Vector3(point2.x, maxScale, point2.z);
    //             // }
    //         }
    //         point1 = point2;
    //     }
    // }


    private void Bombing()
    {
        GameObject newBomb = Instantiate(bomb, bombSpawn.position, bombSpawn.rotation );
        Vector3 velocity = new Vector3( helicopter.velocity.x, 0, helicopter.velocity.z );
        newBomb.GetComponent<BombController>().bombVelocity = velocity;
        newBomb.transform.Rotate(new Vector3(0, newBomb.GetComponent<Rigidbody>().velocity.magnitude, 0));
        bombReady = false;
    }
}
