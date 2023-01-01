using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    Vector2 movementInput;
    bool isAttacking, isDashing;
    public GameObject[] popUps;
    private int popUpIndex;
    public GameObject spawner;
    public float waitTime = 2f;

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Attack.performed += OnAttack;
    }

    public void Update()
    {
        if (DataPersistanceManager.instance.isNewGame)
        {
            for (int i = 0; i < popUps.Length; i++)
            {
                if (i == popUpIndex)
                {
                    popUps[i].SetActive(true);
                }
                else
                {
                    popUps[i].SetActive(false);
                }
            }
            if (popUpIndex == 0)
            {
                if (movementInput.x > 0.01 || movementInput.y > 0.01)
                {
                    if (movementInput.y > 0.01 || movementInput.x > 0.01)
                        popUpIndex++;
                }
            }
            else if (popUpIndex == 1)
            {
                if (isAttacking)
                {
                    popUpIndex++;
                }
            }
            else if (popUpIndex == 2)
            {
                if (isDashing)
                {
                    popUpIndex++;
                }
            }
            else
            {
                if (waitTime <= 0)
                {
                    spawner.SetActive(true);
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            spawner.SetActive(true);
        }
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttacking = true;
        }
        if (context.canceled)
        {
            isAttacking = false;
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
}
