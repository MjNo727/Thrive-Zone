using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button loadGameBtn;
    public FadeAnimator fadeAnimator;
    private void Start(){
        AudioManager.instance.PlayMusic("MainMenuTheme");
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
            AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");

        if(!DataPersistanceManager.instance.HasGameData()){
            loadGameBtn.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        DataPersistanceManager.instance.NewGame();
        DataPersistanceManager.instance.SaveGame();
        // SceneManager.LoadSceneAsync("GameplayScene");
        fadeAnimator.FadeToScreen("GameplayScene");
    }

    public void OnLoadGameClicked()
    {
        DataPersistanceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("GameplayScene");
    }

    public void OnQuitGameClicked(){
        Application.Quit();
    }
}
