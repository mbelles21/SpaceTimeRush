using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public List<Transform> shootPoints;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float bulletAngle = 0f;
    public float fireRate = 1f;
    private float fireInterval = 0f;
    public int fireChance = 30;

    // Update is called once per frame
    void Update()
    {
        if (fireInterval <= 0f) {
            int n = GenerateRandomNumber();
            if(n <= fireChance) {
                Shoot();
            }
            fireInterval = 1f / fireRate;
        }
        fireInterval -= Time.deltaTime;
    }

    public void Shoot()
    {
        Quaternion angle = Quaternion.Euler(0, 0, bulletAngle);
        foreach(Transform t in shootPoints) {
            GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation * angle);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            angle = Quaternion.Inverse(angle);
        }
    }

    int GenerateRandomNumber() 
    {
        return Random.Range(1,101); // between 1 (inclusive) and 101 (exclusive)
    }
}
