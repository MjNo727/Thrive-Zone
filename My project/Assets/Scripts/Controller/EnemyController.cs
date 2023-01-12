using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
public class EnemyController : MonoBehaviour
{
    private GameObject target;
    PlayerController player;
    public int enemyLevel;
    public float enemyXp;
    public float XpMultiplier;
    public TextMeshProUGUI text;
    public AIPath aIPath;
    int currentWaypoint = 0;
    bool isDead = false;

    public Rigidbody2D rb;
    public float knockbackForce = 10f;
    public float knockbackForceUp = 2f;
    public float maxHealth = 50f;
    public float currentHealth;
    public float flashDuration;
    public int numOfFlashes;
    public SpriteRenderer sr;
    public Color flashColor;
    private Animator animator;
    [SerializeField]
    private GameObject floatingTextPrefab;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        player = target.GetComponent<PlayerController>();
        enemyLevel = Random.Range(1, player.level + 3);

        // Scale enemyXp by player XP
        enemyXp = Mathf.Round(enemyLevel * 6 * XpMultiplier);

        text.text = "<color=red>Level: " + enemyLevel + "</color> \n XP: " + enemyXp;

        if (enemyLevel == player.level)
            text.text = "<color=orange>Level: " + enemyLevel + "</color> \n XP: " + enemyXp;

        if (enemyLevel < player.level)
        {
            float multiplier = 1 + (player.level - enemyLevel) * 0.1f;
            text.text = "<color=green>Level: " + enemyLevel + "</color> \n XP: " + Mathf.Round(enemyXp * multiplier);
        }

        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (aIPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aIPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void takeDamage(float damage)
    {
        ShowDamage(damage.ToString());
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        Knockback();
        StartCoroutine(FlashCo());
        if (currentHealth <= 0)
        {
            isDead = true;
            Dead();
        }
        else
        {
            isDead = false;
        }
    }

    void Knockback()
    {
        Transform attacker = GetClosestDamageSource();
        Vector2 knockbackDirection = new Vector2(transform.position.x - attacker.transform.position.x, 0);
        rb.velocity = new Vector2(knockbackDirection.x, knockbackForceUp) * knockbackForce;
    }

    public Transform GetClosestDamageSource()
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag("Weapon");
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;
        foreach (GameObject go in DamageSources)
        {
            float currentDist = Vector3.Distance(transform.position, go.transform.position);
            if (currentDist < closestDistance)
            {
                closestDistance = currentDist;
                currentClosestDamageSource = go.transform;
            }
        }
        return currentClosestDamageSource;
    }

    void Dead()
    {
        //play sfx
        AudioManager.instance.PlaySFX("EnemyDie");
        // Die animation
        animator.SetBool("isDead", true);
        player.GainExpScalable(enemyXp, enemyLevel);
        StartCoroutine(DestroyCo());
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

    void ShowDamage(string text)
    {
        if (floatingTextPrefab)
        {
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }
}
