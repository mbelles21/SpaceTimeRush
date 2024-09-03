using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    private Rigidbody2D rb;
    private BossShooting bShooting;
    private bool isMoving;
    private float xCoord = 10f;
    private float yCoord = 20f;
    private Vector2 targetPos;
    private int loopCount;

    public Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(25, 0, 0);

        rb = GetComponent<Rigidbody2D>();
        bShooting = GetComponent<BossShooting>();
        loopCount = 0;

        xCoord = rb.position.x;
        float xVar = GetCoord(2f);
        int yDir = GetDirection();
        targetPos = new Vector2(rb.position.x + xVar, yCoord * yDir);
        // StartCoroutine(Hold(3f)); // so boss doesn't start moving until level countdown done
        StartCoroutine(MoveToStart());
    }

    IEnumerator MoveToStart()
    {
        float entranceSpeed = moveSpeed * 1.5f;

        while(Vector3.Distance(transform.position, startingPos) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, entranceSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startingPos;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Movement()); // note: might need to adjust timing if speeds are changed
    }

    // Update is called once per frame
    void Update()
    {
        if(loopCount >= 3) {
            loopCount = 0; // reset loop counter
            isMoving = false; // probably don't need 
            float fireCoord = GetCoord(5f);
            Debug.Log("fireCoord= " + fireCoord);
            targetPos = new Vector2(targetPos.x, fireCoord);
            isMoving = true; 
        }

        if(rb.position == targetPos) {
            isMoving = false;
            bShooting.LaserBeam();
            StartCoroutine(Hold(2f));
            float xVar = GetCoord(2f);
            if(transform.position.y > 0) {
                targetPos = new Vector2(xCoord + xVar, -yCoord); // reset y to unreachable point below
            }
            else {
                targetPos = new Vector2(xCoord + xVar, yCoord); // reset y to unreachable point above
            }
        }
    }

    IEnumerator Movement()
    {
        isMoving = true;
        bShooting.EnableShooting(); // enable shooting when moving
        while(isMoving) {
            rb.position = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Hold(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Movement());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        float newY = targetPos.y * -1;
        targetPos = new Vector2(targetPos.x, newY);
        loopCount++;
    }

    float GetCoord(float f) {
        return Random.Range(-f, f); // TODO: adjust values
    }

    int GetDirection() {
        int n = Random.Range(0, 2);
        if(n == 0) {
            return -1;
        }
        else {
            return 1;
        }
    }
}
