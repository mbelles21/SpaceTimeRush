using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement2 : MonoBehaviour
{
    private float activeMoveSpeed = 0f;
    public float moveSpeed = 5f;
    public float yMoveDist = 2f;
    public int loopNum = 3;
    private int loopCount = 0;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // adjust speed based on difficulty -- TODO: maybe adjust for this enemy?
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
        if (rb.position.y > 0)
        {
            FlipObject(); // to face other direction if on top of level
        }

        StartMoving();

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

    void StartMoving()
    {
        StartCoroutine(MovementPattern());
    }

    IEnumerator MovementPattern()
    {
        Vector2 startPos = rb.position;

        while (loopCount < loopNum)
        {
            if (startPos.y > 0)
            {
                // top of screen
                yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, rb.position.y - yMoveDist)));
                yield return StartCoroutine(MoveToPosition(new Vector2(-startPos.x, rb.position.y)));
                yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, rb.position.y + yMoveDist)));
                loopCount++;
                startPos = rb.position; // to change pos so it goes the other way
            }
            else if (startPos.y < 0)
            {
                // bottom of screen
                yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, rb.position.y + yMoveDist)));
                yield return StartCoroutine(MoveToPosition(new Vector2(-startPos.x, rb.position.y)));
                yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, rb.position.y - yMoveDist)));
                loopCount++;
                startPos = rb.position;
            }
        }

        Destroy(gameObject);
    }

    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        while (rb.position != targetPos)
        {
            rb.position = Vector2.MoveTowards(rb.position, targetPos, activeMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void FlipObject()
    {
        transform.Rotate(0f, 0f, 180f);
    }

    void OnDestroy()
    {
        SpeedController.OnTimeSpeedUp -= EnemyOnTimeSpeedUp;
    }
}
