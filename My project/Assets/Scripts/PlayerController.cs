using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    private Transform respawnLocation;
    public float invicibleTime = 1.5f;
    bool isInvicible;
    float invicibleTimer;
    Animator animator;
    Vector2 movementInput;
    Vector2 lookDirection = new Vector2(0, -1);
    Rigidbody2D rb;
    public int maxHealth = 10;
    public int currentHealth;
    public HealthBar healthBar;

    public float movementSpeed = 3f;
    private PlayerInputActions inputActions;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AudioManager.instance.PlayMusic("Theme");
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }
    void Update()
    {
        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
                isInvicible = false;
        }
    }
    private void FixedUpdate()
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
    }
    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            Debug.Log(currentHealth);

            AudioManager.instance.PlaySFX("Hit");
            animator.SetTrigger("Hit");

            if (isInvicible) return;
            isInvicible = true;
            invicibleTimer = invicibleTime;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth == 0)
            Respawn();

        //Set health bar UI
        healthBar.setHealth(currentHealth);
    }
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

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void LoadData(GameData data)
    {
        this.currentHealth = data.health;
        this.transform.position = data.playerPostion;
        //Set health bar to saved current health
        healthBar.setHealth(currentHealth);
    }

    public void SaveData(GameData data)
    {
        data.health = this.currentHealth;
        data.playerPostion = this.transform.position;
    }
    void Respawn()
    {
        changeHealth(maxHealth);
        transform.position = respawnLocation.position;
    }
}
