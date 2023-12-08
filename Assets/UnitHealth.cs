using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth
{
    private int _currentHealth;
    private int _currentMaxhealth;

    public int Health => _currentHealth;
    public int MaxHealth => _currentMaxhealth;

    public UnitHealth(int health, int maxHealth)
    {
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    public void DmgUnit(int dmgAmount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= dmgAmount;
        }
    }

    public void HealUnit(int HealAmount)
    {
        if (_currentHealth < _currentMaxHealth)
        {
            _currentHealth += HealAmount;
        }
        if (_currentHealth > _currentMaxHealth)
        {
            _currentHealth == _currentMaxHealth;
        }
    }
}