using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Rigidbody2D rb;

    public int damage = 10;
    public float moveForce = 20f;
    public AudioClip meteorSFX;
    private AudioSource src;


    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        src.clip = meteorSFX;
        src.Play();
        
        rb = GetComponent<Rigidbody2D>();
        // rb.AddForce(Vector2.left * moveForce, ForceMode2D.Impulse);

        // Destroy(gameObject, 4f);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.left * moveForce * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) {
            PlayerStats p = collision.gameObject.GetComponent<PlayerStats>();
            p.Damage(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("hit deleter");
        Destroy(gameObject);
    }
}
