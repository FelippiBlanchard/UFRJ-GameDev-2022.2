using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyTrigger : MonoBehaviour
{
    public UnityEvent onEnemyTrigger;
    public UnityEvent onEnemyUntrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onEnemyTrigger.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onEnemyUntrigger.Invoke();
        }
    }
}
