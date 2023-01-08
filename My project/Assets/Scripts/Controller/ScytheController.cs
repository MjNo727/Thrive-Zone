using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheController : MonoBehaviour
{
    public Collider2D scytheCollider;
    public float scytheDamage = 20f;

    public void AttackRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        scytheCollider.enabled = true;
    }

    public void AttackLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        scytheCollider.enabled = true;
    }

    public void AttackUp()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        scytheCollider.enabled = true;
    }

    public void AttackDown()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);
        scytheCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController spawnedEnemy = other.GetComponent<EnemyController>();
        BossController boss = other.GetComponent<BossController>();
        if (other.tag == "Enemy")
        {
            if(spawnedEnemy != null){
                spawnedEnemy.takeDamage(scytheDamage);
            }
            if(boss != null){
                boss.takeDamage(scytheDamage);
            }
        }
    }
    public void StopAttack()
    {
        scytheCollider.enabled = false;
    }
}
