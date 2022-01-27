using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Initializers/SceneUIList")]
public class SceneUIList : ScriptableObject
{
    public List<Canvas> CanvasPrefabToDisplay;
    public bool replaceCurrentUI;
    public bool rememberOldUI;

    [Tooltip("True to prevent In Game movement etc")]
    public bool blockPlayerInput;
    // Start is called before the first frame update
    void Add()
    {
        
    }

    // Update is called once per frame
    void Remove()
    {
        
    }
}
