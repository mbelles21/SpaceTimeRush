using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Basic Enemies")]
    public List<Transform> enemySpawns;
    public List<GameObject> enemyPrefabs;
    public List<int> enemiesPerWave; // indexes will match w/ enemyPrefabs list

    [Header("Other")]
    public float spawnRate = 5f;
    private float spawnInterval = 0f;

    public static int TotalEnemies;
    public int maxEnemies = 6;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GetComponent<LevelManager>();

        maxEnemies += GameManager.GameDifficulty;
        spawnRate -= 0.1f * GameManager.GameDifficulty;
        spawnInterval = 1f;
        TotalEnemies = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelManager.timerRunning) {
            // TODO: adjust detection for allowing enemies to spawn ?
            if(spawnInterval <= 0f && TotalEnemies < maxEnemies && !(TotalEnemies + 3 > maxEnemies)) {
                int index = GenerateRandomNumber(enemySpawns.Count);
                StartCoroutine(SpawnWave(index));

                spawnInterval = spawnRate;
            }

            // only decrement spawn interval if enemies are able to spawn
            if(TotalEnemies < maxEnemies && !(TotalEnemies + 3 > maxEnemies)){
                spawnInterval -= Time.deltaTime;
            }
        }
    }

    int GenerateRandomNumber(int listSize) 
    {
        return Random.Range(0, listSize);
    }

    IEnumerator SpawnWave(int index)
    {
        int eIndex = GenerateRandomNumber(enemyPrefabs.Count);
        GameObject enemy = enemyPrefabs[eIndex];
        int amountPerWave = enemiesPerWave[eIndex];

        for(int i = 0; i < amountPerWave; i++) {
            SpawnEnemy(enemy, index);
            yield return new WaitForSeconds(0.75f);
        }
    }

    void SpawnEnemy(GameObject enemy, int index) 
    {
        TotalEnemies++;
        Transform location = enemySpawns[index];
        Instantiate(enemy, location.position, location.rotation);
    }
}
