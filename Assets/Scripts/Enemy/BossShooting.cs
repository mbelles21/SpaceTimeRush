using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public List<Transform> shootPoints;

    [Header("For Normal Shots")]
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float fireRate = 1f;
    private float fireInterval = 0f;
    public int fireChance = 30;

    [Header("For Homing Shots")]
    public GameObject homingShotPrefab;
    public float homingShotForce = 20f;
    public float homingShotRate = 1f;
    private float homingShotInterval = 0f;
    public int homingChance = 25;
    private Transform playerPos;
    private Vector3 shotTarget;

    [Header("For Laser")]
    public GameObject laserPrefab;
    public float laserSpeed = 20f;
    private bool canShoot;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        fireInterval = 4f; // so boss does not start shooting until level countdown done
        homingShotInterval = 7f; // to offset shot types
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        // for normal shots
        if (fireInterval <= 0f && canShoot) {
            int n = GenerateRandomNumber();
            if(n <= fireChance) {
                Shoot();
            }
            fireInterval = 1f / fireRate;
        }
        fireInterval -= Time.deltaTime;

        // for homing shots
        if(homingShotInterval <= 0f && canShoot) {
            int n = GenerateRandomNumber();
            if(n <= homingChance) {
                HomingShot();
            }
            homingShotInterval = 1f / homingShotRate;
        }
        homingShotInterval -= Time.deltaTime;
    }

    void Shoot()
    {
        for(int i = 0; i < 2; i++) {
            Transform t = shootPoints[i];
            GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    void HomingShot()
    {
        Transform t = shootPoints[2];

        shotTarget = playerPos.position;
        Vector2 shootDir = shotTarget - t.position;
        t.up = shootDir;
        
        GameObject bullet = Instantiate(homingShotPrefab, t.position, t.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
    }

    public void LaserBeam()
    {
        canShoot = false; // disable other shooting while firing laser

        Transform t = shootPoints[3];
        GameObject bullet = Instantiate(laserPrefab, t.position, t.rotation);
        
        Vector3 initialScale = bullet.transform.localScale;
        bullet.transform.localScale = new Vector3(initialScale.x, 0, initialScale.z); // start laser w/ 0 length

        StartCoroutine(ScaleLaser(bullet));
    }

    private IEnumerator ScaleLaser(GameObject bullet)
    {
        float timeElapsed = 0f;
        float targetScaleX = 15f; // determine final scale for laser (based on scene layout)

        while(timeElapsed < 1f) {
            timeElapsed += Time.deltaTime * laserSpeed;
            bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, Mathf.Lerp(0, targetScaleX, timeElapsed), bullet.transform.localScale.z);

            yield return null;
        }

        bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, targetScaleX, bullet.transform.localScale.z);
        bullet.GetComponent<Bullet>().DespawnLaser();
    }

    public void EnableShooting()
    {
        canShoot = true;
    }

    int GenerateRandomNumber() 
    {
        return Random.Range(1,101); // between 1 (inclusive) and 101 (exclusive)
    }
}
