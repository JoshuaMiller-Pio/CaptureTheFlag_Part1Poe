using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSuper : MonoBehaviour
{
    private int _health;
    private int _maxHealth = 100;
    private int _damage;
    public EventHandler Flagdropped;
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


    public void damage()
    {
        Health -= Damage;
        if (Health <= 0)
        {
            Death();
        } 
    }

    private void Start()
    {
    }

    private void Awake()
    {
        _damage = 30;
        _health = 100;
    }

    
    public abstract void Death();
}
