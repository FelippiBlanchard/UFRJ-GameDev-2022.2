using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericScriptableEvent<T> : ScriptableObject
{
    [SerializeField] private UnityEvent<T> _event;

    public void Register(UnityAction<T> method)
    {
        _event.AddListener(method);
    }

    public void Trigger(T parameter)
    {
        _event.Invoke(parameter);
    }
    public void Unregister(UnityAction<T> method)
    {
        _event.RemoveListener(method);
    }

    private void Awake()
    {
        _event = new UnityEvent<T>();
    }
}
