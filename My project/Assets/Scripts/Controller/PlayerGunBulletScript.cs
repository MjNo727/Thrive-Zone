using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float gunDamage = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        BossController boss = collision.GetComponent<BossController>();
        if (enemy != null)
        {
            enemy.takeDamage(gunDamage);
            Destroy(gameObject);
        }
        if (boss != null)
        {
            boss.takeDamage(gunDamage);
            Destroy(gameObject);
        }
        Destroy(gameObject, 0.7f);
    }
}
