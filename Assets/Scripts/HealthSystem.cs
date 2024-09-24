using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDeadEvent;
    public event Action<int,int> OnHealthChangedEvent;

    [SerializeField] private int maxHealth = 100;
    private int health;
    public float healthPercentage { get => (float)health / maxHealth; }

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damageAmount) 
    {
        health = Mathf.Clamp(health-damageAmount,0,maxHealth);

        OnHealthChangedEvent?.Invoke(health,maxHealth);

        if (health == 0) 
        {
            OnDeadEvent?.Invoke();
        }
    }
}
