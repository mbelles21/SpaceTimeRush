using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int HealthBetweenLevels;
    public static float ShieldBetweenLevels;

    public static int AdditionalHealth = 0; // might need to change this
    public int totalHealth = 50;
    private int currentHealth = 0;
    public HealthBar healthBar;

    public static float AdditionalShield = 0f;
    public static float AdditionalReduction = 0f;
    public float totalShield = 10f;
    private float currentShield = 0f;
    public float damageReduction = 0.5f;
    public HealthBar shieldBar;
    private bool hasShield;

    private bool isInvincible = false;
    public float iFramesDuration = 1f;
    private float iFrameTimer;

    public static bool DamageDoubled = false;
    private float damageTimer = 5f;

    public AudioClip hitSFX;
    private AudioSource src;
    private LevelManager levelManager;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = FindAnyObjectByType<LevelManager>();

        if(GameManager.LevelCount == 1) {
            currentHealth = totalHealth;
            healthBar.SetMaxHealth(totalHealth);
            // Debug.Log("hp= " + currentHealth);

            currentShield = totalShield;
            shieldBar.SetMaxShield(totalShield);
            hasShield = true;
            // Debug.Log("shield= " + currentShield);

            // initialize static variables
            HealthBetweenLevels = currentHealth;
            ShieldBetweenLevels = currentShield;
        }
        else {
            currentHealth = HealthBetweenLevels;
            healthBar.SetCurrentHealth(currentHealth, totalHealth + AdditionalHealth);
            // Debug.Log("hp= " + currentHealth);

            currentShield = ShieldBetweenLevels;
            shieldBar.SetCurrentShield(currentShield, totalShield + AdditionalShield);
            if(currentShield > 0) {
                hasShield = true;
            }
            else {
                hasShield = false;
            }
            // Debug.Log("shield= " + currentShield);
        }

        damageReduction += AdditionalReduction; // add damage reduction upgrades

        DamageDoubled = false; // to make sure it's false on start
    }

    // Update is called once per frame
    void Update()
    {
        HealthBetweenLevels = currentHealth;
        ShieldBetweenLevels = currentShield;

        // Debug.Log("(in update) " + currentHealth);
        if(currentHealth <= 0) {
            Debug.Log("player died");
            spriteRenderer.enabled = false;
            levelManager.GameOver();
            // Destroy(gameObject);
        }

        if(currentShield > 0) {
            hasShield = true;
        }

        if(isInvincible) {
            iFrameTimer -= Time.deltaTime;
            if(iFrameTimer <= 0f) {
                isInvincible = false;
            }
        }

        if(DamageDoubled) {
            damageTimer -= Time.deltaTime;
            if(damageTimer <= 0f) {
                DamageDoubled = false;
                Debug.Log("damage reverted");
            }
        }
    }

    public void Damage(int damage)
    {
        if(!isInvincible) {
            // play sfx
            src.clip = hitSFX;
            src.Play();

            if(!hasShield) {
                Debug.Log("player damaged");
                currentHealth -= damage;

                // adjust health bar
                healthBar.SetHealth(currentHealth);

                isInvincible = true;
                iFrameTimer = iFramesDuration;
            }
            else {
                Debug.Log("shield hit");
                float newDamage = damage * damageReduction;
                if(newDamage <= currentShield) {
                    currentShield -= newDamage;
                    shieldBar.SetShield(currentShield);
                    if(currentShield == 0) {
                        hasShield = false;
                    }
                }
                else {
                    float dmgCarry = newDamage - currentShield;
                    int roundedDmg = Mathf.RoundToInt(dmgCarry/damageReduction);
                    shieldBar.SetShield(0);
                    hasShield = false;
                    currentHealth -= roundedDmg;
                    healthBar.SetHealth(currentHealth);
                }
            }
        }
        else {
            Debug.Log("iFrames");
        }
    }

    public void GainHealth(int healing)
    {
        currentHealth += healing;
        currentHealth = Mathf.Min(currentHealth, totalHealth + AdditionalHealth); // so cannot gain extra health
        healthBar.SetHealth(currentHealth);
    }

    public void GainShield(int healing)
    {
        currentShield += healing;
        currentShield = Mathf.Min(currentShield, totalShield + AdditionalShield); // so cannot gain extra shield
        shieldBar.SetShield(currentShield);
    }

    public void GainInvincibility(float duration)
    {
        isInvincible = true;
        iFrameTimer = duration; 
    }

    public void DoubleDamage(float duration)
    {
        Debug.Log("damage doubled");
        DamageDoubled = true;
        damageTimer = duration;
    }
}
