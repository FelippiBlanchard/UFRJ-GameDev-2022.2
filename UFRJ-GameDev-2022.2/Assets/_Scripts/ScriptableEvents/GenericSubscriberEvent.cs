using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericSubscriberEvent<T> : MonoBehaviour
{
    public GenericScriptableEvent<T> GameEvent;
    [Space(10)]
    public UnityEvent<T> Methods;

    public void Subscribe()
    {
        GameEvent.Register(Methods.Invoke);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        GameEvent.Unregister(Methods.Invoke);
    }
}
