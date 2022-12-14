using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEventController : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnable;
    [SerializeField] private UnityEvent onStart;

    private void Start()
    {
        onStart.Invoke();
    }

    private void OnEnable()
    {
        onEnable.Invoke();
    }
}
