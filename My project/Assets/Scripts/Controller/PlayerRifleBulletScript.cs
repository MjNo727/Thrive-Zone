using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRifleBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rifleDamage = 20f;

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
            enemy.takeDamage(rifleDamage);
            Destroy(gameObject);
        }
        if (boss != null)
        {
            boss.takeDamage(rifleDamage);
            Destroy(gameObject);
        }
        Destroy(gameObject, 1.5f);
    }
}
