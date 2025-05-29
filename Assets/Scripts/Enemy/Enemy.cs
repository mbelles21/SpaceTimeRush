using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDied(int points, Transform t); // so it can talk to other scripts
    public static event EnemyDied OnEnemyDied; // ditto

    public static float FastSpeedMod = 0f; // here bc all enemies have this component
    public static bool IsTimeModified = false; // also here to help keep track of enemy speeds (mostly for when/if new ones spawn)

    public int MaxHealth = 10;
    private int totalHealth = 0;

    public int scoreValue = 100; 
    public bool isMeteor;

    public AudioClip hitSFX;
    private AudioSource src;

    private LevelManager levelManager;

    // enemy does not collide with walls bc of layers
    // see project settings -> physics 2d -> layer collision matrix

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        levelManager = FindAnyObjectByType<LevelManager>();
        // Debug.Log(gameManager);

        totalHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(totalHealth <= 0) {
            OnEnemyDied.Invoke(scoreValue, gameObject.transform);
            Destroy(gameObject);
        }

        if(!levelManager.timerRunning) {
            Destroy(gameObject);
        }
    }

    public void Damage(int damage)
    {
        // Debug.Log("enemy damaged");
        src.clip = hitSFX;
        src.Play();
        totalHealth -= damage;
    }

    void OnDestroy()
    {
        if(!isMeteor) {
            SpawnEnemies.TotalEnemies--;
        }
    }
}
