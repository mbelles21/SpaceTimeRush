using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public float targetY = 7f;
    public float targetX = 5f; // used as a distance not a coordinate
    
    public int loopNum = 3;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        // adjust speed based on difficulty
        if(GameManager.LevelDifficulty == 1) {
            moveSpeed -= 1f;
        }
        if(GameManager.LevelDifficulty == 3) {
            moveSpeed += 1f;
        }

        rb = GetComponent<Rigidbody2D>();
        StartMoving();
    }

    void Update()
    {
        LockOnTarget();
    }

    void LockOnTarget()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // offset by 90 bc gun facing up by default
        rb.rotation = angle;
    }

    void StartMoving()
    {
        StartCoroutine(MovementPattern());
    }

    IEnumerator MovementPattern()
    {
        Vector2 startPos = rb.position;
        int loopCount = 0;

        if(startPos.y > 0) {
            // top right
            if(startPos.x > 0) {
                while(loopCount < loopNum) {
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, -targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x - targetX, rb.position.y)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x + targetX, rb.position.y)));
                    loopCount++;
                }
            }
            // top left
            else if(startPos.x < 0) {
                while(loopCount < loopNum) {
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, -targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x + targetX, rb.position.y)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x - targetX, rb.position.y)));
                    loopCount++;
                }
            }
        }
        else if(startPos.y < 0) {
            // bottom right
            if(startPos.x > 0) {
                while(loopCount < loopNum) {
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x - targetX, rb.position.y)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, -targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x + targetX, rb.position.y)));
                    loopCount++;
                }
            }
            // bottom left
            else if(startPos.x < 0) {
                while(loopCount < loopNum) {
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x + targetX, rb.position.y)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x, -targetY)));
                    yield return StartCoroutine(MoveToPosition(new Vector2(rb.position.x - targetX, rb.position.y)));
                    loopCount++;
                }
            }
        }
        
        StartCoroutine(EndMovement());
    }

    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        while(rb.position != targetPos) {
            rb.position = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator EndMovement()
    {
        Debug.Log("movement loop ended");

        Vector2 direction = (rb.position.y < 0) ? Vector2.down : Vector2.up;
        float offScreenDistance = 10f; // distance to move off-screen
        float moveDuration = 2f; // duration to move off-screen

        float elapsedTime = 0f;
        Vector2 initialPosition = rb.position;

        while (elapsedTime < moveDuration)
        {
            rb.position = Vector2.Lerp(initialPosition, initialPosition + direction * offScreenDistance, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the enemy has moved completely off-screen
        rb.position = initialPosition + direction * offScreenDistance;
        Destroy(gameObject);
    }
}
