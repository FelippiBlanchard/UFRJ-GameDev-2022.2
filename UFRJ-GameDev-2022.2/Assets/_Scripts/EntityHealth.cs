using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EntityHealth : MonoBehaviour
{

    public UnityEvent onEntityDeath;
    public UnityEvent onEntityHurt;
    public UnityEvent onEntityHurt2;
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

        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (health >= 50)
            {
                onEntityHurt.Invoke();   
            }
            else
            {
                onEntityHurt2.Invoke(); 
            }
        }
    }

    void Die()
    {
        onEntityDeath.Invoke();
    }

}
