using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;

    public void CallMainMenu()
    {
        Time.timeScale = 1;
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
        public void CallMainMenu2()
    {
        Time.timeScale = 1;
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
        public void CallMainMenu3()
    {
        Time.timeScale = 1;
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
