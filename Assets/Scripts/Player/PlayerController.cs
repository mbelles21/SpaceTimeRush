using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Camera playerCamera;

    Vector2 movement;
    Vector2 mousePos;   

    public float knockbackForce = 10f;
    public AudioClip hitSFX;
    
    private float activeMoveSpeed;
    public float boostSpeed = 10f;
    public float boostLength = 0.5f;
    public float boostCooldown = 1f;
    private float boostTimer;
    private float boostCoolTimer;
    public AudioClip boostSFX;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    public Color dashColor = Color.green;
    private float objectWidth;
    private float objectHeight;

    private PlayerStats playerStats;
    private SpeedController speedController;
    private AudioSource src;
    private LevelManager levelManager;

    public Vector3 startingPos;

    void Start()
    {
        transform.position = new Vector3(-25, 0, 0);

        rb = GetComponent<Rigidbody2D>();
        activeMoveSpeed = moveSpeed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        objectWidth = spriteRenderer.bounds.extents.x;
        objectHeight = spriteRenderer.bounds.extents.y;

        playerStats = GetComponent<PlayerStats>();
        speedController = GetComponent<SpeedController>();
        src = GetComponent<AudioSource>();
        levelManager = FindAnyObjectByType<LevelManager>();

        StartCoroutine(MoveToStart());
    }

    IEnumerator MoveToStart()
    {
        // disable shooting
        Shooting shooting = GetComponent<Shooting>();
        shooting.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("enemy"); // change layer so obj can pass thru the level bounds
        float entranceSpeed = moveSpeed * 2.5f;

        while (Vector3.Distance(transform.position, startingPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, entranceSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startingPos;
        gameObject.layer = LayerMask.NameToLayer("player");

        // enable shooting
        shooting.enabled = true; // TODO: re-enable when level countdown reaches 0
    }

    // Update is called once per frame
    void Update()
    {
        if(levelManager.GetTimerRunning()) {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            // for boosting
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) {
                if(boostCoolTimer <= 0 && boostTimer <= 0) {
                    spriteRenderer.color = dashColor;
                    activeMoveSpeed = boostSpeed;
                    boostTimer = boostLength;
                    int newLayer = LayerMask.NameToLayer("boost"); // change layer to avoid enemy collision
                    gameObject.layer = newLayer;

                    src.clip = boostSFX;
                    src.Play();
                }
            }

            if(boostTimer > 0) {
                boostTimer -= Time.deltaTime;
                if(boostTimer <= 0) {
                    spriteRenderer.color = GetCurrentColor();
                    activeMoveSpeed = moveSpeed;
                    boostCoolTimer = boostCooldown;
                    int newLayer = LayerMask.NameToLayer("player"); // reset layer
                    gameObject.layer = newLayer;
                }
            }

            if(boostCoolTimer > 0) {
                boostCoolTimer -= Time.deltaTime;
            }
        }
    }

    // used for physics updates
    void FixedUpdate() 
    {
        Vector2 newPos = rb.position + movement * activeMoveSpeed * Time.deltaTime;

        // restrict movement 
        Vector2 min = playerCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = playerCamera.ViewportToWorldPoint(new Vector2(1, 1));

        newPos.x = Mathf.Clamp(newPos.x, min.x + objectWidth, max.x - objectWidth);
        newPos.y = Mathf.Clamp(newPos.y, min.y + objectHeight, max.y - objectHeight);

        rb.MovePosition(newPos);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; // offset by 90 bc it needs to be
        rb.rotation = angle;
    }


    void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("boss")) {
            Debug.Log("collided with enemy");
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            Vector2 force = knockbackDir * knockbackForce;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, knockbackDir, knockbackForce, LayerMask.GetMask("wall"));
            if(hit.collider != null) {
                knockbackDir = Vector2.Reflect(knockbackDir, hit.normal); // reflect dir if hitting a wall
                force = knockbackDir * knockbackForce;
            }

            rb.AddForce(force, ForceMode2D.Force);
            // Debug.Log("Knockback applied: " + force);

            playerStats.Damage(1);
            src.clip = hitSFX;
            src.Play();

            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if(e != null) {
                e.Damage(1);
            } else {
                Debug.LogWarning("missing enemy component");
            }
        }
        else {
            Debug.Log("collided with " + collision.gameObject.name);
        }

        // so knockback applies when player collides with boss laser beam
        if(collision.gameObject.CompareTag("bullet") && collision.gameObject.GetComponent<Bullet>().IsBulletLaser()) {
            Debug.Log("collided with laser beam");
            Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
            Vector2 force = knockbackDir * knockbackForce;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, knockbackDir, knockbackForce, LayerMask.GetMask("wall"));
            if(hit.collider != null) {
                knockbackDir = Vector2.Reflect(knockbackDir, hit.normal); // reflect dir if hitting a wall
                force = knockbackDir * knockbackForce;
            }

            rb.AddForce(force, ForceMode2D.Force);
        }
    }

    Color GetCurrentColor()
    {
        if(SpeedController.CurrentSpeed == 0) {
            return defaultColor;
        }
        else if(SpeedController.CurrentSpeed == 1) {
            return speedController.slowedColor;
        }
        else {
            return speedController.fastColor;
        }
    }
}