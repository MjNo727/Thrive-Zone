using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private InputAction menu;
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        menu = inputActions.UI.Pause;
        menu.Enable();
        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void CallMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        isPaused = false;
    }

        public void CallMainMenu2(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        isPaused = false;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame(){
        DataPersistanceManager.instance.SaveGame();
    }
}
