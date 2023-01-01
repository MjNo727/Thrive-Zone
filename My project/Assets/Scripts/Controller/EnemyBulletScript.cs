using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public float lifeTime;

    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null && player.currentHealth > 0)
        {
            player.Hurt(10);
            Destroy(gameObject);
        }
    }
}
