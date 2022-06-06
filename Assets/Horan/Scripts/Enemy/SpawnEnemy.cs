using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public enum UseMode {spawnWorldCenter=0, spawnNearPlayer=1}
    public enum GameMode {EnemyNotFindPlayer=0, EnemyFindPlayer=1}

    [Header("Spawn Prefab")]
    [SerializeField]
    public GameObject spawnEnemyPrefab;

    [Header("Mode setting")]
    [SerializeField]
    public UseMode useMode;
    [SerializeField]
    public GameMode gameMode;
    [SerializeField]
    private bool stopSpawn = false;

    [Header("Player Object")]
    [SerializeField]
    private GameObject playerObject;

    [Header("Spawn Parameter")]
    [SerializeField]
    private int createMaxTime = 30;
    [SerializeField]
    private int createMinTime = 10;
    [SerializeField]
    private bool destroyByTimer = false;
    [SerializeField]
    private int destroyTime = 0;

    [Header("Spawn Range")]
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
        if(playerObject == null)
        {
            Debug.Log("Please put the player object in script");
        }

        // Random time
        currentTimer = Random.Range(createMinTime, createMaxTime);
    }

    // Update is called once per frame
    void Update()
    {   
        if(!stopSpawn)
            currentTimer -= Time.deltaTime;

        if(currentTimer <= 0)
        {
            if(playerObject != null){
                playerPosition = playerObject.transform.position;
            }
            else{
                stopSpawn = true;
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
                        flyBomb.playerObject = playerObject;
                        flyBomb.gameMode = FlyBomb.GameMode.EnemyFindPlayer;
                        flyBomb.fallMode = FlyBomb.FallMode.EnemyFallFromSky;
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
