using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{   
    public int maxHealth = 100;
    private int currentHealth = 0;

    public int scoreValue = 1000;

    public AudioClip hitSFX;
    private AudioSource src;

    private bool bossDead;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        bossDead = false;
        levelManager = FindAnyObjectByType<LevelManager>();
        src = GetComponent<AudioSource>();
        currentHealth = maxHealth * GameManager.GameDifficulty;
        Debug.Log("game difficulty " + GameManager.GameDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 && !bossDead) {
            Debug.Log("boss defeated!");
            bossDead = true;
            // OnBossDied.Invoke(scoreValue); // this wouldn't work for some reason
            levelManager.BossDefeated(scoreValue);
            Destroy(gameObject);
            // StartCoroutine(Kill());
        }
    }

    public void Damage(int damage)
    {
        Debug.Log("boss damaged");
        src.clip = hitSFX;
        src.Play();
        currentHealth -= damage;
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
