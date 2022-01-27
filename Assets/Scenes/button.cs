using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    [Tooltip("Events we can use GameEvent SO .")]
    [Header("Events")]
    public GameEvent buttonPressed;
    public void onButtonPressed()
    {
        Debug.Log("??");
        buttonPressed.Raise();
        
        //SceneController.SceneChangeRequest("test");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
  
}
