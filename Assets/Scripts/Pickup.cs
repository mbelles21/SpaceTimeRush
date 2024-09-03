using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // id will determine which pickup it is starting w/ 1
    // 1 == health, 2 == invincibility, 3 == damage x2, 4 == shield
    public int ID = 0;
    public float timeToDespawn = 5f;
    private float timer = 0f;
    public int scoreValue = 25;
    public AudioClip pickupSFX;
    // private AudioSource src;
    
    [Header("For Health/Shield")]
    public int minHealingAmount = 1;
    public int maxHealingAmount = 10;

    [Header("For Invincibility")]
    public float invDuration = 5f;

    [Header("For Damage")]
    public float dmgDuration = 5f;

    private ScoreManager scoreManager;

    void Start()
    {
        // src = GetComponent<AudioSource>();
        timer = timeToDespawn;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            Debug.Log(gameObject.name + " despawned");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player")) {
            PlayerStats p = collider.GetComponent<PlayerStats>();
            AudioSource src = collider.GetComponent<AudioSource>();
            src.clip = pickupSFX;
            src.Play();

            if(ID == 0) {
                Debug.Log("id not assigned");
            }

            if(ID == 1) {
                int healing = Random.Range(minHealingAmount, maxHealingAmount + 1);
                // Debug.Log("gained " + healing + " health");
                p.GainHealth(healing);
            }
            else if(ID == 2) {
                p.GainInvincibility(invDuration);
            }
            else if(ID == 3) {
                p.DoubleDamage(dmgDuration);
            }
            else if(ID == 4) {
                int healing = Random.Range(minHealingAmount, maxHealingAmount + 1);
                p.GainShield(healing);
            }

            // find score manager to add some score
            scoreManager = FindAnyObjectByType<ScoreManager>();
            scoreManager.PickupScore(scoreValue);

            Destroy(gameObject);
        }
    }
}
