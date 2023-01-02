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
    public float dashDistance = 80f;
    private bool isDashing, isAttacking;

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
    public Transform attackHitbox;
    public SwordController swordController;

    Vector2 mousePos;

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
        inputActions.Player.Attack.performed += OnAttack;

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
                animator.SetTrigger("Slide");
                dust.Play();
                Vector2 Position = transform.position;
                Position += lookDirection * dashDistance * Time.deltaTime;
                rb.MovePosition(Position);
                isDashing = false;
            }

            // Handle Attack
            if (isAttacking)
            {
                // ===============Sword====================
                SwordAttack();
                // ===============Gun======================
                // GunAttack();
            }

        }
    }

    void Respawn()
    {
        lookDirection = new Vector2(0f, -1f);
        transform.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Layer 2");
        canMove = true;
        animator.SetBool("isDead", false);
        currentHealth = maxHealth;
        transform.position = respawnLocation.position;
    }

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
        else if(lookDirection.y > 0){
            swordController.AttackUp();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
        else if(lookDirection.y < 0){
            swordController.AttackDown();
            //play sfx
            AudioManager.instance.PlaySFX("Sword");
        }
    }

    public void EndSwordAttack(){
        swordController.StopAttack();
    }

    public void Hurt(float damage)
    {
        //Set health bar UI
        //healthBar.setHealth(currentHealth);

        swordController.StopAttack();
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

    public void IncreaseDamageCap(float amount)
    {
        swordController.swordDamage += amount;
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("SwordAttack");
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
            IncreaseDamageCap(0.5f);
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
        Respawn();
    }
}
