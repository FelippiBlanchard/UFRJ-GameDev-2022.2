using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidSubscriberEvent : MonoBehaviour
{
    public VoidScriptableGameEvent GameEvent;
    [Space(10)]
    public UnityEvent Methods;

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
