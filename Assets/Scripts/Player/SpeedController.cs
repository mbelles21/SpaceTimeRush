using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    private bool isSlowed = false;
    private bool isFast = false;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    public Color slowedColor = Color.blue;
    public Color fastColor = Color.red;

    public float fastLimit = 2f;
    public static float FastLimitUp = 0f;
    public float slowLimit = 2f;
    public static float SlowLimitUp = 0f;
    public float cooldownPeriod = 2f;

    private bool fastCoolingDown = false;
    private bool slowCoolingDown = false;
    private float fastTimer = 0f;
    private float slowTimer = 0f;
    private float fastCooldownTimer = 0f;
    public static float FastCooldownReduction = 0f;
    private float slowCooldownTimer = 0f;
    public static float SlowCooldownReduction = 0f;
    private float timeFast = 0f;
    private float timeSlowed = 0f;
    private static float carryTimeF;
    private static float carryTimeS;
    private bool fastBarFilled = false;
    private bool slowBarFilled = false;

    public SpeedBar fastBar;
    public SpeedBar slowBar;

    public static int CurrentSpeed; // 0 = normal, 1 = slow, 2 = fast

    public AudioClip fastSFX;
    public AudioClip slowSFX;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();

        CurrentSpeed = 0;
        fastLimit += FastLimitUp;
        slowLimit += SlowLimitUp;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        fastBar.SetMaxSpeedTime(fastLimit);
        slowBar.SetMaxSpeedTime(slowLimit);
        carryTimeF = 0f;
        carryTimeS = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // time shenanigan controls
        if(Input.GetKeyDown(KeyCode.Q) && !slowCoolingDown) {
            ToggleSlow();
            slowBarFilled = true;
        }

        if(Input.GetKeyDown(KeyCode.E) && !fastCoolingDown) {
            ToggleFast();
            fastBarFilled = true;
        }

        if(isFast) {
            timeFast += Time.deltaTime * 0.5f; // adjusted to increment at normal rate
            fastTimer += Time.deltaTime * 0.5f;
            carryTimeF = fastTimer;

            fastBar.SetSpeedTime(fastLimit - fastTimer);

            if(fastTimer >= fastLimit) {
                ToggleFast();
                fastCoolingDown = true;
                fastCooldownTimer = 0f; // reset cooldown timer here
                CurrentSpeed = 0;
            }
        }
        if(isSlowed) {
            timeSlowed += Time.deltaTime * 1.5f; // adjusted to increment at normal rate
            slowTimer += Time.deltaTime * 1.5f;
            carryTimeS = slowTimer;

            slowBar.SetSpeedTime(slowLimit - slowTimer);

            if(slowTimer >= slowLimit) {
                ToggleSlow();
                slowCoolingDown = true;
                slowCooldownTimer = 0f;
                CurrentSpeed = 0;
            }
        }

        if(fastCoolingDown) {
            fastCooldownTimer += Time.deltaTime;
            // TODO: animate speed bar refilling

            if(fastCooldownTimer >= cooldownPeriod - FastCooldownReduction) {
                fastCoolingDown = false;
                fastBarFilled = false;
            }
        }
        if(slowCoolingDown) {
            slowCooldownTimer += Time.deltaTime;
            // TODO: animate speed bar refilling

            if(slowCooldownTimer >= cooldownPeriod - SlowCooldownReduction) {
                slowCoolingDown = false;
                slowBarFilled = false;
            }
        }

        if(!fastBarFilled) {
            fastBar.SetSpeedTime(fastLimit);
        }
        if(!slowBarFilled) {
            slowBar.SetSpeedTime(slowLimit);
        }

        // reduce timer when not in use
        if(!isFast && !fastCoolingDown && fastBarFilled) {
            carryTimeF -= Time.deltaTime;
            fastBar.SetSpeedTime(fastLimit - carryTimeF);
            if(carryTimeF < 0f) {
                carryTimeF = 0f;
                fastTimer = 0f;
            }
        }
        if(!isSlowed && !slowCoolingDown && slowBarFilled) {
            carryTimeS -= Time.deltaTime;
            slowBar.SetSpeedTime(slowLimit - carryTimeS);
            if(carryTimeS < 0f) {
                carryTimeS = 0f;
                slowTimer = 0f;
            }
        }

        if(!isFast && !isSlowed) {
            CurrentSpeed = 0; // ensure var is set to default when speed is default
        }
    }

    void ToggleSlow()
    {
        isSlowed = !isSlowed;
        isFast = false;
        slowTimer = 0f;

        if(isSlowed) {
            CurrentSpeed = 1;
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // update fixedDeltaTime so physics updates stay consistent
            spriteRenderer.color = slowedColor;
            src.clip = slowSFX;
            src.Play();
        } 
        else {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            spriteRenderer.color = defaultColor;
        }
    }

    void ToggleFast()
    {
        isFast = !isFast;
        isSlowed = false;
        fastTimer = 0f; // reset speed timer when toggling

        if(isFast) {
            CurrentSpeed = 2;
            Time.timeScale = 1.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // update fixedDeltaTime so physics updates stay consistent
            spriteRenderer.color = fastColor;
            src.clip = fastSFX;
            src.Play();
        } 
        else {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            spriteRenderer.color = defaultColor;
        }
    }

    public float GetSpeedTime()
    {
        return timeFast;
    }

    public float GetSlowTime()
    {
        return timeSlowed;
    }
}
