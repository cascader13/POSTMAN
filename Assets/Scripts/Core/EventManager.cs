using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventManager>();

                if (_instance == null)
                {
                    GameObject emObject = new GameObject("EventManager");
                    _instance = emObject.AddComponent<EventManager>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<string, Action<object>> _eventDictionary;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _eventDictionary = new Dictionary<string, Action<object>>();

        Debug.Log("EventManager initialized");
    }


    // Подписка на событие
    public void StartListening(string eventName, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] += listener;
        }
        else
        {
            _eventDictionary.Add(eventName, listener);
        }
    }

    // Отписка от события
    public void StopListening(string eventName, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] -= listener;
        }
    }

    // Запуск события
    public void TriggerEvent(string eventName, object eventData = null)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            Debug.Log($"Event triggered: {eventName} with data: {eventData}");
            _eventDictionary[eventName]?.Invoke(eventData);
        }
        else
        {
            Debug.LogWarning($"No listeners for event: {eventName}");
        }
    }

    // Очистка всех событий (при смене сцены)
    public void ClearAllEvents()
    {
        _eventDictionary.Clear();
        Debug.Log("All events cleared");
    }


    // Проверка существования события
    public bool HasEvent(string eventName)
    {
        return _eventDictionary.ContainsKey(eventName);
    }

    // Получение количества слушателей
    public int GetListenerCount(string eventName)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            return _eventDictionary[eventName]?.GetInvocationList().Length ?? 0;
        }
        return 0;
    }
}