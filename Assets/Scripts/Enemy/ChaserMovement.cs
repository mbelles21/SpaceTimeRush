using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserMovement : MonoBehaviour
{
    private float activeMoveSpeed = 0f;
    public float moveSpeed = 5f;
    public float timeBetweenMovements = 3f;
    public int loopNum = 5;
    private int loopCount = 0;

    private Transform playerPos;
    private Vector3 targetPos;
    private Vector3 shootPos;
    private bool hasTargetPos;
    private bool isMoving;

    public AudioClip moveSFX;
    private AudioSource src;

    private Rigidbody2D rb;
    private EnemyShooting eShooting;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();

        // adjust speed based on difficulty
        if (GameManager.LevelDifficulty == 1)
        {
            moveSpeed -= 1f;
        }
        if (GameManager.LevelDifficulty == 3)
        {
            moveSpeed += 1f;
        }

        // check if time is fast for new enemies that spawn
        if (Enemy.IsTimeModified)
        {
            activeMoveSpeed = moveSpeed - Enemy.FastSpeedMod;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        rb = GetComponent<Rigidbody2D>();
        eShooting = GetComponent<EnemyShooting>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Chase());

        SpeedController.OnTimeSpeedUp += EnemyOnTimeSpeedUp;
    }

    void EnemyOnTimeSpeedUp(bool b)
    {
        if (b)
        {
            Enemy.IsTimeModified = true;
            activeMoveSpeed = moveSpeed - Enemy.FastSpeedMod;
        }
        else
        {
            Enemy.IsTimeModified = false;
            activeMoveSpeed = moveSpeed;
        }
    }

    IEnumerator Chase()
    {
        while (loopCount < loopNum)
        {
            if (!hasTargetPos)
            {
                // get player pos
                targetPos = playerPos.position;
                hasTargetPos = true;
            }

            if (isMoving)
            {
                // move to pos
                // src.clip = moveSFX;
                // src.Play();
                Vector2 direction = targetPos - transform.position;
                rb.position = Vector2.MoveTowards(rb.position, targetPos, activeMoveSpeed * Time.deltaTime);
                transform.up = direction; // look at target direction

                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    isMoving = false;
                    loopCount++;

                    // get another targetPos for shooting
                    shootPos = playerPos.position;
                    Vector2 shootDir = shootPos - transform.position;
                    transform.up = shootDir;
                    eShooting.Shoot();

                    yield return new WaitForSeconds(timeBetweenMovements);
                    hasTargetPos = false; // wait and reset targetPos
                }
            }
            else
            {
                isMoving = true;
            }

            yield return null;
        }

        // TODO: have it move off screen first
        Destroy(gameObject);
    }
    
    void OnDestroy()
    {
        SpeedController.OnTimeSpeedUp -= EnemyOnTimeSpeedUp;
    }
}
