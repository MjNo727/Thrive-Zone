using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VictoryMenu : MonoBehaviour
{
    public GameObject victoryMenuUI;

    public void CallMainMenu()
    {
        Time.timeScale = 1;
        victoryMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ToStage2()
    {
        Time.timeScale = 1;
        victoryMenuUI.SetActive(false);
        SceneManager.LoadSceneAsync("GameplayScene2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
