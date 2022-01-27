using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{

    [Tooltip("event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListeners(this);
    }

    private void OnDisable()
    {
        Event.unRegisterListeners(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
