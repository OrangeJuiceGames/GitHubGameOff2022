using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController
{
    public event Action OnReachZeroHealth;
    public event Action OnReachLowHealth;
    public event Action OnReachCriticalHealth;

    private float _maxHealth;
    private float _currentHealth;

    private float _lowHealthThreshold;
    private float _criticalHealthThreshold;
    public HealthController(float totalHealth, float lowHealthPercent, float criticalHealthPercent)
    {
        _maxHealth = totalHealth;
        _currentHealth = totalHealth;

        _lowHealthThreshold = CalculateThreshold(lowHealthPercent, totalHealth);
        _criticalHealthThreshold = CalculateThreshold(criticalHealthPercent, totalHealth);
    }

    public void DamageHealth(float damageAmount)
    {
        if (_currentHealth <= 0) { return; }

        float newHealthAmount = damageAmount - _currentHealth;

        if (newHealthAmount <= 0)
        {
            OnReachZeroHealth?.Invoke();
        }
        else if (newHealthAmount <= _criticalHealthThreshold && _currentHealth > _criticalHealthThreshold)
        {
            OnReachCriticalHealth?.Invoke();
        }
        else if (newHealthAmount <= _lowHealthThreshold && _currentHealth > _lowHealthThreshold)
        {
            OnReachLowHealth?.Invoke();
        }

        _currentHealth = newHealthAmount;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    private float CalculateThreshold(float percentage, float maxHealth)
    {
        if (percentage > 1 || percentage <= 0)
        {
            throw new Exception("Health percentage must be between 0 and 1!");
        }

        return maxHealth * percentage;
    }

}
