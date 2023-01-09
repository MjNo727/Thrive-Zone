using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    private PlayerInputActions inputActions;
    Vector2 movementInput;
    public GameObject dialogBox, dialogBox2;
    bool isMeleeAttacking, isRangeAttacking, isDashing;
    public GameObject[] popUps;
    private int popUpIndex;
    public GameObject spawner;

    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Fire1.performed += OnFire1;
        inputActions.Player.Fire2.performed += OnFire2;
    }

    public void Update()
    {
        if (DataPersistanceManager.instance.isNewGame)
        {
            StartCoroutine(TutorialPopup());
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

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isMeleeAttacking = true;
        }
        if (context.canceled)
        {
            isMeleeAttacking = false;
        }
    }

    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRangeAttacking = true;
        }
        if (context.canceled)
        {
            isRangeAttacking = false;
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

    IEnumerator TutorialPopup()
    {
        dialogBox.SetActive(true);
        yield return new WaitForSeconds(5.5f);

        dialogBox2.SetActive(true);
        yield return new WaitForSeconds(5.5f);

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
            if (movementInput.x != 0 && movementInput.y != 0)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 1)
        {
            if (isMeleeAttacking)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (isRangeAttacking)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3)
        {
            if (isDashing)
            {
                popUpIndex++;
            }
        }
        else
        {
            spawner.SetActive(true);
        }
    }
}
