using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAnimator : MonoBehaviour
{
    public Animator animator;
    private string screenToLoad;

    public void FadeToScreen(string screenIndex){
        screenToLoad = screenIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete(){
        SceneManager.LoadSceneAsync(screenToLoad);
    }
}
