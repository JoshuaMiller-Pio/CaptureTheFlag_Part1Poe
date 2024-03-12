using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    private int _health;
    private int _maxHealth = 10;
    private int _damage;

    public int MaxHealth
    {
        get => _maxHealth;
    }
    public int Health
    {
        get => _health;
        set => _health = value;
    }
    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }

    
    
    
    
    private void Awake()
    {
        _damage = 5;
        _health = 10;
    }

    
    public abstract void Death();
}
