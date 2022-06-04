using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBomb : EnemyStats
{
    [Header("FlyBomb Parameters")]
    [SerializeField]
    private float attack = 100.0f;   // Hurt of projectile
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    public enum FallMode {EnemyNotFall=0, EnemyFallFromSky=1}
    public FallMode fallMode;
    [SerializeField]
    public enum GameMode {EnemyNotFindPlayer=0, EnemyFindPlayer=1}
    public GameMode gameMode;
    [SerializeField]
    private bool canAnythingDamage = true;
    [SerializeField]
    private bool onHitTheGround = true;


    private Vector3 position;
    private Rigidbody rigidBody;



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
            position = playerObject.transform.position - gameObject.transform.position;
            gameObject.transform.position =  Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
        }
        else if (gameMode == GameMode.EnemyNotFindPlayer){

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;

        if( collider.CompareTag("Player") )
        {
            
            OnDamageBySelf(currentHealth);

            PlayerState player = collider.GetComponent<PlayerState>();
            if(player != null){
                player.OnDamage(attack);
            }
            else{
                //Debug.Log("Can't find player!");
            }
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
