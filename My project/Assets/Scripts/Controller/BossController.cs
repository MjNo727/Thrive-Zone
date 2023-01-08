using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossController : MonoBehaviour
{
    private GameObject target;
    public GameObject victoryMenuUI;
    PlayerController playerLevel;
    public TextMeshProUGUI text;
    public EnemyHealthbar healthbar;
    public GameObject healthBarUI, lvCanvas;
    Transform player;
    public Transform gunPoint;
    public Transform gun;
    private Animator animator;
    public GameObject BulletProjectile;
    private bool detectSoundPlayed = false;
    [SerializeField]
    private GameObject floatingTextPrefab;

    [Header("Stats")]
    public int enemyLevel = 20;
    public float maxHealth = 1000f;
    public float currentHealth;
    public float moveSpeed;

    [Header("Attack Pattern")]
    public float bossDamage = 5f;
    public float followPlayerRange;
    private bool inRange;
    public float attackRange;
    [Header("Flash")]
    public float flashDuration;
    public int numOfFlashes;
    public SpriteRenderer sr;
    public Color flashColor;
    [Header("Attack Interval")]
    public float startTimeBetweenShots;
    private float timeBetweenShots;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        playerLevel = target.GetComponent<PlayerController>();

        text.text = "Destronos\n<color=red>Level: " + enemyLevel;

        currentHealth = maxHealth;
        healthbar.SetHealth(currentHealth, maxHealth);
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //bullet follow
        Vector3 difference = player.position - gun.transform.position;
        float RotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, RotZ);

        if (player.transform.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        if (Vector2.Distance(transform.position, player.position) <= followPlayerRange && Vector2.Distance(transform.position, player.position) > attackRange)
        {
            inRange = true;
            animator.SetBool("isInCheckRange", true);
        }
        else
        {
            inRange = false;
            animator.SetBool("isInCheckRange", false);
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (timeBetweenShots <= 0)
            {
                Instantiate(BulletProjectile, gunPoint.position, gunPoint.transform.rotation);
                timeBetweenShots = startTimeBetweenShots;
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            ChangeStages();
        }
    }

    public void takeDamage(float damage)
    {
        ShowDamage(damage.ToString());
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth, maxHealth);

        animator.SetTrigger("Hit");
        StartCoroutine(FlashCo());
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void ChangeStages()
    {
        // 3 phases
        if (currentHealth <= maxHealth && currentHealth >= 0.7 * maxHealth)
        {
            if (detectSoundPlayed == false)
            {
                AudioManager.instance.PlayMusic("Boss");
                detectSoundPlayed = true;
            }
        }

        else if (currentHealth < 0.7 * maxHealth && currentHealth >= 0.4 * maxHealth)
        {
            if (detectSoundPlayed == false)
            {
                AudioManager.instance.PlayMusic("Boss");
                detectSoundPlayed = true;
            }
            bossDamage += 10f;
        }
        else if (currentHealth < 0.4 * maxHealth && currentHealth >= 0)
        {
            animator.SetBool("isBuffed", true);
            if (detectSoundPlayed == false)
            {
                AudioManager.instance.PlayMusic("Boss");

            }
            bossDamage += 20f;
        }
    }

    void Dead()
    {
        //play sfx
        AudioManager.instance.PlaySFX("RocksCrumble");
        // turn off health bar
        healthBarUI.SetActive(false);
        lvCanvas.SetActive(false);

        // Die animation
        animator.SetBool("isDead", true);

        //Victory Menu popup
        StartCoroutine(PlayVictoryUI(2f));

        // Disable boss
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // For seeing range in scene
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, followPlayerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null) // collide with player
        {
            player.Hurt(Random.Range(bossDamage - 1f, bossDamage + 3f));
        }
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

    private IEnumerator PlayVictoryUI(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.instance.PauseMusic("Theme");
        AudioManager.instance.PlaySFX("Victory");
        victoryMenuUI.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void ShowDamage(string text){
        if(floatingTextPrefab){
            GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
    }
}
