using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameEvent : UnityEvent<object> { }

public class GameEventListener : MonoBehaviour
{
    [Header("Event Settings")]
    [SerializeField] private string _eventName;

    [Header("Response")]
    [SerializeField] private UnityGameEvent _response;

    [Header("Options")]
    [SerializeField] private bool _listenOnStart = true;
    [SerializeField] private bool _logEvents = false;

    private void Start()
    {
        if (_listenOnStart)
        {
            StartListening();
        }
    }

    public void StartListening()
    {
        EventManager.Instance.StartListening(_eventName, OnEventTriggered);

        if (_logEvents)
        {
            Debug.Log($"Started listening to event: {_eventName} on {gameObject.name}");
        }
    }

    public void StopListening()
    {
        EventManager.Instance.StopListening(_eventName, OnEventTriggered);

        if (_logEvents)
        {
            Debug.Log($"Stopped listening to event: {_eventName} on {gameObject.name}");
        }
    }

    private void OnEventTriggered(object eventData)
    {
        if (_logEvents)
        {
            Debug.Log($"Event {_eventName} received on {gameObject.name} with data: {eventData}");
        }

        _response?.Invoke(eventData);
    }

    private void OnDestroy()
    {
        StopListening();
    }

    private void OnEnable()
    {
        StartListening();
    }

    private void OnDisable()
    {
        StopListening();
    }
}