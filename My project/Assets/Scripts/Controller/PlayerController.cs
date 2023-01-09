using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerController : MonoBehaviour, IDataPersistance
{
    [Header("Controls")]
    [SerializeField]
    private Transform respawnLocation;
    private bool canMove = true;

    Animator animator;
    Vector2 movementInput;
    Vector2 lookDirection = new Vector2(0, -1);
    public float movementSpeed = 3f;
    private PlayerInputActions inputActions;
    Rigidbody2D rb;
    public float dashDistance = 100f;
    public float cooldownDashTime = 2f;
    public float cooldownGunTime = 1.5f;
    public float cooldownRifleTime = 1f;
    public float cooldownBowTime = 2f;
    float nextFire;
    private bool isDashing, isMeleeAttacking, isRangeAttacking;

    [Header("Healthbar")]
    public float maxHealth = 100f;
    public float currentHealth;
    private float lerpTimer;
    public float chipSpeed = 2f;
    public Image frontHealthbar, backHealthbar;
    public TextMeshProUGUI healthText;
    //public HealthBar healthBar;

    [Header("Expbar")]
    public int level;
    public int levelCap;
    public float currentXp;
    public float requiredXp;
    private float lerpExpTimer;
    private float delayTimer;
    public Image frontXpbar, backXpbar;
    public TextMeshProUGUI levelText, expText;

    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier = 300;
    [Range(2f, 4f)]
    public float powerMultiplier = 2;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7;

    [Header("IFrame")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer sr;

    [Header("VFX")]
    public ParticleSystem dust;
    public ParticleSystem levelUp, levelUpSplash;

    [Header("Attack")]
    public Transform swordHitbox;
    public Transform hammerHitbox;
    public Transform scytheHitbox;
    public Transform shootingPoint;
    public GameObject gunBulletPrefab, rifleBulletPrefab, arrowBulletPrefab;
    public float bulletForce = 300f;
    public SwordController swordController;
    public HammerController hammerController;
    public ScytheController scytheController;

    [Header("GameOverUI")]
    public GameObject gameOverUI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AudioManager.instance.PlayMusic("Theme");
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Fire1.performed += OnFire1;
        inputActions.Player.Fire2.performed += OnFire2;

        frontXpbar.fillAmount = currentXp / requiredXp;
        backXpbar.fillAmount = currentXp / requiredXp;
        requiredXp = CalculateRequiredXp();
        levelText.text = "Level\n" + level;
    }

    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        UpdateXpUI();
        if (currentXp > requiredXp)
            LevelUp();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput.magnitude > 0.01)
            {
                Vector2 Position = transform.position;
                Position += movementInput * movementSpeed * Time.deltaTime;
                rb.MovePosition(Position);
            }

            Vector2 move = new Vector2(movementInput.x, movementInput.y);
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }

            animator.SetFloat("LookX", lookDirection.x);
            animator.SetFloat("LookY", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            // Handle Dash
            if (isDashing)
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + cooldownDashTime;
                    animator.SetTrigger("Slide");
                    dust.Play();
                    Vector2 Position = transform.position;
                    Position += lookDirection * dashDistance * Time.deltaTime;
                    rb.MovePosition(Position);
                    isDashing = false;
                }
            }

            // Handle Attack
            if (isMeleeAttacking)
            {
                // ===============Sword====================
                SwordAttack();

                // ===============Hammer====================
                // HammerAttack();

                // ===============Scythe====================
                // ScytheAttack();

            }
            if (isRangeAttacking)
            {
                // ===============Gun======================
                // GunAttack();

                // ===============Rifle======================
                // RifleAttack();

                // ===============Bow======================
                ArrowAttack();
            }
        }
    }

    // void Respawn()
    // {
    //     lookDirection = new Vector2(0f, -1f);
    //     transform.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 2");
    //     canMove = true;
    //     animator.SetBool("isDead", false);
    //     currentHealth = maxHealth;
    //     transform.position = respawnLocation.position;
    // }

    public void SwordAttack()
    {
        if (lookDirection.x > 0)
        {
            swordController.AttackRight();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
        else if (lookDirection.x < 0)
        {
            swordController.AttackLeft();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
        else if (lookDirection.y > 0)
        {
            swordController.AttackUp();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
        else if (lookDirection.y < 0)
        {
            swordController.AttackDown();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
        isMeleeAttacking = false;
    }

    public void HammerAttack()
    {
        if (lookDirection.x > 0)
        {
            hammerController.AttackRight();
            //play sfx
            AudioManager.instance.PlaySFX("Hammer");
        }
        else if (lookDirection.x < 0)
        {
            hammerController.AttackLeft();
            //play sfx
            AudioManager.instance.PlaySFX("Hammer");
        }
        else if (lookDirection.y > 0)
        {
            hammerController.AttackUp();
            //play sfx
            AudioManager.instance.PlaySFX("Hammer");
        }
        else if (lookDirection.y < 0)
        {
            hammerController.AttackDown();
            //play sfx
            AudioManager.instance.PlaySFX("Hammer");
        }
        isMeleeAttacking = false;
    }

    public void ScytheAttack()
    {
        if (lookDirection.x > 0)
        {
            scytheController.AttackRight();
            //play sfx
            AudioManager.instance.PlaySFX("Scythe");
        }
        else if (lookDirection.x < 0)
        {
            scytheController.AttackLeft();
            //play sfx
            AudioManager.instance.PlaySFX("Scythe");
        }
        else if (lookDirection.y > 0)
        {
            scytheController.AttackUp();
            //play sfx
            AudioManager.instance.PlaySFX("Scythe");
        }
        else if (lookDirection.y < 0)
        {
            scytheController.AttackDown();
            //play sfx
            AudioManager.instance.PlaySFX("Scythe");
        }
        isMeleeAttacking = false;
    }

    public void RifleAttack()
    {
        if (lookDirection.x > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownRifleTime;
                GameObject bulletGO = Instantiate(rifleBulletPrefab, shootingPoint.position, transform.rotation);
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.right);
                Destroy(bulletGO, 3f);
            }

        }
        if (lookDirection.x < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownRifleTime;
                GameObject bulletGO = Instantiate(rifleBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 180, 0));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.right);
                Destroy(bulletGO, 3f);
            }
        }
        if (lookDirection.y > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownRifleTime;
                GameObject bulletGO = Instantiate(rifleBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, 90));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.up);
                Destroy(bulletGO, 3f);
            }
        }
        if (lookDirection.y < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownRifleTime;
                GameObject bulletGO = Instantiate(rifleBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, -90));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.up);
                Destroy(bulletGO, 3f);
            }
        }
        AudioManager.instance.PlaySFX("Bullet");
        isRangeAttacking = false;
    }

    public void GunAttack()
    {
        if (lookDirection.x > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownGunTime;
                GameObject bulletGO = Instantiate(gunBulletPrefab, shootingPoint.position, transform.rotation);
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.right);
                Destroy(bulletGO, 3f);
            }
        }
        if (lookDirection.x < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownGunTime;
                GameObject bulletGO = Instantiate(gunBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 180, 0));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.right);
                Destroy(bulletGO, 3f);
            }
        }
        if (lookDirection.y > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownGunTime;
                GameObject bulletGO = Instantiate(gunBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, 90));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.up);
                Destroy(bulletGO, 3f);
            }
        }
        if (lookDirection.y < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownGunTime;
                GameObject bulletGO = Instantiate(gunBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, -90));
                bulletGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.up);
                Destroy(bulletGO, 3f);
            }
        }
        AudioManager.instance.PlaySFX("Bullet2");
        isRangeAttacking = false;
    }

    public void ArrowAttack()
    {
        if (lookDirection.x > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownBowTime;
                GameObject arrowGO = Instantiate(arrowBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 180, 0));
                arrowGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.right);
                Destroy(arrowGO, 3f);
            }
        }
        if (lookDirection.x < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownBowTime;
                GameObject arrowGO = Instantiate(arrowBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, 0));
                arrowGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.right);
                Destroy(arrowGO, 3f);
            }
        }
        if (lookDirection.y > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownBowTime;
                GameObject arrowGO = Instantiate(arrowBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, -90));
                arrowGO.GetComponent<Rigidbody2D>().AddForce(500 * transform.up);
                Destroy(arrowGO, 3f);
            }
        }
        if (lookDirection.y < 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + cooldownBowTime;
                GameObject arrowGO = Instantiate(arrowBulletPrefab, shootingPoint.position, Quaternion.Euler(0, 0, 90));
                arrowGO.GetComponent<Rigidbody2D>().AddForce(500 * -transform.up);
                Destroy(arrowGO, 3f);
            }
        }
        AudioManager.instance.PlaySFX("Arrow");
        isRangeAttacking = false;
    }

    //====================Animation Event Function==========================
    public void EndSwordAttack()
    {
        swordController.StopAttack();
    }

    public void EndHammerAttack()
    {
        hammerController.StopAttack();
    }

    public void EndScytheAttack()
    {
        scytheController.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
    // =======================================================================

    public void Hurt(float damage)
    {
        //swordController.StopAttack();
        //hammerController.StopAttack();
        //scytheController.StopAttack();
        currentHealth -= damage;
        AudioManager.instance.PlaySFX("Hit");
        animator.SetTrigger("Hit");
        StartCoroutine(FlashCo());
        lerpTimer = 0f;

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            canMove = false;
            rb.isKinematic = true;
            StartCoroutine(EnableRigidbody(3f));
            StartCoroutine(PlayGameOverUI(2f));
        }
    }

    // Health + XP UI
    public void UpdateHealthUI()
    {
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = currentHealth / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthbar.fillAmount = hFraction;
            backHealthbar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthbar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthbar.color = Color.green;
            backHealthbar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthbar.fillAmount = Mathf.Lerp(fillF, backHealthbar.fillAmount, percentComplete);
        }
        healthText.text = Mathf.Round(currentHealth) + "/" + Mathf.Round(maxHealth);
    }

    public void UpdateXpUI()
    {
        float fXp = frontXpbar.fillAmount;
        float xpFraction = currentXp / requiredXp;
        if (fXp < xpFraction)
        {
            delayTimer += Time.deltaTime;
            backXpbar.fillAmount = xpFraction;
            if (delayTimer > 1)
            {
                lerpExpTimer += Time.deltaTime;
                float percentComplete = lerpExpTimer / 4;
                frontXpbar.fillAmount = Mathf.Lerp(fXp, backXpbar.fillAmount, percentComplete);
            }
        }
        expText.text = Mathf.Round(currentXp) + "/" + requiredXp;
    }

    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;
        lerpTimer = 0f;
    }

    public void IncreaseHealthCap(int level)
    {
        maxHealth += (currentHealth * 0.01f) * ((100 - level) * 0.1f);
        currentHealth += maxHealth * 0.25f;
    }


    // Handle Input System Events
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            movementInput = Vector2.zero;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDashing = true;
        }
        if (context.canceled)
        {
            isDashing = false;
        }
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // isMeleeAttacking = true;
            animator.SetTrigger("SwordAttack");
            // animator.SetTrigger("HammerAttack");
            // animator.SetTrigger("ScytheAttack");
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRangeAttacking = true;
            // animator.SetTrigger("GunAttack");
            animator.SetTrigger("BowAttack");
            // animator.SetTrigger("RifleAttack");
        }
    }

    public void GainExpScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp += xpGained * multiplier;
        }
        else
        {
            currentXp += xpGained;
        }
        lerpExpTimer = 0f;
        delayTimer = 0f;
    }

    private int CalculateRequiredXp()
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }

    public void LevelUp()
    {
        if (level < 20)
        {
            AudioManager.instance.PlaySFX("LevelUp");
            levelUp.Play();
            levelUpSplash.Play();
            level++;
            frontXpbar.fillAmount = 0f;
            backXpbar.fillAmount = 0f;
            currentXp = Mathf.RoundToInt(currentXp - requiredXp);

            IncreaseHealthCap(level);
            requiredXp = CalculateRequiredXp();
            levelText.text = "Level " + level;
        }
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    // Handle Save + Load
    public void LoadData(GameData data)
    {
        this.currentHealth = data.playerCurrentHealth;
        this.maxHealth = data.playerMaxHealth;
        this.transform.position = data.playerCurrentPostion;
        this.requiredXp = data.playerRequiredExp;
        this.currentXp = data.playerCurrentExp;
        this.level = data.playerCurrentLevel;
        //Set health bar to saved current health
        //healthBar.setHealth(currentHealth);
    }

    public void SaveData(GameData data)
    {
        data.playerCurrentHealth = this.currentHealth;
        data.playerMaxHealth = this.maxHealth;
        data.playerCurrentExp = this.currentXp;
        data.playerCurrentPostion = this.transform.position;
        data.playerCurrentLevel = this.level;
        data.playerRequiredExp = this.requiredXp;
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numOfFlashes)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    private IEnumerator EnableRigidbody(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.isKinematic = false;
        // Respawn();
    }

    private IEnumerator PlayGameOverUI(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.instance.PauseMusic("Theme");
        AudioManager.instance.PlaySFX("GameOver");
        gameOverUI.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}
