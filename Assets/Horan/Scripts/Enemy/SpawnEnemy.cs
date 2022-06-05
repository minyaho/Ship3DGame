using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Parmaters")]
    [SerializeField]
    public GameObject spawnEnemyPrefab;
    [SerializeField]
    public enum UseMode {spawnWorldCenter=0, spawnNearPlayer=1}
    public UseMode useMode;
    [SerializeField]
    public enum GameMode {EnemyNotFindPlayer=0, EnemyFindPlayer=1}
    public GameMode gameMode;
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private int createMaxTime = 30;
    [SerializeField]
    private int createMinTime = 10;
    [SerializeField]
    private bool destroyByTimer = false;
    [SerializeField]
    private int destroyTime = 0;
    [SerializeField]
    private Vector3 spawnMax;
    [SerializeField]
    private Vector3 spawnMin;

    private float currentTimer;
    private Vector3 spawnPosition;
    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Random time
        currentTimer = Random.Range(createMinTime, createMaxTime);
    }

    // Update is called once per frame
    void Update()
    {   
        currentTimer -= Time.deltaTime;
        if(currentTimer <= 0)
        {
            if(playerObject != null){
                playerPosition = playerObject.transform.position;
            }
            else{
                playerPosition = new Vector3(0, 0, 0);
            }

            if(useMode == UseMode.spawnWorldCenter){
                float randomX = Random.Range(spawnMin.x, spawnMax.x);
                float randomY = Random.Range(spawnMin.y, spawnMax.y);
                float randomZ = Random.Range(spawnMin.z, spawnMax.z);
                spawnPosition = new Vector3(randomX, randomY, randomZ);
            }
            else if (useMode == UseMode.spawnNearPlayer)
            {
                float randomX = Random.Range(spawnMin.x, spawnMax.x);
                float randomY = Random.Range(spawnMin.y, spawnMax.y);
                float randomZ = Random.Range(spawnMin.z, spawnMax.z);
                spawnPosition = new Vector3(randomX + playerPosition.x, randomY + playerPosition.y, randomZ + playerPosition.z);
            }
            else
            {
                float randomX = Random.Range(spawnMin.x, spawnMax.x);
                float randomY = Random.Range(spawnMin.y, spawnMax.y);
                float randomZ = Random.Range(spawnMin.z, spawnMax.z);
                spawnPosition = new Vector3(randomX, randomY, randomZ);
            }
        
            currentTimer = Random.Range(createMinTime, createMaxTime);
            GameObject gameObject = Instantiate(spawnEnemyPrefab);
            gameObject.transform.position = spawnPosition;

            // Set FlyBomb
            if(gameObject.GetComponent<FlyBomb>() != null){
                FlyBomb flyBomb = gameObject.GetComponent<FlyBomb>();
                switch(gameMode){
                    case GameMode.EnemyFindPlayer:
                        flyBomb.gameMode = FlyBomb.GameMode.EnemyFindPlayer;
                        flyBomb.fallMode = FlyBomb.FallMode.EnemyNotFall;
                        break;
                    case GameMode.EnemyNotFindPlayer:
                        flyBomb.gameMode = FlyBomb.GameMode.EnemyNotFindPlayer;
                        flyBomb.fallMode = FlyBomb.FallMode.EnemyFallFromSky;
                        break;
                }
            }

            // Set Enemy
            if (gameObject.GetComponent<EnemyStats>() != null){
                EnemyStats enemy = gameObject.GetComponent<EnemyStats>();

                if(destroyByTimer)
                {
                    enemy.OnDestoryByTimer(destroyTime);
                }
            }
            else    // Set Other
            {
                Destroy(gameObject, destroyTime);
            }
        }
    }
}
