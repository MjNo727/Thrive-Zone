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
    // public Transform target;
    public float speed = 50f;
    public float nextWaypointDistance = 3f;
    public SpriteRenderer enemyGFX;
    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
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

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }

    void FixedUpdate()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        if (force.x >= 0.01f)
        {
            enemyGFX.flipX = false;
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.flipX = true;
        }
        if (isDead)
        {
            force = Vector2.zero;
        }
        rb.AddForce(force);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void takeDamage(float damage)
    {
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
}
