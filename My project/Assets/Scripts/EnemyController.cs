using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    // private GameObject player;
    // PlayerController playerLevel;
    // public int enemyLevel;
    // public float enemyXp;
    // public float XpMultiplier;
    public float maxHealth = 50f;
    public float currentHealth;
    public float flashDuration;
    public int numOfFlashes;
    public SpriteRenderer sr;
    public Color flashColor;
    private Animator animator;

    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
        // playerLevel = player.GetComponent<PlayerController>();
        // enemyLevel = Random.Range(1, playerLevel.level + 2);

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        StartCoroutine(FlashCo());
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        // Die animation
        animator.SetBool("isDead", true);
        StartCoroutine(DestroyCo());
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        while (temp < numOfFlashes)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
    }

    private IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null && player.currentHealth > 0) // collide with player
        {
            player.Hurt(3);
        }
    }
}
