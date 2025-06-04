using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController2 : MonoBehaviour
{
    public float moveSpeed = 3f;

    private List<EnemyShooting> shootingComps = new List<EnemyShooting>();

    public Vector3 startingPos;
    private Vector2 moveDir;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyShooting eShoot in GetComponentsInChildren<EnemyShooting>())
        {
            shootingComps.Add(eShoot);
            eShoot.enabled = false;
        }
        // Debug.Log("eShoot count = " + shootingComps.Count);

        transform.position = new Vector3(25, 0, 0);

        moveDir = new Vector2(1, 1).normalized; // set up initial movement direction (should start by going up/left)

        StartCoroutine(MoveToStart());
    }

    IEnumerator MoveToStart()
    {
        float entranceSpeed = moveSpeed * 1.5f;

        while (Vector3.Distance(transform.position, startingPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPos, entranceSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startingPos;
        yield return new WaitForSeconds(0.5f);

        isActive = true;

        // turn shooting back on
        foreach (EnemyShooting eShoot in shootingComps)
        {
            eShoot.enabled = true;
        }
    }

    void Update()
    {
        if (isActive)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "vertical":
                moveDir.y *= -1;
                return;

            case "horizontal":
                moveDir.x *= -1;
                return;
        }
    }
}
