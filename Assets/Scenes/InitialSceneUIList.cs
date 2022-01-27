using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSceneUIList : MonoBehaviour
{
    [SerializeField]
    private SceneUIList _sceneUIList;
    private GameObject currentInitializers;

    public SceneUIList SceneUIList { get { return _sceneUIList; } }

    void OnEnable()
    {
        SceneController.OnScenePreLoading += SceneController_OnScenePreLoading; 
        SceneController.OnSceneLoaded += SceneController_OnSceneLoaded;
    }

    private void SceneController_OnScenePreLoading(string sceneName)
    {
        Debug.Log("SceneController_OnScenePreLoading");
    
    }

    private void SceneController_OnSceneLoaded(string sceneName)
    {
        Debug.Log("SceneController_OnSceneLoaded");
    }

    private void OnDisable()
    {
        SceneController.OnScenePreLoading -= SceneController_OnScenePreLoading;
        SceneController.OnSceneLoaded -= SceneController_OnSceneLoaded;
    }
}
