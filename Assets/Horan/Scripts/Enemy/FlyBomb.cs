using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBomb : EnemyStats
{
    public enum FallMode {EnemyNotFall=0, EnemyFallFromSky=1}
    public enum GameMode {EnemyNotFindPlayer=0, EnemyFindPlayer=1}

    [Header("FlyBomb Parameters")]
    [SerializeField]
    private float attack = 100.0f;   // Hurt of projectile
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    public GameObject playerObject;

    [Header("Mode Parameters")]
    [SerializeField]
    public FallMode fallMode;
    [SerializeField]
    public GameMode gameMode;

    [Header("Destory Condition")]
    [SerializeField]
    private bool canAnythingDamage = true;
    [SerializeField]
    private bool onHitTheGround = true;

    [Header("Not Find Player Distance")]
    [SerializeField]
    public bool setDistance = true;
    [SerializeField]
    private float distanceRangeMax = 600.0f;
    [SerializeField]
    private float distanceRangeMin = 300.0f;

    [Header("Distance Monitor")]
    [SerializeField]
    private float distanceRandom = 0.0f;
    [SerializeField]
    private float distancePlayer = 0.0f;
    

    private Vector3 position;
    private Rigidbody rigidBody;
    // private float distancePlayer;
    // private float distanceRandom;



    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();

        if (playerObject == null){
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if(playerObject == null)
                gameMode = GameMode.EnemyNotFindPlayer;
        }

        if (canAnythingDamage == true)
        {
            currentHealth = 1;
        }

        distanceRandom = Random.Range(distanceRangeMin, distanceRangeMax);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (fallMode == FallMode.EnemyFallFromSky)
        {
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
        }
        else if (fallMode == FallMode.EnemyNotFall)
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = false;
        }
        if (gameMode == GameMode.EnemyFindPlayer){
            if(playerObject != null)
            {
                position = playerObject.transform.position - gameObject.transform.position;
                distancePlayer = Vector3.Distance(playerObject.transform.position, gameObject.transform.position);

                if(setDistance == true)
                {
                    
                    if (distancePlayer >= distanceRandom)
                    {
                        gameObject.transform.position =  Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
                    }
                    else
                    {
                        fallMode = FallMode.EnemyFallFromSky;
                    }
                }
                else
                {
                    gameObject.transform.position =  Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
                }
            }
        }
        else if (gameMode == GameMode.EnemyNotFindPlayer){

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;

        if( collider.CompareTag("Player") )
        {
            PlayerState player = collider.GetComponent<PlayerState>();
            if(player != null){
                player.OnDamage(attack);
            }
            else{
                //Debug.Log("Can't find player!");
            }

            OnDamageBySelf(currentHealth);
        }
        else if( collider.CompareTag("PlayerProjectile") )
        {
            // Debug.Log(collider.tag);
        }
        else if(collider.CompareTag("Terrain") )
        {
            if(onHitTheGround)
                OnDamageBySelf(currentHealth);
        }
        else if(canAnythingDamage){
            OnDamageBySelf(currentHealth);
        }
    }
}
