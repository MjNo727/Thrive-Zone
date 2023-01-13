using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    
    [SerializeField] private Button mapGameBtn;
    [SerializeField] private Button mapGame2Btn;
    [SerializeField] private Button mapGame3Btn;
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
    public void OnMapGameClicked()
    {
        DataPersistanceManager.instance.NewGame();
        
        DataPersistanceManager.instance.scene = "GameplayScene";
        DataPersistanceManager.instance.SaveGame();
        // SceneManager.LoadSceneAsync("GameplayScene");
        fadeAnimator.FadeToScreen("GameplayScene");
    }

        public void OnMapGame2Clicked()
    {
        DataPersistanceManager.instance.NewGame();
        
        DataPersistanceManager.instance.scene = "GameplayScene2";
        DataPersistanceManager.instance.SaveGame();
        // SceneManager.LoadSceneAsync("GameplayScene");
        fadeAnimator.FadeToScreen("GameplayScene2");
        
    }

    public void OnMapGame3Clicked()
    {
        DataPersistanceManager.instance.NewGame();
        
        DataPersistanceManager.instance.scene = "GameplayScene3";
        DataPersistanceManager.instance.SaveGame();
        // SceneManager.LoadSceneAsync("GameplayScene");
        fadeAnimator.FadeToScreen("GameplayScene3");
        
    }

    public void OnLoadGameClicked()
    {
        DataPersistanceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync(DataPersistanceManager.instance.scene);
    }

    public void OnQuitGameClicked(){
        Application.Quit();
    }
}
