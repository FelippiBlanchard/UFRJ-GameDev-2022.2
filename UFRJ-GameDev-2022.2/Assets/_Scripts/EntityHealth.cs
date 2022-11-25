using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EntityHealth : MonoBehaviour
{

    public UnityEvent onEntityDeath;
    public UnityEvent onEntityHurt;
    public UnityEvent onEntityHeal;

    int health;
    int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }

    public void TakeHeal(int heal)
    {
        health = Math.Max(maxHealth, health + heal);
        onEntityHeal.Invoke();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        onEntityHurt.Invoke();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        onEntityDeath.Invoke();
    }

}
