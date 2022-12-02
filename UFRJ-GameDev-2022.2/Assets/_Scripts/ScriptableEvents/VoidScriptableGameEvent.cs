using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "GenericScripts/ScriptableVoidEvent")]
public class VoidScriptableGameEvent : ScriptableObject
{
    [SerializeField] private UnityEvent _event;

    public void Register(UnityAction method)
    {
        _event.AddListener(method);
    }

    public void Trigger()
    {
        _event.Invoke();
    }

    public void Unregister(UnityAction method)
    {
        _event.RemoveListener(method);
    }

    private void Awake()
    {
        _event = new UnityEvent();
    }
}
