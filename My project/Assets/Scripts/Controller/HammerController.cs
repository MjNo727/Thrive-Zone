using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public Collider2D hammerCollider;
    public float hammerDamage = 5f;

    public void AttackRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        hammerCollider.enabled = true;
    }

    public void AttackLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        hammerCollider.enabled = true;
    }

    public void AttackUp()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        hammerCollider.enabled = true;
    }

    public void AttackDown()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);
        hammerCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController spawnedEnemy = other.GetComponent<EnemyController>();
        BossController boss = other.GetComponent<BossController>();
        if (other.tag == "Enemy")
        {
            if(spawnedEnemy != null){
                spawnedEnemy.takeDamage(hammerDamage);
            }
            if(boss != null){
                boss.takeDamage(hammerDamage);
            }
        }
    }
    public void StopAttack()
    {
        hammerCollider.enabled = false;
    }
}
