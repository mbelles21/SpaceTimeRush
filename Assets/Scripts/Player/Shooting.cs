using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float fireRate = 1f;
    public static float FireRateUp = 0f;
    private float fireInterval = 0f;

    void Start()
    {
        fireRate += FireRateUp;
    }

    // Update is called once per frame
    void Update()
    {
        // for player shooting
        if(gameObject.tag == "Player") {
            // Fire1 should be left mouse button
            if(Input.GetButtonDown("Fire1") && fireInterval <= 0f && !PauseMenu.GamePaused) {
                Shoot();
                fireInterval = 1f / fireRate;
            }
            fireInterval -= Time.deltaTime;
        }        
    }

    void Shoot() 
    {
        if(shootPoint != null) {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shootPoint.up * bulletForce, ForceMode2D.Impulse);
        }   
    }
}
