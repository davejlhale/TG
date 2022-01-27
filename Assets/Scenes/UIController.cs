using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[Serializable]
public class UIController : MonoBehaviour
{
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup options;
    [SerializeField] private CanvasGroup town;
    [SerializeField] private CanvasGroup wilderness;
    
    
    private bool fading;
    private int memory = 1;
    public enum UIStates { toggleMainMenu = 1, toggleoptions = 2, enterWildreness = 3, enterTown = 4 }

        private IEnumerator Fader()
    {
        fading = true;
        while (fading)
        {
            Fade(town);
            Fade(mainMenu);
            Fade(options);
            Fade(wilderness);
            yield return null;
        }    
    }
    private void FadeIn(CanvasGroup cg)
    {
        if (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime;
            if (cg.alpha >= 1)
            {
                fadeIn = false;
                fading = false;
            }
        }
    }
    private void FadeOut(CanvasGroup cg)
    {
        if (cg.alpha >= 0)
        {
            cg.alpha -= Time.deltaTime;
            if (cg.alpha == 1)
            {
                fadeOut = false;
                fading = false;
            }
        }
    }
        private void Fade(CanvasGroup cg)
    {
        if (fadeIn)
        {
           
            FadeIn(cg);
        }
        if (fadeOut)
        {
            FadeOut(cg);
        }
    }
    public void ShowUI()
    {
        fadeIn = true;
    }
    public void hideUI()
    {
        fadeOut = true;
    }
   
    public void ChangeUIState(int ChangeUiStateTo)
    {//toggleMainMenu=1, toggleoptions=2, enterWildreness=3, enterTown=4
        Debug.Log(ChangeUiStateTo);
        switch (ChangeUiStateTo)
        {
            case 1:
                // code block
                break;
            case 2:
                toggleOptions();
                break;
            case 3:
                enterWilderness();
                break;
            case 4:
                enterTown();
                break;
            default:
                // code block
                break;
        }
    }

    private void enterTown()
    {
        mainMenu.gameObject.SetActive(false);
        wilderness.gameObject.SetActive(false);
        options.gameObject.SetActive(false);
        town.alpha = 0;
        town.gameObject.SetActive(true);
    }
    private void enterWilderness()
    {
        mainMenu.gameObject.SetActive(false);
        town.gameObject.SetActive(false); 
        options.gameObject.SetActive(false);
        town.alpha = 0;
        wilderness.gameObject.SetActive(true);

    }
    public void toggleOptions()
    {
        if (memory == 1)
            mainMenu.gameObject.SetActive(!mainMenu.gameObject.activeSelf);
        else if (memory == 3)
            wilderness.gameObject.SetActive(!wilderness.gameObject.activeSelf);
        else if (memory == 4)
            town.gameObject.SetActive(!town.gameObject.activeSelf);
        options.gameObject.SetActive(!options.gameObject.activeSelf);
    }
    public void onMainMenuSelection(bool toggle)
    {
        if (!toggle)
            mainMenu.gameObject.SetActive(false);
        else
            mainMenu.gameObject.SetActive(false);
    }
    
    void OnEnable()
    {
        //delegate event recieving
        SceneController.OnScenePreLoading += SceneController_OnScenePreLoading;
        SceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneController.OnSceneLoaded -= SceneController_OnSceneLoaded;
        SceneController.OnScenePreLoading -= SceneController_OnScenePreLoading;
    }

    private void SceneController_OnSceneLoaded(string sceneName)
    {
        fadeIn = true;
        fadeOut = false;
        StartCoroutine(Fader());
    }

    private void SceneController_OnScenePreLoading(string sceneName)
    {
        fadeOut = true;
        fadeIn = false;
        StartCoroutine(Fader());
    }

   
    

   

   
    
}

