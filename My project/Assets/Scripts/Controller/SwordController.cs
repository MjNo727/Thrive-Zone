using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Collider2D swordCollider;
    public float swordDamage = 10f;

    public void AttackRight()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        swordCollider.enabled = true;
    }

    public void AttackLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        swordCollider.enabled = true;
    }

    public void AttackUp()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90);
        swordCollider.enabled = true;
    }

    public void AttackDown()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);
        swordCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController spawnedEnemy = other.GetComponent<EnemyController>();
        BossController boss = other.GetComponent<BossController>();
        if (other.tag == "Enemy")
        {
            if(spawnedEnemy != null){
                spawnedEnemy.takeDamage(swordDamage);
            }
            if(boss != null){
                boss.takeDamage(swordDamage);
            }
        }
    }
    public void StopAttack()
    {
        swordCollider.enabled = false;
    }
}
