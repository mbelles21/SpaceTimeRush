using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 5;
    public static int DamageUpgrades = 0;
    private int playerDamage;
    public AudioClip bulletSFX;
    private AudioSource src;
    public bool isLaser; // for boss laser

    void Start() 
    {
        src = GetComponent<AudioSource>();
        src.clip = bulletSFX;
        src.Play();

        playerDamage = damage + DamageUpgrades;

        if(!isLaser) {
            Destroy(gameObject, 3f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        // ignore collision w/ other bullets
        if(collision.gameObject.tag == "bullet") {
            return;
        }

        if(collision.gameObject.tag == "enemy") {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if(PlayerStats.DamageDoubled) {
                int newDamage = playerDamage * 2;
                e.Damage(newDamage);
            }
            else {
                e.Damage(playerDamage);
            }
            // Debug.Log("bullet damage = " + damage);
        }
        else if(collision.gameObject.tag == "boss") {
            BossStats b = collision.gameObject.GetComponent<BossStats>();
            if(PlayerStats.DamageDoubled) {
                int newDamage = playerDamage * 2;
                b.Damage(newDamage);
            }
            else {
                b.Damage(playerDamage);
            }
        }
        else if(collision.gameObject.tag == "Player") {
            PlayerStats p = collision.gameObject.GetComponent<PlayerStats>();
            p.Damage(damage);
        }

        if(!isLaser) {
            Destroy(gameObject); // destroy bullet on impact
        }
    }

    public void DespawnLaser()
    {
        Destroy(gameObject, 1.8f); // TODO: move laser off screen then destroy (use coroutine probably)
    }

    public bool IsBulletLaser()
    {
        return isLaser;
    }
}
