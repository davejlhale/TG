using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Events")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> eventListeners =
                new List<GameEventListener>();

    public void Raise()
    {
        Debug.Log(eventListeners.Count);
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
    }

    public void RegisterListeners(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void unRegisterListeners(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
