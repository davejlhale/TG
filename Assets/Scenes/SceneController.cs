using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    //delegate event raising
    public delegate void SceneLoadedEvent(string sceneName);
    public static event SceneLoadedEvent OnSceneLoaded;
    public delegate void SceneAboutToUnloadEvent(string sceneUnloadingName);
    public static event SceneAboutToUnloadEvent OnScenePreLoading;
  
 
    private string _currentSceneName;
    public string CurrentSceneName
    {
        get { return _currentSceneName; }
        set
        {
            if (_currentSceneName != value)
            {
                //Destroy ui with _currentname
                _currentSceneName = value;
                //load current sceneSO? so that UI flips
            }

        }
    }

    // Start is called before the first frame update

    protected static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<SceneController>();

            if (instance != null)
                return instance;
            Create();
            return instance;
        }
    }

    public string FirstPlayScene { get { return _firstPlayScene; } }

    [SerializeField]
    private GameObject SceneInitializersPrefab;
  
    [Tooltip("First scene to load on first play click")]
    [SerializeField]
    private string _firstPlayScene;
   

    [Tooltip("Canvas for fading in and out screen transitions")]
    [SerializeField]
    private Image screenLoader;
    private string _preloadScreen;
    private string sceneToLoad;
    private GameObject currentInitializers;
    private GameObject savedInitializers;

    public static SceneController Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        instance = sceneControllerGameObject.AddComponent<SceneController>();
        return instance;
    }
    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(transform.root.gameObject);
        _preloadScreen = SceneManager.GetActiveScene().name;
    }
    
    public void OnSceneChangeRequest(string sceneName)
    {
        sceneToLoad = sceneName;
        if (sceneToLoad == string.Empty)
        {
            Debug.Log("OnSceneChangeRequest for null / trying to load scene : " + FirstPlayScene);
            sceneToLoad = FirstPlayScene;
        }

        if (_currentSceneName == sceneToLoad)
        {
            Debug.Log("a request to reload active scene.");
        }
      
        if (sceneToLoad == "_preload" )
        {
            Debug.Log("ignoring request to load _preload scene.");
            return;
        }

        int screenIndex = sceneIndexFromName(sceneToLoad);
        if (screenIndex == -1)
        {
            Debug.LogError("Scene " + sceneToLoad + " not found in build"); 
        }
        else
        {
            
            Destroy(GameObject.Find("Scene Initializers"));
            LoadScene(sceneToLoad);
        }
       
    }
    private int sceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            string testedScreen = NameFromIndex(i);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
    private void LoadScene(string sceneToLoad, float duration = 1, float waitTime = 0)
    {
        StartCoroutine(instance.FadeScreen(sceneToLoad, duration, waitTime));
    }
    private IEnumerator FadeScreen(string sceneToLoad, float duration, float waitTime)
    {
        screenLoader.gameObject.SetActive(true);

        //raise unloading event for UI to fade etc
        OnScenePreLoading.Invoke(sceneToLoad);
      
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            screenLoader.color = new Color(0, 0, 0, t);
            yield return null;
        }
       
        Debug.Log("currentscene "+_currentSceneName +"/n sceneToLoad "+sceneToLoad + " :: _preloadScreen " + _preloadScreen);
        //if conditions OK try UNLOAD
        if (_currentSceneName != null && sceneToLoad != _preloadScreen )
        {
            yield return SceneManager.UnloadSceneAsync(_currentSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            if (SceneManager.GetSceneByName(_currentSceneName) != null)
            {
                Debug.Log("Failed to unloaded " + _currentSceneName);
            }
            else
            {
                Debug.Log("Unloaded " + _currentSceneName); 
            }
        }

        AsyncOperation AO = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!AO.isDone)
            yield return null;
        
        Scene tmpScene = SceneManager.GetSceneByName(sceneToLoad);
        if (tmpScene.IsValid())
        {
            SceneManager.SetActiveScene(tmpScene);
            _currentSceneName = sceneToLoad;
        }
        yield return new WaitForSeconds(waitTime);

        OnSceneLoaded?.Invoke(sceneToLoad);
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            screenLoader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        if (!GameObject.Find("Scene Initializers"))
        {
            Instantiate(SceneInitializersPrefab);
        }
        screenLoader.gameObject.SetActive(false);
    }
}