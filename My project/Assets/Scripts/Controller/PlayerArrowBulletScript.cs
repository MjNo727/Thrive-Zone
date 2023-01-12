using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrowBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float arrowDamage = 7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        EnemyController enemy = collision.GetComponent<EnemyController>();
        BossController boss = collision.GetComponent<BossController>();
        if(enemy != null){
            enemy.takeDamage(arrowDamage);
            Destroy(gameObject);
        } 
        if (boss != null)
        {
            boss.takeDamage(arrowDamage);
            Destroy(gameObject);
        }
        Destroy(gameObject, 1.5f);
    }
}
