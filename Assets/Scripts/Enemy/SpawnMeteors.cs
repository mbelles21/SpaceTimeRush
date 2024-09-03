using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteors : MonoBehaviour
{
    [Header("Meteors")]
    public Transform meteorSpawn;
    public Transform topBound;
    public Transform bottomBound;
    public List<GameObject> meteorPrefabs;
    public float timeBetweenMeteors = 10f;
    public float meteorSpawnChance = 0.1f;
    private float meteorTimer;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GetComponent<LevelManager>();

        // meteorSpawnChance *= GameManager.GameDifficulty; // TODO: make spawn chance smaller 
        meteorTimer = timeBetweenMeteors;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelManager.timerRunning) {       
            // for meteors
            meteorTimer -= Time.deltaTime;
            if(meteorTimer <= 0) {
                float r = (GenerateRandomNumber(100) + 1) / 100;
                if(r <= meteorSpawnChance) {
                    SpawnMeteor();   
                }
                meteorTimer = timeBetweenMeteors;
            }
        }
    }

    int GenerateRandomNumber(int listSize) 
    {
        return Random.Range(0, listSize);
    }

    void SpawnMeteor()
    {
        float maxY = topBound.position.y - 2f;
        float minY = bottomBound.position.y + 2f;
        
        float posY = Random.Range(minY, maxY + 1);
        float posX = meteorSpawn.position.x;
        Vector3 spawnPos = new Vector3(posX, posY, 0f);

        int i = GetListIndex(meteorPrefabs.Count);
        GameObject meteorPrefab = meteorPrefabs[i];
        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }

    int GetListIndex(int listSize)
    {
        return Random.Range(0, listSize);
    }
}
